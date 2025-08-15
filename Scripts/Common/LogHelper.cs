﻿using UnityEngine;

namespace _0.DucLib.Scripts.Common
{
    public class LogHelper
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogYellow(string mess)
        {
            Debug.Log($"<color=yellow>[LOG]{mess}</color>");
        }
        public static void LogPurple(string mess)
        {
            Debug.Log($"<color=#FF00E6>[LOG]{mess}</color>");
        }
        public static void LogError([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=red>[CHECK POINT]-[{memberName}]</color>");
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CheckPoint([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=yellow>[CHECK POINT]-[{memberName}]</color>");
        }
        public static void CheckCode([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=#FFA500>[Code comment]-[{memberName}]</color>"); // was: yellow
        }
        public static void LogLine(
            string value = "",
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0
        )
        {
            Debug.Log($"<color=yellow>[CHECK LINE]-[{memberName}] at line {lineNumber} : {value}</color>");
        }
        public static void Todo(string todo,[System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=#1AFFFF>[TODO]-[{memberName}] - {todo}</color>");
        }
        public static void CheckPoint(string mess,[System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=yellow>[CHECK POINT]-[{memberName}] - {mess}</color>");
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CheckPointRemove([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=yellow>[COMMENT CODE][{memberName}]</color>");
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CheckPointBoolRemove([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            Debug.Log($"<color=yellow>[Return true][{memberName}]</color>");
        }
        
    }
}