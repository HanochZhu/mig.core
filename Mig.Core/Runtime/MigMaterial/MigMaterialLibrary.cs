using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mig.Core
{

    public class MigMaterialItem
    {
        public Guid Guid;
        public string Name;
        public Material Material;

        public Sprite Icon;
    }

    [CreateAssetMenu]
    public class MigMaterialLibrary : ScriptableObject
    {
        [SerializeField]
        public List<MigMaterialItem> migMaterialItems = new();

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
            return instance.migMaterialItems.Where(m => m.Equals(matGUID)).FirstOrDefault().Material ;
        }

#if UNITY_EDITOR
        public void CreateNewItem()
        {
            var newItem = new MigMaterialItem();
            newItem.Guid = Guid.NewGuid();
            migMaterialItems.Add(newItem);
        }
#endif
    }
}
