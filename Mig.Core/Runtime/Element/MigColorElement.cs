using Mig;
using UnityEngine;

namespace Mig.Core
{
    public class MigColorElement : MigElement
    {
        public Color CurrentMaterialColor;
        public override void Apply()
        {
            if (renderer)
            {
                renderer.material.color = CurrentMaterialColor;
            }
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
            if (renderer)
            {
                CurrentMaterialColor = renderer.material.color;
            }
        }

    }

}
