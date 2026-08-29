using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Mig.Core
{
    public class FtpRemoteStorage : IRemoteStorage
    {
        public string BackendName => "ftp";

        public float Progress => FTPClient.GetUpLoadPercentage();

        public Task<IReadOnlyList<string>> ListProjectsAsync(CancellationToken token)
        {
            var names = new List<string>();
            if (!FTPClient.HasConfiguredHost())
            {
                return Task.FromResult<IReadOnlyList<string>>(names);
            }

            var folders = FTPClient.GetFTPDirList(FTPClient.GetCurrentFTPDirRoot()) ?? new List<string>();
            foreach (var folder in folders)
            {
                var name = Path.GetFileNameWithoutExtension(folder);
                if (!string.IsNullOrEmpty(name))
                {
                    names.Add(name);
                }
            }

            return Task.FromResult<IReadOnlyList<string>>(names);
        }

        public async Task<byte[]> DownloadThumbnailAsync(string projectName, CancellationToken token)
        {
            var address = FTPClient.CombineUrl(FTPClient.GetCurrentFTPDirRoot(), projectName, projectName + ".png");
            return await FTPClient.DownloadToBytesAsync(address, token) ?? Array.Empty<byte>();
        }

        public async Task<bool> DownloadPackageAsync(string projectName, string localPath, CancellationToken token)
        {
            var address = FTPClient.CombineUrl(FTPClient.GetCurrentFTPDirRoot(), projectName, projectName + ".mig");
            return await FTPClient.DownloadToFileAsync(address, localPath, true);
        }

        public async Task<bool> UploadThumbnailAsync(string projectName, byte[] pngBytes)
        {
            EnsureProjectDir(projectName);
            var success = false;
            var address = FTPClient.CombineUrl(FTPClient.GetCurrentFTPDirRoot(), projectName, projectName + ".png");
            await FTPClient.UploadBytes(address, pngBytes, result => success = result);
            return success;
        }

        public async Task<bool> UploadPackageAsync(string projectName, Stream packageStream)
        {
            EnsureProjectDir(projectName);
            var success = false;
            var address = FTPClient.CombineUrl(FTPClient.GetCurrentFTPDirRoot(), projectName, projectName + ".mig");
            await FTPClient.UploadStream(address, packageStream, result => success = result);
            return success;
        }

        private static void EnsureProjectDir(string projectName)
        {
            var dir = FTPClient.CombineUrl(FTPClient.GetCurrentFTPDirRoot(), projectName);
            if (!FTPClient.DirectoryIsExist(dir))
            {
                FTPClient.MakeDir(dir);
            }
        }
    }
}
