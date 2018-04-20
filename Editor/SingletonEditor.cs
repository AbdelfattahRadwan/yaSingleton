﻿using UnityEditor;
using UnityEngine;
using yaSingleton.Helpers;

namespace yaSingleton.Editor {
    /// <summary>
    /// Fallback Singleton Inspector - validates the singleton location. Extend this class if you want the validation in your singletons.
    /// Note: The editor targets ScriptableObjects due BaseSingleton being abstract (no custom inspectors for abstract types).
    /// </summary>
    [CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
    public class SingletonEditor : UnityEditor.Editor {
        protected bool isSingletonEditor;

        /// <summary>
        /// Disable in inherited editors to draw the validation at any point of your code.
        /// </summary>
        protected bool autoDrawSingletonValidation = true;

        protected virtual void OnEnable() {
            isSingletonEditor = target && SingletonInitializer.IsSingleton(target.GetType());
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if(autoDrawSingletonValidation) {
                DrawSingletonValidation();
            }
        }

        protected void DrawSingletonValidation() {
            if(!isSingletonEditor) {
                return;
            }

            var path = AssetDatabase.GetAssetPath(this);
            var separator = System.IO.Path.DirectorySeparatorChar;

            if(!string.IsNullOrEmpty(path) && !path.Contains(separator + "Resources" + separator)) {
                EditorGUILayout.HelpBox(
                    "All " + target.GetType().Name +
                    " (Singleton) instances must be in the Resources folder. Otherwise they won't be loaded outside of the Editor.",
                    MessageType.Warning, true);
            }
        }
    }
}