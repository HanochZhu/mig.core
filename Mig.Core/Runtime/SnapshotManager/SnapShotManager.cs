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
            // update last snapshot first
            UpdateSnapshotImage(CurrentSnapshotIndex);

            SnapShotData snapShotData = new SnapShotData();
            // TODO, we should use an additional camera to render snapshot

            snapShotData.StepCount = CurrentSnapshotCount;
            // only set guid at here
            snapShotData.StepGuid = Guid.NewGuid();
            snapShotData.Comment = string.Empty;

            m_allSnapShotSteps.Add(snapShotData);
            CurrentSnapshotIndex = CurrentSnapshotCount - 1;

            UpdateSnapshotImage(CurrentSnapshotIndex);

            OnSnapShotUpdated?.Invoke();
        }

        /// <summary>
        /// delete current, then move forward
        /// </summary>
        /// <param name="index"></param>
        public void DeleteSnapshotStepAt(int index, GameObject root)
        {
            Debug.Log($"[Mig] Delete snapshot at {index}");

            if (index == 0 && CurrentSnapshotCount == 1)
            {
                Debug.LogError("[Mig] You can not delete only one snapshot");
                return;
            }
            else if (index == 0 && CurrentSnapshotCount != 1)
            {
                SnapShotUtils.DeleteAllSnapshotOf(root, m_allSnapShotSteps[0].StepGuid);
                m_allSnapShotSteps.RemoveAt(index);

                CurrentSnapshotIndex = 0;
                UpdateCurrentSnapShot();
                return;
            }

            SnapShotUtils.DeleteAllSnapshotOf(root, m_allSnapShotSteps[index].StepGuid);
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

            InsertSnapShotStepAt(afterIndex + 1, snapShotData);
        }

        public void InsertSnapShotStepAt(int index, SnapShotData snapshot)
        {
            Debug.Assert(index < m_allSnapShotSteps.Count, $"[Mig::SnapshotManager] InsertSnapShotStepAt index {index} is out of range");

            m_allSnapShotSteps.Insert(index, snapshot);
            CurrentSnapshotIndex = index;
        }

        public void PushBackSnapShotStep(SnapShotData snapshot)
        {
            m_allSnapShotSteps.Add(snapshot);
            CurrentSnapshotIndex = CurrentSnapshotCount - 1;
        }

        public void ApplyToTargetSnapshot(int index, GameObject root)
        {
            Debug.Assert(root && index != CurrentSnapshotCount, $"[Mig::ApplyToNextSnapShot] gameobejct root is null, or {index} equals to CurrentSnapshotCount {CurrentSnapshotCount}");

            var applySnapshot = m_allSnapShotSteps[index];

            SnapShotUtils.ApplyToSnapshot(root, applySnapshot.StepGuid);

            CurrentSnapshotIndex = index;
        }

        public void ApplyToNextSnapShot(GameObject root)
        {
            Debug.Assert(root, "[Mig::ApplyToNextSnapShot] gameobejct root is null, generate gameobject root first");

            CurrentSnapshotIndex = (CurrentSnapshotIndex + 1) % CurrentSnapshotCount;
            SnapShotUtils.ApplyToSnapshot(root, m_allSnapShotSteps[CurrentSnapshotIndex].StepGuid);
        }

        public void ApplyToPreviousSnapshot(GameObject root)
        {
            Debug.Assert(root, "[Mig::ApplyToNextSnapShot] gameobejct root is null, generate gameobject root first");

            CurrentSnapshotIndex--;
            if (CurrentSnapshotIndex < 0)
            {
                CurrentSnapshotIndex = CurrentSnapshotCount - 1;
            }
            SnapShotUtils.ApplyToSnapshot(root, m_allSnapShotSteps[CurrentSnapshotIndex].StepGuid);
        }

        public bool DeleteSnapShotStep(int index)
        {
            // todo
            return true;
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