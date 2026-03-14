using Mig;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mig.Core
{
    public class MigMaterialElement : MigElement
    {
        // empty means origin material
        public Guid CurrentMaterialGUID = Guid.Empty;
        public override void Apply()
        {
            if (renderer == null || renderer.material == null || CurrentMaterialGUID == null)
            {
                Debug.LogError($"can not get mesh renderer at {this.Wrapper.name}");
                return;
            }

            if(CurrentMaterialGUID == Guid.Empty) 
            {
                return;
            }
            renderer.material = MigMaterialLibrary.LoadMaterialByGuiD(CurrentMaterialGUID);
            this.material.UpdateMaterial(renderer.material);
        }

        public override MigElement Clone()
        {
            var clone = new MigMaterialElement();
            clone.CurrentMaterialGUID = this.CurrentMaterialGUID;
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentMaterialGUID = Guid.Empty;
        }
    }
}

