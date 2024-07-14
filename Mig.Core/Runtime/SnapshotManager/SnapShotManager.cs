using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mig.Snapshot
{
    public class SnapshotManager : EasySington<SnapshotManager>
    {
        public Action OnLoadedAllSnapShot;

        public Action OnSnapShotUpdated;

        /// <summary>
        ///  index start from 0
        /// </summary>
        public int CurrentSnapshotIndex { get; private set; } = 0;

        public Guid CurrentSnapshotGuid
        {
            get
            {
                if (CurrentSnapshotCount == 0)
                {
                    return new Guid();
                }
                return m_allSnapShotSteps[CurrentSnapshotIndex].StepGuid;
            }
        }

        public Guid FirstSnapshotGuid
        {
            get
            {
                if (CurrentSnapshotCount == 0)
                {
                    Debug.LogError("[Mig] failed to find first snapshot, Create one");
                    UpdateCurrentSnapShot();
                }
                return m_allSnapShotSteps[0].StepGuid;
            }
        }

        public int CurrentSnapshotCount => m_allSnapShotSteps.Count;

        private List<SnapShotData> m_allSnapShotSteps = new();

        public void UpdateCurrentSnapShot()
        {
            if (CurrentSnapshotCount == 0)
            {
                Debug.Log("[Mig] Init snapshot first");
                // create a new snapshot at first snapshot
                AddSnapShotStepAtEnd();
            }
            UpdateSnapshotImage(CurrentSnapshotIndex);
            OnSnapShotUpdated?.Invoke();
        }

        private void UpdateSnapshotImage(int index)
        {
            if (index >= CurrentSnapshotCount)
            {
                Debug.LogError($"Failed to update snapshot for {index} out of range of snapshot count {CurrentSnapshotCount}");
                return;
            }
            var currentSnapShot = m_allSnapShotSteps[index];
            var image = SnapShotUtils.TakeScreenshotForStepThumbail(Camera.main, 520, 480);
            currentSnapShot.Image = image;
        }

        /// <summary>
        /// TODO, might execute in coroutine
        /// </summary>
        public void AddSnapShotStepAtEnd()
        {
            InsertSnapShotStepAfter(CurrentSnapshotIndex);
        }

        /// <summary>
        /// delete current, then move forward
        /// </summary>
        /// <param name="index"></param>
        public void DeleteSnapshotStepAt(int index)
        {
            Debug.Log($"[Mig] Delete snapshot at {index}");

            if (index == 0 && CurrentSnapshotCount == 1)
            {
                Debug.LogError("[Mig] You can not delete only one snapshot");
                return;
            }
            else if (index == 0 && CurrentSnapshotCount != 1)
            {
                SnapShotUtils.DeleteAllSnapshotOf(m_allSnapShotSteps[0].StepGuid);
                m_allSnapShotSteps.RemoveAt(index);

                CurrentSnapshotIndex = 0;
                UpdateCurrentSnapShot();
                return;
            }

            SnapShotUtils.DeleteAllSnapshotOf(m_allSnapShotSteps[index].StepGuid);
            m_allSnapShotSteps.RemoveAt(index);

            CurrentSnapshotIndex = index - 1;
            UpdateCurrentSnapShot();
            return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afterIndex"></param>
        public void InsertSnapShotStepAfter(int afterIndex)
        {
            SnapShotData snapShotData = new SnapShotData();
            // TODO, we should use an additional camera to render snapshot

            snapShotData.StepCount = CurrentSnapshotCount;
            // only set guid at here
            snapShotData.StepGuid = Guid.NewGuid();
            snapShotData.Comment = string.Empty;

            if(CurrentSnapshotCount == 0)
            {
                InsertSnapShotStepAt(0, snapShotData);
            }
            else
            {
                SnapShotUtils.CloneAllSnapshot(m_allSnapShotSteps[afterIndex].StepGuid, snapShotData.StepGuid);
                InsertSnapShotStepAt(afterIndex + 1, snapShotData);
            }
        }

        public void InsertSnapShotStepAt(int index, SnapShotData snapshot)
        {
            m_allSnapShotSteps.Insert(index, snapshot);
            CurrentSnapshotIndex = index;
            UpdateSnapshotImage(index);
            OnSnapShotUpdated?.Invoke();
        }

        public void ApplyToTargetSnapshot(int index)
        {
            if(index > CurrentSnapshotIndex)
            {
                index %= CurrentSnapshotCount;
            }
            else if(index < CurrentSnapshotIndex && index < 0)
            {
                index = CurrentSnapshotCount - 1;
            }

            var applySnapshot = m_allSnapShotSteps[index];

            SnapShotUtils.ApplyToSnapshot(applySnapshot.StepGuid);

            CurrentSnapshotIndex = index;
        }

        public SnapShotData GetSnapShotStep(int index) => m_allSnapShotSteps[index];

        public List<SnapShotData> GetCurrentAllSnapShot() => m_allSnapShotSteps;

        public Texture2D GetProjectSnapshotImage()
        {
            if (m_allSnapShotSteps.Count == 0)
            {
                return null;
            }
            return m_allSnapShotSteps[0].Image;
        }

        public void SetAllSnapShot(List<SnapShotData> datas)
        {
            m_allSnapShotSteps = datas;

            OnSnapShotUpdated?.Invoke();
        }
    }
}