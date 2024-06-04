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

        public override void Record()
        {
            if (renderer)
            {
                CurrentMaterialColor = renderer.material.color;
            }
        }
    }

}
