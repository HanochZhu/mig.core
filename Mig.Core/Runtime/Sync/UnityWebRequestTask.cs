using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Mig.Core
{
    internal static class UnityWebRequestTask
    {
        public static async Task<UnityWebRequest> Send(UnityWebRequest request, CancellationToken token = default)
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                if (token.IsCancellationRequested)
                {
                    request.Abort();
                    break;
                }

                await Task.Yield();
            }

            return request;
        }

        public static bool IsSuccess(UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.Success;
        }
    }
}
