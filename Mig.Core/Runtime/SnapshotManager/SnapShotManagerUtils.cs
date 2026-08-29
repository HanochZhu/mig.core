using System.Linq;
using UnityEngine;
using Mig.Core;
using System;

namespace Mig.Snapshot
{
    public static class SnapshotManagerUtils
    {
        public static Texture2D TakeScreenshotForStepThumbnail(Camera source, int width, int height)
        {
            source.transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
            source.clearFlags = CameraClearFlags.Color;
            RenderTexture renderTexture = new RenderTexture(width, height, 24)
            {
                antiAliasing = 4
            };
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
            source.targetTexture = renderTexture;
            source.Render();
            source.targetTexture = null;
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
            texture2D.Apply();
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(renderTexture);
            return texture2D;
        }

        public static void RecordCameraPose(SnapShotData snapshot)
        {
            if (snapshot == null || Camera.main == null)
            {
                return;
            }

            var cameraTransform = Camera.main.transform;
            snapshot.HasCameraPose = true;
            snapshot.CameraPosition = cameraTransform.position;
            snapshot.CameraRotation = cameraTransform.rotation;
        }

        public static void ApplyCameraPose(SnapShotData snapshot)
        {
            if (snapshot == null || !snapshot.HasCameraPose || Camera.main == null)
            {
                return;
            }

            Camera.main.transform.SetPositionAndRotation(snapshot.CameraPosition, snapshot.CameraRotation);
        }

        public static void ApplyToSnapshot(Guid step)
        {
            if (MigElementWrapper.WrapperRoot == null)
            {
                return;
            }

            var elements = MigElementWrapper.WrapperRoot.GetComponentsInChildrenOnly<MigElementWrapper>(true).SelectMany(es => es.Elements).ToList();

            // first: revert all element to origin
            elements.Where(e => e.StepGUID == Guid.Empty)
                .ToList().ForEach(e => e.Apply());

            // second: apply current index
            elements.Where(e => e.StepGUID == step)
                    .ToList()
                    .ForEach(e => e.Apply());
        }

        public static void DeleteAllSnapshotOf(Guid guid)
        {
            if (MigElementWrapper.WrapperRoot == null)
            {
                return;
            }

            var wrappers = MigElementWrapper.WrapperRoot.GetComponentsInChildrenOnly<MigElementWrapper>(true);

            wrappers.ForEach((wrapper) =>
            {
                wrapper.Elements.RemoveAll(e => e.StepGUID == guid);
            });
        }

        public static void CloneAllSnapshot(Guid from, Guid to)
        {
            if (MigElementWrapper.WrapperRoot == null)
            {
                return;
            }

            var wrappers = MigElementWrapper.WrapperRoot.GetComponentsInChildrenOnly<MigElementWrapper>(true);

            wrappers.ForEach((wrapper) =>
            {
                wrapper.Elements.Where((a)=> a.StepGUID == from)
                .Select((a)=> a.Clone())
                .ToList()
                .ForEach(e=>
                {
                    e.Wrapper = wrapper;
                    wrapper.PushBackElement(e);
                    e.StepGUID = to;
                });
            });
        }
    }
}