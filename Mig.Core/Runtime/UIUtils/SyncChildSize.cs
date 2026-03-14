using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core.UIUtils
{
    public class SyncChildSize : MonoBehaviour
    {

        public RectTransform refChild;

        public Vector2 offset = Vector2.zero;

        private RectTransform RectTransform;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            RectTransform.sizeDelta = refChild.sizeDelta + offset;
        }
    }

}

