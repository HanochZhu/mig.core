using Newtonsoft.Json;
using System;
using UnityEngine;


namespace Mig.Core
{
    public abstract class MigElement
    {
        [JsonIgnore] private MigElementWrapper wrapper;
        [JsonIgnore] public MigElementWrapper Wrapper
        {
            set
            {
                wrapper = value;
                if (wrapper == null || material != null)
                {
                    return;
                }

                var hostRenderer = wrapper.GetComponent<Renderer>();
                if (hostRenderer == null)
                {
                    return;
                }

                material = new MigMaterial(hostRenderer.material, hostRenderer.gameObject);
            }
            get
            {
                return wrapper;
            }
        }

        [JsonProperty("stepGuid")] private Guid stepGuid;
        [JsonProperty("gameObjectPath")] private string gameObjectPath;
        public int OperateCount;

        [JsonIgnore]
        public Guid StepGUID
        {
            get => stepGuid;
            internal set => stepGuid = value;
        }

        [JsonIgnore]
        public string GameObjectPath
        {
            get => gameObjectPath;
            internal set => gameObjectPath = value;
        }

        [JsonIgnore]
        public GameObject gameObject => Wrapper.gameObject;
        [JsonIgnore]
        public Transform transform => Wrapper.transform;

        [JsonIgnore]
        public Renderer renderer => gameObject ? gameObject.GetComponent<Renderer>(): null;

        [JsonIgnore]
        public MigMaterial material;

        public virtual void Init(string gameobjectPath, Guid currentGUID)
        {
#if MIG_RUNTIME
        // TODO
#else
            /// if in runtime mode, the step count should set by deserializer.
            GameObjectPath = gameobjectPath;// GameObjectExtensions.GetGameObjectTreePath(Wrapper.gameObject, ModelManager.Instance.CurrentGameObjectRoot.transform);
            stepGuid = currentGUID;
#endif
        }

        /// <summary>
        /// if we select current step snapshot, the fast way to apply all state is apply all element
        /// </summary>
        public void ReleaseIfUnused()
        {
            OperateCount--;
            if (OperateCount > 0 || Wrapper == null)
            {
                return;
            }

            Wrapper.RemoveElement(this);
            Wrapper = null;
        }

        public abstract void Apply();

        public abstract void Record();

        public abstract MigElement Clone();
    }

}
