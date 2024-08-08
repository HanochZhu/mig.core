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
            if (renderer == null)
            {
                Debug.LogError($"[MigTilingElement] Can not find render at {this.Wrapper.name}");
                return;
            }
            
            this.migMaterial.transparency = CurrentTransparency;
        }

        public override MigElement Clone()
        {
            var clone = new MigTransparencyElement();
            clone.CurrentTransparency = this.CurrentTransparency;   
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentTransparency = this.migMaterial.transparency;
        }
    }

}
