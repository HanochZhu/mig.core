using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public interface IModelSaver : IModelOperator
    {
        void Save(string pathORAddress, GameObject modelParent, Action<bool> onSaveComplete);

        void Save(string pathORAddress, ISerializer serializer, Action<bool> onSaveComplete);
    }
}

