using Mig;
using UnityEngine;

namespace Mig.Core
{
    public class MigTranformElement : MigElement
    {
        public Vector3 CurrentPostion;
        public Quaternion CurrentRotate;
        public Vector3 CurrentScale;
        public override void Apply()
        {
            if (transform)
            {
                transform.position = CurrentPostion;
                transform.rotation = CurrentRotate;
                transform.localScale = CurrentScale;
            }
        }

        public override void Record()
        {
            if (transform)
            {
                CurrentPostion = transform.position;
                CurrentRotate = transform.rotation;
                CurrentScale = transform.localScale;
            }
        }
    }

}
