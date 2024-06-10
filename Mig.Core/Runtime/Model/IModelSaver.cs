using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public interface IModelSaver : IModelOperator
    {
        void Save(string pathORAddress, GameObject modelParent, Action onSaveComplete);

        void Save(string pathORAddress, ISerializer serializer, Action onSaveComplete);
    }
}

