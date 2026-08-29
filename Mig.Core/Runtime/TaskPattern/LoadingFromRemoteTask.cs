using System;
using UnityEngine;

namespace Mig.Core.TaskPattern
{
    public class LoadingFromRemoteTask : TaskHandlerBase
    {
        private readonly string projectName;
        private readonly string saveZipPath;

        public LoadingFromRemoteTask(string projectName, string savePath, Action<bool> taskCallback) : base(taskCallback)
        {
            this.projectName = projectName;
            this.saveZipPath = savePath;
        }

        public override async void Execute()
        {
            EventManager.TriggerEvent(MigEventCommon.OnLoadingModelBegin, "Downloading");
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(saveZipPath))
            {
                Debug.LogWarning("You must set projectName and saveZipPath before download");
                Fail();
                return;
            }

            var result = await RemoteStorage.DownloadPackageAsync(projectName, saveZipPath);
            if (!result)
            {
                Debug.LogError($"Failed to download project {projectName} to {saveZipPath}");
                Fail();
                return;
            }

            Continue();
        }
    }
}
