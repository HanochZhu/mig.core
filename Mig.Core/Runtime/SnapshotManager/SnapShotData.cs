using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Snapshot
{
    public class SnapShotData
    {
        private Dictionary<string, SnapShotState> GameObjectSnapShotState = new();

        public int StepCount;

        public Guid StepGuid;

        public string Name;
        public string Comment;

        // TODO
        [JsonIgnore]
        public Texture2D Image;

        // TODO It seems that SnapShotStateRoot is unnecessary;
        //public SnapShotState SnapShotStateRoot;

        /// <summary>
        /// Todo might not used
        /// </summary>
        //public string GameObjectName;
    }
}