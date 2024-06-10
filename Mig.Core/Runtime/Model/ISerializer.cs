using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Mig
{
    public interface ISerializer 
    {
        Task<byte[]> Serialize();

        string GetName();
    }

}
