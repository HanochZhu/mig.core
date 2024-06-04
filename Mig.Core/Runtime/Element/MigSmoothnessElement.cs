using Mig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigSmoothnessElement : MigElement
    {
        public float CurrentSmoothness;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                meshRender.material.SetFloat("_Glossiness", CurrentSmoothness);
            }
        }

        public override void Record()
        {
            if (this.renderer)
            {
                CurrentSmoothness = renderer.material.GetFloat("_Glossiness");
            }
        }
    }

}
