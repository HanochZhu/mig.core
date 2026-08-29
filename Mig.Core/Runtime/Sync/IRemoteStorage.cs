using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mig.Core
{
    public interface IRemoteStorage
    {
        string BackendName { get; }

        float Progress { get; }

        Task<IReadOnlyList<string>> ListProjectsAsync(CancellationToken token);

        Task<byte[]> DownloadThumbnailAsync(string projectName, CancellationToken token);

        Task<bool> DownloadPackageAsync(string projectName, string localPath, CancellationToken token);

        Task<bool> UploadThumbnailAsync(string projectName, byte[] pngBytes);

        Task<bool> UploadPackageAsync(string projectName, Stream packageStream);
    }
}
