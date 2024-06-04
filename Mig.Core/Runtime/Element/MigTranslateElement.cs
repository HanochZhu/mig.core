using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigTranslateElement : MigElement
    {
        [JsonIgnore]
        public Vector3 StepWorldPosition;


        public override void Apply()
        {
            this.transform.position = StepWorldPosition;
        }

        public override void Record()
        {
            StepWorldPosition = this.transform.position;
        }
    }

}
