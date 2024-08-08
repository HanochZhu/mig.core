using UnityEngine;

namespace Mig.Core
{
    public class MigSmoothnessElement : MigElement
    {
        public float CurrentSmoothness;
        public override void Apply()
        {
            if (renderer == null)
            {
                Debug.LogError($"can not find renderer at {this.Wrapper.name}");
                return;
            }
            this.migMaterial.Smoothness = CurrentSmoothness;

        }

        public override MigElement Clone()
        {
            var clone = new MigSmoothnessElement();
            clone.CurrentSmoothness = this.CurrentSmoothness;
            clone.GameObjectPath = this.GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentSmoothness = this.migMaterial.Smoothness;
        }
    }

}
