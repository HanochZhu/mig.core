using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityGLTF;

namespace Mig.Core
{
    public class MigGltfMaterialWrapper : MigMaterialWrapperBase
    {
        private PBRGraphMap PBRGraphMap;

        public MigGltfMaterialWrapper(Material mat) : base(mat) 
        {
            PBRGraphMap = new PBRGraphMap(mat);
        }

        public override Texture mainTexutre { 
            get => PBRGraphMap.BaseColorTexture; 
            set => PBRGraphMap.BaseColorTexture = value; 
        }
        public override float Metallic
        {
            get => (float)PBRGraphMap.MetallicFactor;
            set => PBRGraphMap.MetallicFactor = value;
        }

        public override Texture NormalMap 
        { 
            get => PBRGraphMap.NormalTexture; 
            set => PBRGraphMap.NormalTexture = value; 
        }
        public override float Smoothness 
        { 
            get => (float)PBRGraphMap.RoughnessFactor; 
            set => PBRGraphMap.RoughnessFactor = value; 
        }
        public override Vector2 mainTextureScale 
        { 
            get => PBRGraphMap.SpecularTextureScale; 
            set => PBRGraphMap.SpecularTextureScale = value; 
        }
        public override float transparency 
        {
            get 
            {
                return PBRGraphMap.BaseColorFactor.a;
            }
            set
            {
                var baseColor = PBRGraphMap.BaseColorFactor;
                baseColor.a = value;
                PBRGraphMap.BaseColorFactor = baseColor;
            }
        }

        public override Color mainColor 
        {
            get =>  PBRGraphMap.BaseColorFactor;
            set => PBRGraphMap.BaseColorFactor = value;
        }
    }

}
