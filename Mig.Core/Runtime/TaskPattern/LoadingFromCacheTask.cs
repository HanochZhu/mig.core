using System;
using System.IO;
using UnityEngine;

namespace Mig.Core.TaskPattern
{
    public class LoadingFromCacheTask : TaskHandlerBase
    {
        private readonly string cachedZipPath;

        public LoadingFromCacheTask(string cachedZipPath, Action<bool> taskCallback) : base(taskCallback)
        {
            this.cachedZipPath = cachedZipPath;
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(cachedZipPath) || !File.Exists(cachedZipPath))
            {
                Debug.Log($"[Mig] No cached project at {cachedZipPath}");
                Fail();
                return;
            }

            Debug.Log($"[Mig] Using cached project at {cachedZipPath}");
            Continue();
        }
    }
}
