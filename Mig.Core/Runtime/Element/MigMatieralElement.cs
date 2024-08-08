using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigMaterialElement : MigElement
    {
        [JsonIgnore]
        public Material CurrentMaterial;
        public override void Apply()
        {
            if (renderer == null)
            {
                Debug.LogError($"can not get mesh renderer at {this.Wrapper.name}");
                return;
            }
            renderer.material = CurrentMaterial;
            this.material.UpdateMaterial(CurrentMaterial);
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
            CurrentMaterial = renderer.material;
        }
    }
}

