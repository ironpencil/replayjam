using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class DebugLogger
{
    public const string DEBUG_MODE = "RELEASE";

    public static void Log(string message)
    {
        if (DEBUG_MODE.Equals("DEBUG"))
        {
            Debug.Log(message);
        }
    }

    public static void LogWarning(string message)
    {
        if (DEBUG_MODE.Equals("DEBUG"))
        {
            Debug.LogWarning(message);
        }
    }

    public static void LogError(string message)
    {
        if (DEBUG_MODE.Equals("DEBUG"))
        {
            Debug.LogError(message);
        }
    }
}
