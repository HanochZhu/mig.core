using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Mig
{
    public interface ISerializer 
    {
        Task<bool> Serialize();

        string GetName();
    }

}
