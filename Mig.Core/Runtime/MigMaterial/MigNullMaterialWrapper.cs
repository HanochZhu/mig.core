using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mig.Core
{
    public class MigNullMaterialWrapper : MigMaterialWrapperBase
    {
        public MigNullMaterialWrapper(Material material) : base(material)
        {
        }

        public override Texture mainTexutre { get ; set ; }
        public override Color mainColor { get; set ; }
        public override float Metallic { get ; set ; }
        public override Texture NormalMap { get; set; }
        public override float Smoothness { get; set ; }
        public override Vector2 mainTextureScale { get ; set ; }
        public override float transparency { get ; set ; }
    }

}
