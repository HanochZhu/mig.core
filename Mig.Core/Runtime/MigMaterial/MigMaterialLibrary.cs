using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mig.Core
{

    [Serializable]
    public class MigMaterialItem
    {
        public Guid Guid;
        public string Name;
        public Material Material;

        public Sprite Icon;

        public MigMaterialItem()
        {
            Guid = Guid.NewGuid();
        }
    }

    [Serializable]
    public class MigTextureItem
    {
        public Guid Guid;
        public string Name;
        public Texture Texture;
        public MigTextureItem()
        {
            Guid = Guid.NewGuid();
        }
    }

    [CreateAssetMenu]
    public class MigMaterialLibrary : ScriptableObject
    {
        public List<MigMaterialItem> migMaterialItems = new();
        public List<Texture> textures = new();

        private static MigMaterialLibrary instance;

        public static MigMaterialLibrary Instance
        {
            get
            {
                if (!instance)
                {
                    instance = Resources.Load("MigMaterialLibrary") as MigMaterialLibrary;
                    if (instance == null)
                    {
                        Debug.Assert(false, "Fail to load MigMaterialLibrary at /MigMaterialLibrary");
                    }
                }
                return instance;
            }
        }
        
        public static Material LoadMaterialByGuiD(Guid matGUID)
        {
            var selectMaterialItem = instance.migMaterialItems.Where(m => m.Guid.Equals(matGUID)).FirstOrDefault();
            return selectMaterialItem.Material ;
        }

#if UNITY_EDITOR
        public void CreateNewItem()
        {
            var newItem = new MigMaterialItem();
            newItem.Guid = Guid.NewGuid();
            migMaterialItems.Add(newItem);
        }
        public void DeleteLastItem()
        {
            var newItem = new MigMaterialItem();
            newItem.Guid = Guid.NewGuid();
            migMaterialItems.Add(newItem);
        }
#endif
    }
}
