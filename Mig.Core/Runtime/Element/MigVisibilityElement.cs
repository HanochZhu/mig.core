using UnityEngine;

namespace Mig.Core
{
    public class MigVisibilityElement : MigElement
    {
        public bool IsVisible = true;

        public override void Apply()
        {
            if (gameObject == null)
            {
                return;
            }

            gameObject.SetActive(IsVisible);
        }

        public override MigElement Clone()
        {
            var clone = new MigVisibilityElement();
            clone.IsVisible = IsVisible;
            clone.GameObjectPath = GameObjectPath;
            return clone;
        }

        public override void Record()
        {
            IsVisible = gameObject == null || gameObject.activeSelf;
        }
    }
}
