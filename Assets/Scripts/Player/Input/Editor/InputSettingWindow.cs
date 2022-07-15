using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace XVerse.Player.Input
{
    public class InputSettingWindow : EditorWindow
    {
        private static float offset = 5;
        private static float lineHeight = EditorGUIUtility.singleLineHeight;
        private static float groupWidth = 220, inputWidth = 380, windowHeight = 200, buttonSize = 20;

        private int setIndex;
        private bool isOpen;
        private KeyboardInputWindow inputWindow;
        private ReorderableList keyboardGroupList, keyboardInputList, mouseGroupList, mouseInputList;
        private Vector2[] scrollPos = new Vector2[4];

        public static void ShowWindow(int index)
        {
            var window = CreateInstance<InputSettingWindow>();
            window.setIndex = index;
            window.isOpen = false;
            window.titleContent = new GUIContent(XInput.Instance[index].InputSettingName);
            window.minSize = new Vector2(groupWidth + inputWidth + offset * 3, lineHeight * 5 + buttonSize * 2 + windowHeight * 2 + offset * 10);
            window.maxSize = window.minSize;

            // Keyboard
            window.keyboardGroupList = new ReorderableList(XInput.Instance[window.setIndex].KeyboardInputSetting, typeof(KeyboardInputGroup), true, false, false, false);

            window.keyboardGroupList.elementHeight = lineHeight + offset;
            window.keyboardGroupList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height = lineHeight;
                rect.y += 3;
                XInput.Instance[window.setIndex].KeyboardInputSetting[index].InputGroupName = EditorGUI.TextField(rect, XInput.Instance[window.setIndex].KeyboardInputSetting[index].InputGroupName);
            };
            window.keyboardGroupList.onAddCallback = (list) =>
            {
                XInput.Instance[window.setIndex].KeyboardInputSetting.Add(new KeyboardInputGroup());
                list.index = XInput.Instance[window.setIndex].KeyboardInputSetting.Count - 1;
                window.keyboardInputList.list = XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs;
                XInput.Instance[window.setIndex].KeyboardInputSetting[list.index].InputGroupName = String.Format("New Input Setting ({0})", list.index + 1);
            };
            window.keyboardGroupList.onRemoveCallback = (list) =>
            {
                XInput.Instance[window.setIndex].KeyboardInputSetting.RemoveAt(list.index);
                list.index = window.getKeyboardIndex();
                if (list.count > 0) { window.keyboardInputList.list = XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs; }
                else { window.keyboardInputList.list = new KeyboardInputGroup().Inputs; }
            };
            window.keyboardGroupList.onCanRemoveCallback = (list) =>
            {
                return (XInput.Instance[window.setIndex].KeyboardInputSetting?.Count ?? 0) > 0;
            };
            window.keyboardGroupList.onSelectCallback = (list) =>
            {
                window.keyboardInputList.list = XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs;
            };

            window.keyboardInputList = new ReorderableList(window.keyboardGroupList.count > 0 ? XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs : new KeyboardInputGroup().Inputs, typeof(KeyboardInput), true, false, false, false);
            window.keyboardInputList.elementHeight = lineHeight + offset;
            window.keyboardInputList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height = lineHeight;
                rect.y += 3;
                XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[index].InputName = EditorGUI.TextField(new Rect(rect.position.x, rect.position.y, 160, rect.height), XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[index].InputName);
                XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[index].InputKeyType = (KeyboardInputType)EditorGUI.EnumPopup(new Rect(rect.position.x + 165, rect.position.y, 100, rect.height), XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[index].InputKeyType);
                if (GUI.Button(new Rect(rect.position.x + 270, rect.position.y, 85, rect.height), XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[index].InputKeyName.ToString())) { KeyboardInputWindow.ShowWindow(window.setIndex, window.keyboardGroupList.index, index, ref window.isOpen, ref window.inputWindow); }
            };
            window.keyboardInputList.onAddCallback = (list) =>
            {
                XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs.Add(new KeyboardInput());
                list.index = XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs.Count - 1;
                XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs[list.index].InputName = String.Format("New Input ({0})", list.index + 1);
            };
            window.keyboardInputList.onCanAddCallback = (list) =>
            {
                return (window.keyboardGroupList?.count ?? 0) > 0;
            };
            window.keyboardInputList.onRemoveCallback = (list) =>
            {
                XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs.RemoveAt(list.index);
                list.index = XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs.Count - 1 > 0 ? XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()].Inputs.Count - 1 : 0; ;
            };
            window.keyboardInputList.onCanRemoveCallback = (list) =>
            {
                return ((window.keyboardGroupList?.count ?? 0) > 0) && ((XInput.Instance[window.setIndex].KeyboardInputSetting[window.getKeyboardIndex()]?.Inputs.Count ?? 0) > 0);
            };

            // Mouse
            window.mouseGroupList = new ReorderableList(XInput.Instance[window.setIndex].MouseInputSetting, typeof(MouseInputGroup), true, false, false, false);

            window.mouseGroupList.elementHeight = lineHeight + offset;
            window.mouseGroupList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height = lineHeight;
                rect.y += 3;
                XInput.Instance[window.setIndex].MouseInputSetting[index].InputGroupName = EditorGUI.TextField(rect, XInput.Instance[window.setIndex].MouseInputSetting[index].InputGroupName);
            };
            window.mouseGroupList.onAddCallback = (list) =>
            {
                XInput.Instance[window.setIndex].MouseInputSetting.Add(new MouseInputGroup());
                list.index = XInput.Instance[window.setIndex].MouseInputSetting.Count - 1;
                window.mouseInputList.list = XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs;
                XInput.Instance[window.setIndex].MouseInputSetting[list.index].InputGroupName = String.Format("New Input Setting ({0})", list.index + 1);
            };
            window.mouseGroupList.onRemoveCallback = (list) =>
            {
                XInput.Instance[window.setIndex].MouseInputSetting.RemoveAt(list.index);
                list.index = window.getMouseIndex();
                if (list.count > 0) { window.mouseInputList.list = XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs; }
                else { window.mouseInputList.list = new MouseInputGroup().Inputs; }
            };
            window.mouseGroupList.onCanRemoveCallback = (list) =>
            {
                return (XInput.Instance[window.setIndex].MouseInputSetting?.Count ?? 0) > 0;
            };
            window.mouseGroupList.onSelectCallback = (list) =>
            {
                window.mouseInputList.list = XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs;
            };

            window.mouseInputList = new ReorderableList(window.mouseGroupList.count > 0 ? XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs : new MouseInputGroup().Inputs, typeof(MouseInput), true, false, false, false);
            window.mouseInputList.elementHeight = lineHeight + offset;
            window.mouseInputList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height = lineHeight;
                rect.y += 3;
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputName = EditorGUI.TextField(new Rect(rect.position.x, rect.position.y, 160, rect.height), XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputName);
                EditorGUI.BeginChangeCheck();
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseType = (MouseInputType)EditorGUI.EnumPopup(new Rect(rect.position.x + 165, rect.position.y, 100, rect.height), XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseType);
                if (EditorGUI.EndChangeCheck() && XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseType == MouseInputType.Drag)
                {
                    XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseName = MouseInputName.Wheel;
                }
                EditorGUI.BeginDisabledGroup(XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseType == MouseInputType.Drag);
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseName = (MouseInputName)EditorGUI.EnumPopup(new Rect(rect.position.x + 270, rect.position.y, 85, rect.height), XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].InputMouseName);
                EditorGUI.EndDisabledGroup();
                //if (GUI.Button(new Rect(rect.position.x + 270, rect.position.y, 85, rect.height), XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[index].inputMouseName.ToString())) { MouseInputWindow.ShowWindow(window.setIndex, window.mouseGroupList.index, index, ref window.isOpen, ref window.inputWindow); }
            };
            window.mouseInputList.onAddCallback = (list) =>
            {
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs.Add(new MouseInput());
                list.index = XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs.Count - 1;
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs[list.index].InputName = String.Format("New Input ({0})", list.index + 1);
            };
            window.mouseInputList.onCanAddCallback = (list) =>
            {
                return (window.mouseGroupList?.count ?? 0) > 0;
            };
            window.mouseInputList.onRemoveCallback = (list) =>
            {
                XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs.RemoveAt(list.index);
                list.index = XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs.Count - 1 > 0 ? XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()].Inputs.Count - 1 : 0; ;
            };
            window.mouseInputList.onCanRemoveCallback = (list) =>
            {
                return ((window.mouseGroupList?.count ?? 0) > 0) && ((XInput.Instance[window.setIndex].MouseInputSetting[window.getMouseIndex()]?.Inputs.Count ?? 0) > 0);
            };

            window.ShowUtility();
        }

        private int getKeyboardIndex()
        {
            int index = keyboardGroupList.index;
            index = index < (XInput.Instance[setIndex].KeyboardInputSetting?.Count ?? 0) ? index : XInput.Instance[setIndex].KeyboardInputSetting.Count - 1;
            return index >= 0 ? index : 0;
        }
        private int getMouseIndex()
        {
            int index = mouseGroupList.index;
            index = index < (XInput.Instance[setIndex].MouseInputSetting?.Count ?? 0) ? index : XInput.Instance[setIndex].MouseInputSetting.Count - 1;
            return index >= 0 ? index : 0;
        }

        private void OnFocus()
        {
            if (isOpen) { inputWindow.Close(); }
        }

        private void OnGUI()
        {
            XInput.Instance.PlayerInputSettings[setIndex].InputSettingName = EditorGUI.TextField(new Rect(offset, offset, groupWidth + inputWidth, lineHeight), "Input Setting Name", XInput.Instance.PlayerInputSettings[setIndex].InputSettingName);

            // Keyboard
            EditorGUI.LabelField(new Rect(offset, offset * 2 + lineHeight, groupWidth + inputWidth, lineHeight), "1. Keboard Input Setting");

            EditorGUI.LabelField(new Rect(offset, offset * 3 + lineHeight * 2, groupWidth + inputWidth, lineHeight), "Category");
            EditorGUI.DrawRect(new Rect(offset, offset * 4 + lineHeight * 3, groupWidth, windowHeight), new Color(42 / 256f, 42 / 256f, 42 / 256f));
            GUILayout.BeginArea(new Rect(offset, offset * 4 + lineHeight * 3, groupWidth, windowHeight));
            scrollPos[0] = GUILayout.BeginScrollView(scrollPos[0], GUILayout.Height(windowHeight));
            keyboardGroupList.DoLayoutList();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            if (GUI.Button(new Rect(groupWidth - buttonSize * 2, offset * 5 + lineHeight * 3 + windowHeight, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to the list")))
                keyboardGroupList.onAddCallback(keyboardGroupList);
            EditorGUI.BeginDisabledGroup(!keyboardGroupList.onCanRemoveCallback(keyboardGroupList));
            if (GUI.Button(new Rect(groupWidth - buttonSize + offset, offset * 5 + lineHeight * 3 + windowHeight, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selection from the list")))
                keyboardGroupList.onRemoveCallback(keyboardGroupList);
            EditorGUI.EndDisabledGroup();

            EditorGUI.LabelField(new Rect(offset * 2 + groupWidth, offset * 3 + lineHeight * 2, groupWidth + inputWidth, lineHeight), "Command / Type / Input");
            EditorGUI.DrawRect(new Rect(offset * 2 + groupWidth, offset * 4 + lineHeight * 3, inputWidth, windowHeight), new Color(42 / 256f, 42 / 256f, 42 / 256f));
            GUILayout.BeginArea(new Rect(offset * 2 + groupWidth, offset * 4 + lineHeight * 3, inputWidth, windowHeight));
            scrollPos[1] = GUILayout.BeginScrollView(scrollPos[1], GUILayout.Height(windowHeight));
            keyboardInputList.DoLayoutList();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            EditorGUI.BeginDisabledGroup(!keyboardInputList.onCanAddCallback(keyboardInputList));
            if (GUI.Button(new Rect(groupWidth + inputWidth + offset - buttonSize * 2, offset * 5 + lineHeight * 3 + windowHeight, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to the list")))
                keyboardInputList.onAddCallback(keyboardInputList);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!keyboardInputList.onCanRemoveCallback(keyboardInputList));
            if (GUI.Button(new Rect(groupWidth + inputWidth + offset * 2 - buttonSize, offset * 5 + lineHeight * 3 + windowHeight, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selection from the list")))
                keyboardInputList.onRemoveCallback(keyboardInputList);
            EditorGUI.EndDisabledGroup();

            // Mouse
            EditorGUI.LabelField(new Rect(offset, offset * 6 + lineHeight * 3 + windowHeight + buttonSize, groupWidth + inputWidth, lineHeight), "2. Mouse Input Setting");

            EditorGUI.LabelField(new Rect(offset, offset * 7 + lineHeight * 4 + windowHeight + buttonSize, groupWidth, lineHeight), "Category");
            EditorGUI.DrawRect(new Rect(offset, offset * 8 + lineHeight * 5 + windowHeight + buttonSize, groupWidth, windowHeight), new Color(42 / 256f, 42 / 256f, 42 / 256f));
            GUILayout.BeginArea(new Rect(offset, offset * 8 + lineHeight * 5 + windowHeight + buttonSize, groupWidth, windowHeight));
            scrollPos[2] = GUILayout.BeginScrollView(scrollPos[2], GUILayout.Height(windowHeight));
            mouseGroupList.DoLayoutList();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            if (GUI.Button(new Rect(groupWidth - buttonSize * 2, offset * 9 + lineHeight * 5 + windowHeight * 2 + buttonSize, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to the list")))
                mouseGroupList.onAddCallback(mouseGroupList);
            EditorGUI.BeginDisabledGroup(!mouseGroupList.onCanRemoveCallback(mouseGroupList));
            if (GUI.Button(new Rect(groupWidth - buttonSize + offset, offset * 9 + lineHeight * 5 + windowHeight * 2 + buttonSize, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selection from the list")))
                mouseGroupList.onRemoveCallback(mouseGroupList);
            EditorGUI.EndDisabledGroup();

            EditorGUI.LabelField(new Rect(offset * 2 + groupWidth, offset * 7 + lineHeight * 4 + windowHeight + buttonSize, groupWidth + inputWidth, lineHeight), "Command / Type / Input");
            EditorGUI.DrawRect(new Rect(offset * 2 + groupWidth, offset * 8 + lineHeight * 5 + windowHeight + buttonSize, inputWidth, windowHeight), new Color(42 / 256f, 42 / 256f, 42 / 256f));
            GUILayout.BeginArea(new Rect(offset * 2 + groupWidth, offset * 8 + lineHeight * 5 + windowHeight + buttonSize, inputWidth, windowHeight));
            scrollPos[3] = GUILayout.BeginScrollView(scrollPos[3], GUILayout.Height(windowHeight));
            mouseInputList.DoLayoutList();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            EditorGUI.BeginDisabledGroup(!mouseInputList.onCanAddCallback(mouseInputList));
            if (GUI.Button(new Rect(groupWidth + inputWidth + offset - buttonSize * 2, offset * 9 + lineHeight * 5 + windowHeight * 2 + buttonSize, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to the list")))
                mouseInputList.onAddCallback(mouseInputList);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!mouseInputList.onCanRemoveCallback(mouseInputList));
            if (GUI.Button(new Rect(groupWidth + inputWidth + offset * 2 - buttonSize, offset * 9 + lineHeight * 5 + windowHeight * 2 + buttonSize, buttonSize, buttonSize), EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selection from the list")))
                mouseInputList.onRemoveCallback(mouseInputList);
            EditorGUI.EndDisabledGroup();
        }
    }
}