using System;

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
            Continue();
        }

        /// <summary>
        /// Advance to the next task, or notify the outer callback when this is the last step.
        /// </summary>
        protected void Continue()
        {
            if (NextTask != null)
            {
                NextTask.Execute();
                return;
            }

            m_taskCallback?.Invoke(true);
        }

        protected void Fail()
        {
            m_taskCallback?.Invoke(false);
        }
    }
}
