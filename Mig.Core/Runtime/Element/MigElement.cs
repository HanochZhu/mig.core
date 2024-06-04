using Newtonsoft.Json;
using UnityEngine;


namespace Mig.Core
{
    public abstract class MigElement
    {
        [JsonIgnore] public MigElementWrapper Wrapper;

        [JsonProperty("stepIndex")] private int stepIndex;
        [JsonProperty("gameObjectPath")] private string gameObjectPath;
        public int OperateCount;


        [JsonIgnore]
        public int StepIndex
        {
            get => stepIndex;
            internal set => stepIndex = value;
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

        public virtual void Init(int currentStep, string gameobjectPath)
        {
#if MIG_RUNTIME
        // TODO
#else
            /// if in runtime mode, the step count should set by deserializer.
            stepIndex = currentStep;// SnapshotManager.Instance.CurrentSnapshotIndex;
            GameObjectPath = gameobjectPath;// GameObjectExtensions.GetGameObjectTreePath(Wrapper.gameObject, ModelManager.Instance.CurrentGameObjectRoot.transform);
#endif
        }

        /// <summary>
        /// if we select current step snapshot, the fast way to apply all state is apply all element
        /// </summary>
        public abstract void Apply();

        public abstract void Record();
    }

}
