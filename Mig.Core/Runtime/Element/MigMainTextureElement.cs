using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigMainTextureElement : MigElement
    {
        [JsonIgnore]
        public Texture CurrentTexture;
        public override void Apply()
        {
            var meshRender = this.gameObject.GetComponent<Renderer>();
            if (meshRender != null)
            {
                meshRender.material.mainTexture = CurrentTexture;
            }
        }

        public override void Record()
        {
            CurrentTexture = renderer.material.mainTexture;
        }
    }

}
