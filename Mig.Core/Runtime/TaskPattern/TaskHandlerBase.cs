using System;
using System.Threading.Tasks;

namespace Mig.Core.TaskPattern
{
    public class TaskHandlerBase 
    {
        public TaskHandlerBase NextTask { get; set; }

        protected Action<bool> m_taskCallback;

        public TaskHandlerBase(Action<bool> taskCallback)
        {
            m_taskCallback = taskCallback;
        }

        public virtual void Execute()
        {
            m_taskCallback?.Invoke(true);

            if (NextTask != null)
            {
                NextTask.Execute();
            }
        }
    }
}

