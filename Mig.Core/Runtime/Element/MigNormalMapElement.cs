using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigNormalMapElement : MigElement
    {
        // TODO
        [JsonIgnore]
        public Texture CurrentNormalMap;
        public override void Apply()
        {
            if (renderer == null)
            {
                Debug.LogError($"[MigNormalMapElement] Can not find render at {this.Wrapper.name}");
                return;
            }
            this.migMaterial.NormalMap = CurrentNormalMap;
        }

        public override MigElement Clone()
        {
            var clone = new MigNormalMapElement();
            clone.CurrentNormalMap = this.CurrentNormalMap; 
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentNormalMap = this.migMaterial.NormalMap;
        }
    }
}

