using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigMainTextureElement : MigElement
    {

        public Texture2D CurrentTexture;
        public override void Apply()
        {
            if (migMaterial == null)
            {
                Debug.LogError($"can not find render at {Wrapper.gameObject.name}");
                return;
            }
            migMaterial.mainTexture = CurrentTexture;
        }

        public override MigElement Clone()
        {
            var clone = new MigMainTextureElement();
            clone.CurrentTexture = this.CurrentTexture;
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentTexture = (Texture2D)migMaterial.mainTexture;
        }
    }

}
