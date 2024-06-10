using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public interface IModelOperator
    {
        float GetPercentage();

        string ErrorMsg();

        ModelOperateState GetState();

        // We need dispose loader respective
        void OnDispose();
    }

}
