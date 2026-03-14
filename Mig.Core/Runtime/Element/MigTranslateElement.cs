using Mig;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Core
{
    public class MigTranslateElement : MigElement
    {
        public Vector3 StepLocalPosition;
        public Quaternion StepLocalRotation;
        public Vector3 StepLocalScale;


        public override void Apply()
        {
            this.transform.localPosition = StepLocalPosition;
            this.transform.localRotation = StepLocalRotation;
            this.transform.localScale = StepLocalScale;
        }

        public override MigElement Clone()
        {
            var clone = new MigTranslateElement();
            clone.StepLocalScale = this.StepLocalScale;
            clone.StepLocalPosition = this.StepLocalPosition;   
            clone.StepLocalRotation = this.StepLocalRotation;
            clone.GameObjectPath = this.GameObjectPath; 
            return clone;
        }

        public override void Record()
        {
            StepLocalPosition = this.transform.localPosition;
            StepLocalRotation = this.transform.localRotation;
            StepLocalScale = this.transform.lossyScale;
        }
    }

}
