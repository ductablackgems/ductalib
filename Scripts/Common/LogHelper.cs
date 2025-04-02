﻿using UnityEngine;

namespace _0.DucLib.Scripts.Common
{
    public class LogHelper
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogYellow(string mess)
        {
#if DEBUG_NET
            Debug.Log($"<color=yellow>[LOG]{mess}</color>");
#endif
        }
        public static void LogPurple(string mess)
        {
#if DEBUG_NET
            Debug.Log($"<color=#FF00E6>[LOG]{mess}</color>");
#endif
        }
        public static void LogError([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
#if DEBUG_NET
            Debug.Log($"<color=red>[CHECK POINT]-[{memberName}]</color>");
#endif
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CheckPoint([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
#if DEBUG_NET
            Debug.Log($"<color=yellow>[CHECK POINT]-[{memberName}]</color>");
#endif
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