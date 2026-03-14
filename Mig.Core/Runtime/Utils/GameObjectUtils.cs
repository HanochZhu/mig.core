using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    public static T GetOrAddComponent<T>(this GameObject self) where T : Component
    {
        if (self == null)
        {
            Debug.LogError("[Mig] self is null, can not GetOrAddComponent at");
            return null;
        }

        if (self.TryGetComponent<T>(out T component))
        {
            return component;
        }

        return self.AddComponent<T>();
    }

    public static List<T> GetComponentsInChildrenOnly<T>(this GameObject parent, bool includeInactive = false, bool recursive = true)
    {
        List<T> list = new List<T>();
        if (parent != null)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.TryGetComponent<T>(out _) && (includeInactive || child.gameObject.activeInHierarchy))
                {
                    list.AddRange(child.GetComponents<T>());
                }
                if (recursive && child.transform.childCount > 0)
                {
                    list.AddRange(child.gameObject.GetComponentsInChildrenOnly<T>(includeInactive, recursive));
                }
            }
        }
        return list;
    }
}
