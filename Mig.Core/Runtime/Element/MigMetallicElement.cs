using Mig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigMetallicElement : MigElement
    {
        public float CurrentMetallic;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                // Todo
                meshRender.material.SetFloat("_Metallic", CurrentMetallic);
            }
        }

        public override void Record()
        {
            if (renderer)
            {
                CurrentMetallic = renderer.material.GetFloat("_Metallic");
            }
        }
    }
}

