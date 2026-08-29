using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Mig.Core
{
    public class HttpRemoteStorage : IRemoteStorage
    {
        [Serializable]
        private class ProjectListResponse
        {
            public ProjectDto[] projects;
        }

        [Serializable]
        private class ProjectDto
        {
            public string name;
        }

        public string BackendName => "http";

        public float Progress { get; private set; }

        public async Task<IReadOnlyList<string>> ListProjectsAsync(CancellationToken token)
        {
            var names = new List<string>();
            using var request = CreateRequest(UnityWebRequest.kHttpVerbGET, "/v1/projects");
            await UnityWebRequestTask.Send(request, token);
            if (token.IsCancellationRequested || !UnityWebRequestTask.IsSuccess(request))
            {
                if (!token.IsCancellationRequested)
                {
                    Debug.LogWarning($"[Mig] HTTP list failed: {request.error} {request.downloadHandler?.text}");
                }
                return names;
            }

            var json = request.downloadHandler.text;
            var parsed = JsonUtility.FromJson<ProjectListResponse>(json);
            if (parsed?.projects == null)
            {
                return names;
            }

            foreach (var project in parsed.projects)
            {
                if (!string.IsNullOrEmpty(project.name))
                {
                    names.Add(project.name);
                }
            }

            return names;
        }

        public async Task<byte[]> DownloadThumbnailAsync(string projectName, CancellationToken token)
        {
            using var request = CreateRequest(UnityWebRequest.kHttpVerbGET, $"/v1/projects/{Uri.EscapeDataString(projectName)}/thumbnail");
            await UnityWebRequestTask.Send(request, token);
            if (token.IsCancellationRequested || !UnityWebRequestTask.IsSuccess(request))
            {
                return Array.Empty<byte>();
            }

            return request.downloadHandler.data ?? Array.Empty<byte>();
        }

        public async Task<bool> DownloadPackageAsync(string projectName, string localPath, CancellationToken token)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(localPath) ?? ".");
            var succeeded = false;
            using (var request = CreateRequest(UnityWebRequest.kHttpVerbGET, $"/v1/projects/{Uri.EscapeDataString(projectName)}/package"))
            {
                request.downloadHandler = new DownloadHandlerFile(localPath);
                Progress = 0f;
                await UnityWebRequestTask.Send(request, token);
                Progress = 1f;
                succeeded = !token.IsCancellationRequested && UnityWebRequestTask.IsSuccess(request);
                if (!succeeded)
                {
                    Debug.LogError($"[Mig] HTTP package download failed: {request.error}");
                }
            }

            if (!succeeded || !File.Exists(localPath) || new FileInfo(localPath).Length == 0)
            {
                TryDeleteLocalFile(localPath);
                return false;
            }

            return true;
        }

        public async Task<bool> UploadThumbnailAsync(string projectName, byte[] pngBytes)
        {
            return await UploadBytes($"/v1/projects/{Uri.EscapeDataString(projectName)}/thumbnail", pngBytes, "image/png");
        }

        public async Task<bool> UploadPackageAsync(string projectName, Stream packageStream)
        {
            var bytes = ReadAllBytes(packageStream);
            return await UploadBytes($"/v1/projects/{Uri.EscapeDataString(projectName)}/package", bytes, "application/octet-stream");
        }

        private async Task<bool> UploadBytes(string path, byte[] bytes, string contentType)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return false;
            }

            using var request = CreateRequest(UnityWebRequest.kHttpVerbPUT, path);
            request.uploadHandler = new UploadHandlerRaw(bytes) { contentType = contentType };
            request.downloadHandler = new DownloadHandlerBuffer();
            Progress = 0f;
            await UnityWebRequestTask.Send(request);
            Progress = 1f;
            if (!UnityWebRequestTask.IsSuccess(request))
            {
                Debug.LogError($"[Mig] HTTP upload failed: {request.error} {request.downloadHandler?.text}");
                return false;
            }

            return true;
        }

        private static UnityWebRequest CreateRequest(string method, string path)
        {
            var request = new UnityWebRequest(SyncSettings.BaseUrl.TrimEnd('/') + path, method);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + SyncSettings.ApiToken);
            request.SetRequestHeader("X-Mig-Account", AccountManager.GetCurrentAccountID());
            return request;
        }

        private static void TryDeleteLocalFile(string localPath)
        {
            try
            {
                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Mig] Failed to remove incomplete package {localPath}: {ex.Message}");
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            if (stream is MemoryStream memory)
            {
                return memory.ToArray();
            }

            using var copy = new MemoryStream();
            stream.CopyTo(copy);
            return copy.ToArray();
        }
    }
}
