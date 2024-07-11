using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigMaterialElement : MigElement
    {
        // TODo
        [JsonIgnore]
        public Material CurrentMaterial;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                meshRender.material = CurrentMaterial;
            }
        }

        public override MigElement Clone()
        {
            var clone = new MigMaterialElement();
            clone.CurrentMaterial = this.CurrentMaterial;
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                CurrentMaterial = meshRender.material;
            }
        }
    }
}

