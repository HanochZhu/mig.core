using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigLitMaterialWrapper : MigMaterialWrapperBase
    {
        public MigLitMaterialWrapper(Material material) : base(material) { }
        public override Texture mainTexutre 
        { 
            get => this.Material.mainTexture; 
            set => this.Material.mainTexture = value; 
        }
        public override float Metallic
        {
            get => this.Material.GetFloat("_Metallic");
            set => this.Material.SetFloat("_Metallic", value);
        }
        public override Texture NormalMap 
        { 
            get => this.Material.GetTexture("_BumpMap");
            set => this.Material.SetTexture("_BumpMap", value); 
        }
        public override float Smoothness 
        { 
            get => this.Material.GetFloat("_Smoothness"); 
            set => this.Material.SetFloat("_Smoothness", value); 
        }
        public override Vector2 mainTextureScale 
        { 
            get => Material.mainTextureScale; 
            set => Material.mainTextureScale = value; 
        }
        public override float transparency 
        { 
            get
            {
                return this.Material.color.a;
            }
            set
            {
                var color = this.Material.color;
                color.a = value;
                this.Material.color = color;
            }
        }

        public override Color mainColor 
        { 
            get => this.Material.color;
            set => this.Material.color = value; 
        }
    }
}

