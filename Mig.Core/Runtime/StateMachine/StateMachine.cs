using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class StateMachine
    {
        private IMigStateController m_currentController;

        public void ChangeState(IMigStateController controller)
        {
            if (m_currentController != null)
            {
                m_currentController.Sleep();
            }
            m_currentController = controller;
            m_currentController.Awake();
        }

        public void Update()
        {
            if (m_currentController == null)
            {
                return;
            }
            m_currentController.Update();
        }

        public void Stop()
        {
            m_currentController.Sleep();
        }
    }
}

