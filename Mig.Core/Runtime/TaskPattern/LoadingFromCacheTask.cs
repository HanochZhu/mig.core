using System;

namespace Mig.Core.TaskPattern
{
    public class LoadingFromCacheTask : TaskHandlerBase
    {
        public LoadingFromCacheTask(Action<bool> taskCallback) : base(taskCallback)
        {

        }
    }

}
