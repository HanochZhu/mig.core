using System;
using System.Threading.Tasks;
using Unity.SharpZipLib.Utils;
using UnityEngine;

namespace Mig.Core.TaskPattern
{
    public class UnzipProjectFileTask : TaskHandlerBase
    {
        public string saveZipPath;
        public string extractPath;

        public UnzipProjectFileTask(string saveZipPath, string extractPath, Action<bool> taskCallback) : base(taskCallback)
        {
            this.saveZipPath = saveZipPath; 
            this.extractPath = extractPath;
        }

        public override async void Execute()
        {
            if (string.IsNullOrEmpty(saveZipPath) || string.IsNullOrEmpty(extractPath))
            {
                Debug.Log($"You must set saveZipPath to extractPath before Unzip");
                this.m_taskCallback?.Invoke(false);
                return;
            }

            await Task.Run(() =>
            {
                ZipUtility.UncompressFromZip(saveZipPath, string.Empty, extractPath);
            });

            this.m_taskCallback?.Invoke(true);

            base.Execute();
        }

    }

}
