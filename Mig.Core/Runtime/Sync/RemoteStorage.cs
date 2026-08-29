using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Mig.Core
{
    public static class RemoteStorage
    {
        private static IRemoteStorage current;

        public static IRemoteStorage Current
        {
            get
            {
                if (current == null)
                {
                    Reload();
                }

                return current;
            }
        }

        public static string BackendName => Current.BackendName;

        public static float Progress => Current.Progress;

        public static bool IsConfigured
        {
            get
            {
                SyncSettings.EnsureLoaded();
                if (SyncSettings.PreferFtp)
                {
                    return FTPClient.HasConfiguredHost();
                }

                return SyncSettings.HasHttpEndpoint || FTPClient.HasConfiguredHost();
            }
        }

        public static void Reload()
        {
            SyncSettings.EnsureLoaded();
            if (SyncSettings.PreferFtp && FTPClient.HasConfiguredHost())
            {
                current = new FtpRemoteStorage();
            }
            else if (SyncSettings.HasHttpEndpoint)
            {
                current = new HttpRemoteStorage();
            }
            else if (FTPClient.HasConfiguredHost())
            {
                current = new FtpRemoteStorage();
            }
            else
            {
                current = new HttpRemoteStorage();
            }

            Debug.Log($"[Mig] Remote storage backend: {current.BackendName}");
        }

        public static Task<IReadOnlyList<string>> ListProjectsAsync(CancellationToken token) =>
            Current.ListProjectsAsync(token);

        public static Task<byte[]> DownloadThumbnailAsync(string projectName, CancellationToken token) =>
            Current.DownloadThumbnailAsync(projectName, token);

        public static Task<bool> DownloadPackageAsync(string projectName, string localPath, CancellationToken token = default) =>
            Current.DownloadPackageAsync(projectName, localPath, token);

        public static Task<bool> UploadThumbnailAsync(string projectName, byte[] pngBytes) =>
            Current.UploadThumbnailAsync(projectName, pngBytes);

        public static Task<bool> UploadPackageAsync(string projectName, Stream packageStream) =>
            Current.UploadPackageAsync(projectName, packageStream);
    }
}
