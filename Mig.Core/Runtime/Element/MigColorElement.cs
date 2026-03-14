using Mig;
using UnityEngine;

namespace Mig.Core
{
    public class MigColorElement : MigElement
    {
        public Color CurrentMaterialColor;
        public override void Apply()
        {
            if (!renderer)
            {
                Debug.LogError($"[MigColorElement] can not find render at {Wrapper.gameObject.name}");
                return;
            }
            this.material.mainColor = CurrentMaterialColor;
        }

        public override MigElement Clone()
        {
            var clone = new MigColorElement();
            clone.CurrentMaterialColor = CurrentMaterialColor;
            clone.GameObjectPath = GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            CurrentMaterialColor = this.material.mainColor;
        }

    }

}
