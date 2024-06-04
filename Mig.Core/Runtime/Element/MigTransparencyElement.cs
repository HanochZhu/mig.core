using Mig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigTransparencyElement : MigElement
    {
        public float CurrentTransparency;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                var color = meshRender.material.color;
                color.a = CurrentTransparency;
                meshRender.material.color = color;
            }
        }

        public override void Record()
        {
            if (renderer)
            {
                CurrentTransparency = renderer.material.color.a;
            }
        }
    }

}
