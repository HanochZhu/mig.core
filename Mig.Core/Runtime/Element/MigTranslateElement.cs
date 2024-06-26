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
        public Vector3 StepLocalPosition;
        public Quaternion StepLocalRotation;
        public Vector3 StepLocalScale;


        public override void Apply()
        {
            this.transform.localPosition = StepLocalPosition;
            this.transform.localRotation = StepLocalRotation;
            this.transform.localScale = StepLocalScale;
        }

        public override void Record()
        {
            StepLocalPosition = this.transform.localPosition;
            StepLocalRotation = this.transform.localRotation;
            StepLocalScale = this.transform.lossyScale;
        }
    }

}
