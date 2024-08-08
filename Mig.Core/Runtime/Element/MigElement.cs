using Newtonsoft.Json;
using System;
using UnityEngine;
using Mig.Core;
using System.Runtime.InteropServices.WindowsRuntime;


namespace Mig.Core
{
    public abstract class MigElement
    {
        [JsonIgnore] public MigElementWrapper Wrapper;

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
        public MigMaterial migMaterial;

        public virtual void Init(string gameobjectPath, Guid currentGUID)
        {
#if MIG_RUNTIME
        // TODO
#else
            /// if in runtime mode, the step count should set by deserializer.
            GameObjectPath = gameobjectPath;// GameObjectExtensions.GetGameObjectTreePath(Wrapper.gameObject, ModelManager.Instance.CurrentGameObjectRoot.transform);
            stepGuid = currentGUID;
#endif

            //if (renderer == null)
            //{
            //    Debug.LogError("can no find render at this gameobject");
            //    return;
            //}
            migMaterial = new MigMaterial(renderer.material);
        }

        /// <summary>
        /// if we select current step snapshot, the fast way to apply all state is apply all element
        /// </summary>
        public abstract void Apply();

        public abstract void Record();

        public abstract MigElement Clone();
    }

}
