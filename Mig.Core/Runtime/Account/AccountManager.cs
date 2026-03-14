using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mig
{
    public class AccountManager
    {
        private readonly static string AccountID = "949F599B-4B53-4B4D-BAD6-B856F7B91830";

        /// <summary>
        /// Get Current Account 
        /// TODO, might get from account server
        /// For some reason, I want to align this symbols.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAccountID()                  => AccountID;


    }

}

