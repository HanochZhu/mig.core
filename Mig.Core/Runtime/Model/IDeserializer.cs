using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public interface IDeserializer
    {
        void DeserializerAsync(byte[] srcByteArray, Action<bool> callback);
    }
}
