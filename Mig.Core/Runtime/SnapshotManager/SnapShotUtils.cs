using System.Linq;
using UnityEngine;
using Mig.Core;
using System;

namespace Mig.Snapshot
{
    public static class SnapShotUtils
    {
        public static Texture2D TakeScreenshotForStepThumbail(Camera source, int width, int height)
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

        public static void ApplyToSnapshot(Guid step)
        {
            var elements = MigElementWrapper.WrapperRoot.GetComponentsInChildrenOnly<MigElementWrapper>().SelectMany(es => es.Elements).ToList();

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
            var wrappers = MigElementWrapper.WrapperRoot.GetComponentsInChildrenOnly<MigElementWrapper>();

            wrappers.ForEach((wrapper) =>
            {
                wrapper.Elements.RemoveAll(e => e.StepGUID == guid);
            });
        }
    }
}