using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Mig.Snapshot
{
    public class SnapShotData
    {
        public int StepCount;

        public Guid StepGuid;

        public string Name;
        public string Comment;

        public bool HasCameraPose;
        public Vector3 CameraPosition;
        public Quaternion CameraRotation;

        [JsonIgnore]
        public Texture2D Image;
    }
}
