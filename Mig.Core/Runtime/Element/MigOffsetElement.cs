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
            material.mainTextureOffset = CurrentOffset;
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
            CurrentOffset = material.mainTextureOffset;
        }
    }

}
