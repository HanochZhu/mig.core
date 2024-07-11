using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigElementWrapper : MonoBehaviour
    {
        public static GameObject WrapperRoot;

        private List<MigElement> elements = new();

        public MigElement this[int index]
        {
            get 
            {
                if (index >= elements.Count)
                {
                    Debug.Log($"Failed to get element from mig element wrapper at {index}");
                    return null;
                }
                return elements[index];
            }
            set 
            {
                elements[index] = value; 
            }
        }
        public List<MigElement> Elements
        {
            get => elements;
        }

        public void PushBackElement(MigElement element)
        {
            elements.Add(element);
        }

        public void RemoveElement(MigElement element)
        {
            elements.Remove(element);
        }
    }
}