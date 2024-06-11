using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Mig.Core.TaskPattern
{
    public class LoadingFromFTPTask : TaskHandlerBase
    {

        private string downloadAddress;
        private string saveZipPath;

        public LoadingFromFTPTask(string downloadAddress, string savePath, Action<bool> taskCallback) : base(taskCallback)
        {
            this.downloadAddress = downloadAddress;
            this.saveZipPath = savePath;
        }

        public override async void Execute()
        {
            if (string.IsNullOrEmpty(downloadAddress) || string.IsNullOrEmpty(saveZipPath))
            {
                Debug.LogWarning($"You must set downloadAddress to saveZipPath before download");
                m_taskCallback?.Invoke(false);
                return;
            }
            var result = await FTPClient.DownloadToFileAsync(downloadAddress, saveZipPath, true);

            if (!result)
            {
                Debug.LogError($"Failed to download file {downloadAddress} to {saveZipPath}");
                m_taskCallback?.Invoke(false);
                return;
            }

            base.Execute();
        }

    }
}

