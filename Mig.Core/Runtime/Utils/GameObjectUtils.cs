using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
}
