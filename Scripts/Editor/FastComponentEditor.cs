#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace _0.DucLib.Scripts.Editor
{
    public static class FastComponentEditor
    {
        [MenuItem("DucTATool/FastComponent/Copy Component %&c")]
        private static void CopyComponent()
        {
            var tracker = ActiveEditorTracker.sharedTracker;
            if (tracker.activeEditors.Length == 0) return;

            var targetComponent = tracker.activeEditors[0].target as Component;
            if (targetComponent == null || targetComponent is Transform) return;

            UnityEditorInternal.ComponentUtility.CopyComponent(targetComponent);
            Debug.Log($"✅ Copied: {targetComponent.GetType().Name}");
        }

        [MenuItem("DucTATool/FastComponent/Paste Component %&v")]
        private static void PasteComponent()
        {
            var selectedGO = Selection.activeGameObject;
            if (selectedGO == null) return;

            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(selectedGO);
            Debug.Log($"📋 Pasted to: {selectedGO.name}");
        }

        [MenuItem("DucTATool/FastComponent/Remove Component %&r")]
        private static void RemoveComponent()
        {
            var tracker = ActiveEditorTracker.sharedTracker;
            if (tracker.activeEditors.Length == 0) return;
            var targetComponent = tracker.activeEditors[0].target as Component;
            if (targetComponent == null || targetComponent is Transform) return;
            Object.DestroyImmediate(targetComponent);
            Debug.Log($"🗑️ Removed: {targetComponent.GetType().Name}");
        }
    }
}
#endif