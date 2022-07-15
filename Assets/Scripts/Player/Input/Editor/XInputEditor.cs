using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace XVerse.Player.Input
{

    [CustomEditor(typeof(XInput))]
    public class XInputEditor : Editor
    {
        private static float lineHeight = EditorGUIUtility.singleLineHeight;
        private ReorderableList inputSettingList;

        [MenuItem("Assets/Open XInput")]
        public static void OpenInspector()
        {
            Selection.activeObject = XInput.Instance;
        }

        private void OnEnable()
        {
            inputSettingList = new ReorderableList(XInput.Instance.PlayerInputSettings, typeof(InputSetting));

            inputSettingList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Player Input Settings");
            };
            inputSettingList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                float length = EditorGUIUtility.currentViewWidth - 65;
                rect.height = lineHeight;
                rect.y += 3;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(new Rect(rect.x, rect.y, length - rect.x - 5, rect.height), XInput.Instance.PlayerInputSettings[index].InputSettingName);
                EditorGUI.EndDisabledGroup();
                if (GUI.Button(new Rect(length, rect.y, 55, rect.height), "Open")) { InputSettingWindow.ShowWindow(index); }
            };
            inputSettingList.onAddCallback = (list) =>
            {
                XInput.Instance.PlayerInputSettings.Add(new InputSetting());
                list.index = XInput.Instance.PlayerInputSettings.Count - 1;
                XInput.Instance.PlayerInputSettings[list.index].InputSettingName = String.Format("New Input Setting ({0})", list.index + 1);
            };
            inputSettingList.onRemoveCallback = (list) =>
            {
                XInput.Instance.PlayerInputSettings.RemoveAt(list.index);
                if (list.index >= XInput.Instance.PlayerInputSettings.Count) { list.index = XInput.Instance.PlayerInputSettings.Count - 1; }
            };
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Player Input Setting", EditorStyles.boldLabel);
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(true);
            XInput.Instance.PresentIndex = EditorGUILayout.IntField("Index", XInput.Instance.PresentIndex);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(5);
            inputSettingList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}