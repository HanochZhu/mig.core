using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mig.Core.Editor
{
    [CustomEditor(typeof(MigMaterialLibrary))]
    public class MigMaterialLibraryEditor : UnityEditor.Editor
    {
        private MigMaterialLibrary migMaterialLibrary;
        public void OnEnable()
        {
            migMaterialLibrary = (MigMaterialLibrary)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (MigMaterialItem item in migMaterialLibrary.migMaterialItems)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(item.Guid.ToString());
                GUILayout.BeginVertical();
                item.Name = GUILayout.TextField(item.Name);
                item.Material = EditorGUILayout.ObjectField(item.Material, typeof(Material),false) as Material;
                item.Icon = EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), false) as Sprite;
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }

            if(GUILayout.Button("+"))
            {
                migMaterialLibrary.CreateNewItem();
            }
        }
    }
}
