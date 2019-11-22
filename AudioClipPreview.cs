using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Syy.Tools
{
    public class AudioClipPreview : EditorWindow
    {
        [MenuItem("Window/AudioClipPreview")]
        public static void Open()
        {
            GetWindow<AudioClipPreview>(nameof(AudioClipPreview));
        }

        void OnEnable()
        {
            Type audioUtil = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            PlayMethod = audioUtil.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );

            StopMethod = audioUtil.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { },
                null
            );

            Selection.selectionChanged += OnSelectionChanged;
        }

        void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        static MethodInfo PlayMethod;
        static MethodInfo StopMethod;

        void OnSelectionChanged()
        {
            StopMethod.Invoke(null, null);
            AudioClip[] clips = Selection.GetFiltered<AudioClip>(SelectionMode.Unfiltered);
            if (clips == null || clips.Length <= 0) return;
            PlayMethod.Invoke(null, new object[] { clips[0], 0, false });
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("選択したAudioClipを再生");
        }
    }
}
