using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugConsole
{
    public class DebugConsole : MonoBehaviour
    {
        static string myLog = "";
        private string output;
        private string stack;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + "\n" + myLog;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
            }
        }

        void OnGUI()
        {
            // if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                myLog = GUI.TextArea(new Rect(0, Screen.height / 2, Screen.width / 3, Screen.height / 2), myLog);
            }
        }
    }
}