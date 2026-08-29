using System;
using UnityEngine;
using Mig;

namespace Mig.Core
{
    [CreateAssetMenu(fileName = "SyncSettings", menuName = "Mig/Sync Settings")]
    public class SyncSettings : ScriptableObject
    {
        [Tooltip("HTTP sync server, e.g. http://127.0.0.1:8787")]
        public string baseUrl = "http://127.0.0.1:8787";

        public string apiToken = "mig-dev-token";

        [Tooltip("Optional account override. Leave empty to use AccountManager.")]
        public string accountId = "";

        [Tooltip("Use FTP even when an HTTP base URL is set.")]
        public bool preferFtp = false;

        private static bool loaded;
        private static string currentBaseUrl = "http://127.0.0.1:8787";
        private static string currentToken = "mig-dev-token";
        private static bool currentPreferFtp;

        public static string BaseUrl
        {
            get
            {
                EnsureLoaded();
                return currentBaseUrl;
            }
        }

        public static string ApiToken
        {
            get
            {
                EnsureLoaded();
                return currentToken;
            }
        }

        public static bool PreferFtp
        {
            get
            {
                EnsureLoaded();
                return currentPreferFtp;
            }
        }

        public static bool HasHttpEndpoint => !string.IsNullOrWhiteSpace(BaseUrl);

        public static void EnsureLoaded()
        {
            if (loaded)
            {
                return;
            }

            loaded = true;
            var settings = Resources.Load<SyncSettings>("SyncSettings");
            if (settings != null)
            {
                if (!string.IsNullOrWhiteSpace(settings.baseUrl))
                {
                    currentBaseUrl = settings.baseUrl.Trim().TrimEnd('/');
                }
                else
                {
                    currentBaseUrl = "";
                }

                if (!string.IsNullOrWhiteSpace(settings.apiToken))
                {
                    currentToken = settings.apiToken.Trim();
                }

                currentPreferFtp = settings.preferFtp;

                if (!string.IsNullOrWhiteSpace(settings.accountId))
                {
                    AccountManager.SetCurrentAccountID(settings.accountId);
                }
            }

            ApplyRuntimeOverrides();
        }

        /// <summary>
        /// Launch args and PlayerPrefs win over the asset so a shipped build
        /// can point at a new host without rebuilding.
        /// </summary>
        private static void ApplyRuntimeOverrides()
        {
            var prefsUrl = PlayerPrefs.GetString("Mig.Sync.BaseUrl", "");
            if (!string.IsNullOrWhiteSpace(prefsUrl))
            {
                currentBaseUrl = prefsUrl.Trim().TrimEnd('/');
            }

            var prefsToken = PlayerPrefs.GetString("Mig.Sync.ApiToken", "");
            if (!string.IsNullOrWhiteSpace(prefsToken))
            {
                currentToken = prefsToken.Trim();
            }

            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-syncUrl" && !string.IsNullOrWhiteSpace(args[i + 1]))
                {
                    currentBaseUrl = args[i + 1].Trim().TrimEnd('/');
                }
                if (args[i] == "-syncToken" && !string.IsNullOrWhiteSpace(args[i + 1]))
                {
                    currentToken = args[i + 1].Trim();
                }
            }
        }
    }
}
