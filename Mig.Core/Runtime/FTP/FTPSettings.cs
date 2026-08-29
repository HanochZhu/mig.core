using UnityEngine;

namespace Mig.Core
{
    [CreateAssetMenu(fileName = "FTPSettings", menuName = "Mig/FTP Settings")]
    public class FTPSettings : ScriptableObject
    {
        [Tooltip("FTP host, e.g. ftp://example.com/")]
        public string host = "";

        public string username = "mig";
        public string password = "migassets";

        [Tooltip("Optional account folder override. Leave empty to use AccountManager.")]
        public string accountId = "";
    }
}
