using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public abstract class MigMaterialWrapperBase
    {
        protected Material Material;

        public MigMaterialWrapperBase(Material material)
        {
            Material = material;
        }
        public abstract Texture mainTexutre { get; set; }
        public abstract Color mainColor { get; set; }

        public abstract float Metallic{ get; set; }

        public abstract Texture NormalMap{ get; set; }

        public abstract float Smoothness { get; set; }
        public abstract Vector2 mainTextureScale { get; set; }

        public abstract float transparency { get; set; }
    }
}
