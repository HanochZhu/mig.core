using Mig;
using Mig.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Snapshot
{
    public record SnapShotState
    {
        /// <summary>
        /// What is Mig Element?
        /// This definition is copy from Jig. All operatable gamoebjct in scene is binded to jig element.
        /// So that jig can control and record operation and position of each gameobject
        /// MigElementName also means gameobject name
        /// </summary>
        internal List<MigElement> AllOperatorElement = new();

        public string ID;

        public List<SnapShotState> ChildState;

    }
}