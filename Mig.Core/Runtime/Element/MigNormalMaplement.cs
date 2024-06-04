using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigNormalMaplement : MigElement
    {
        // TODO
        [JsonIgnore]
        public Texture CurrentNormalMap;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                meshRender.material.SetTexture("_BumpMap", CurrentNormalMap);
            }
        }

        public override void Record()
        {
            if (renderer)
            {
                CurrentNormalMap = renderer.material.GetTexture("_BumpMap");
            }
        }
    }
}

