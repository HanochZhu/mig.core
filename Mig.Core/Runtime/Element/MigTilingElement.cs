using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigTilingElement : MigElement
    {
        [JsonIgnore]
        public Vector2 CurrentTiling;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogError($"[MigTilingElement] Can not find render at {this.Wrapper.name}");
                return;
            }
            this.migMaterial.mainTextureScale = CurrentTiling;
        }

        public override MigElement Clone()
        {
            var clone = new MigTilingElement();
            clone.CurrentTiling = CurrentTiling;    
            clone.GameObjectPath = GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            if (renderer)
            {
                CurrentTiling = this.migMaterial.mainTextureScale;
            }
        }
    }
}

