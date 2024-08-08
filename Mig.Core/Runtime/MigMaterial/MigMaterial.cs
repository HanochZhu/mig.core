using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityGLTF;

namespace Mig.Core
{
    public class MigMaterial
    {
        private MigMaterialWrapperBase migMaterialWrapperBase;
        private const string gltfShaderName = "UnityGLTF/PBRGraph";
        public MigMaterial(Material mat)
        {
            UpdateMaterial(mat);
        }

        public void UpdateMaterial(Material mat)
        {
            if (mat.shader.name == gltfShaderName)// 
            {
                migMaterialWrapperBase = new MigGltfMaterialWrapper(mat);
                return;
            }
            migMaterialWrapperBase = new MigLitMaterialWrapper(mat);
        }

        public Texture mainTexture
        {
            set => migMaterialWrapperBase.mainTexutre = value;
            get => migMaterialWrapperBase.mainTexutre;
        }

        public Color mainColor
        {
            set => migMaterialWrapperBase.mainColor = value;
            get => migMaterialWrapperBase.mainColor;
        }

        public float Metallic
        {
            get => migMaterialWrapperBase.Metallic;
            set => migMaterialWrapperBase.Metallic = value;
        }
        public Texture NormalMap 
        { 
            get => migMaterialWrapperBase.NormalMap; 
            set => migMaterialWrapperBase.NormalMap = value; 
        }
        public float Smoothness 
        {
            get => migMaterialWrapperBase.Smoothness;
            set => migMaterialWrapperBase.Smoothness = value;
        }
        public Vector2 mainTextureScale 
        {
            get => migMaterialWrapperBase.mainTextureScale;
            set => migMaterialWrapperBase.mainTextureScale = value;
        }

        public float transparency
        {
            get => migMaterialWrapperBase.transparency;
            set => migMaterialWrapperBase.transparency = value;
        }
    }
}
