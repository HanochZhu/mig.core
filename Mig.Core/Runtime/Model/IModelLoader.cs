using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mig
{

    public enum ModelOperateState
    {
        LOADING,
        LOAD_COMPLETE,
        SAVEING,
        SAVE_COMPLETE,
        ERROR,
    }

    public interface IModelLoader : IModelOperator
    {
        // We need load model (fbx) from file or web.
        // So Loader means an toolkit to provide a set of tool.
        void SetParent(Transform root);

        void LoadAsync(string path, Action<GameObject> go);

        string GetLoaderName();
    }
}
