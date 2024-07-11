using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigOffsetElement : MigElement
    {
        [JsonIgnore]
        public Vector2 CurrentOffset;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                meshRender.material.mainTextureOffset = CurrentOffset;
            }
        }

        public override MigElement Clone()
        {
            var clone = new MigOffsetElement();
            clone.CurrentOffset = this.CurrentOffset;
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            if (renderer)
            {
                CurrentOffset = renderer.material.mainTextureOffset;
            }
        }
    }

}
