using UnityEngine;

namespace Mig.Core
{
    public class MigMetallicElement : MigElement
    {
        public float CurrentMetallic;
        public override void Apply()
        {
            if (renderer == null)
            {
                Debug.LogWarning($"Fail to get mesh render at {this.gameObject.name}");
                return;
            }
            material.Metallic = CurrentMetallic;
        }

        public override MigElement Clone()
        {
            var clone = new MigMetallicElement();
            clone.CurrentMetallic = CurrentMetallic;
            clone.GameObjectPath = GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            if (!renderer)
            {
                Debug.LogWarning($"Fail to record at {this.Wrapper.gameObject.name}");
                return;
            }
            CurrentMetallic = material.Metallic;
        }
    }
}

