using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;


namespace XVerse.Player.Input
{
    public class KeyboardInputWindow : EditorWindow
    {
        private static float offset = 5;
        private static int keyboardCellSize = 30;
        private static Color unassignedKeyColor = Color.grey, assignedKeyColor = Color.cyan;

        private int setIndex, groupIndex, inputIndex;
        private Dictionary<KeyCode, List<string>> keyCodeInputSetting;

        public static void ShowWindow(int set, int group, int input, ref bool open, ref KeyboardInputWindow inputwindow)
        {
            var window = CreateInstance<KeyboardInputWindow>();

            open = true;
            inputwindow = window;

            window.setIndex = set;
            window.groupIndex = group;
            window.inputIndex = input;
            window.titleContent = new GUIContent(String.Format("{0} / {1} / Keyboard Input", XInput.Instance[window.setIndex].KeyboardInputSetting[window.groupIndex].InputGroupName, XInput.Instance[window.setIndex].KeyboardInputSetting[window.groupIndex].Inputs[window.inputIndex].InputName));
            window.minSize = new Vector2(keyboardCellSize * 15 + offset * 15, keyboardCellSize * 5 + offset * 6);
            window.maxSize = window.minSize;

            window.keyCodeInputSetting = new Dictionary<KeyCode, List<string>>();
            foreach (KeyboardInputGroup keyboardInputGroup in XInput.Instance[window.setIndex].KeyboardInputSetting)
            {
                foreach (KeyboardInput keyboardInput in keyboardInputGroup.Inputs)
                {
                    KeyCode inputKeyName = (KeyCode)Enum.Parse(typeof(KeyCode), keyboardInput.InputKeyName.ToString());
                    string inputKeyInfo = keyboardInputGroup.InputGroupName + " / " + keyboardInput.InputName;
                    if (!window.keyCodeInputSetting.ContainsKey(inputKeyName))
                    {
                        window.keyCodeInputSetting.Add(inputKeyName, new List<string>() { inputKeyInfo });
                    }
                    else
                    {
                        window.keyCodeInputSetting[inputKeyName].Add(inputKeyInfo);
                    }
                }
            }
            window.ShowUtility();
        }

        private static string[][] keyboardName = new string[][]
        {
            new string[]{"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+", "Back"},
            new string[]{"Tab", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "[", "]", "|"},
            new string[]{"Caps", "A", "S", "D", "F","G", "H", "J", "K", "L", ";", "'", "Enter"},
            new string[]{"Shift", "Z", "X", "C", "V", "B", "N", "M", ",", ".", "/", "Shift"},
            new string[]{"Control", "Alt", "Space", "ก่", "ก็", "ก้", "กๆ"}
        };

        private static KeyCode[][] keyboardCode = new KeyCode[][]
        {
            new KeyCode[]{KeyCode.BackQuote, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
                KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus, KeyCode.Plus, KeyCode.Backspace},
            new KeyCode[]{KeyCode.Tab, KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
                KeyCode.LeftBracket, KeyCode.RightBracket, KeyCode.Backslash},
            new KeyCode[]{KeyCode.CapsLock, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
                KeyCode.Semicolon, KeyCode.Quote, KeyCode.Return},
            new KeyCode[]{KeyCode.LeftShift, KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M,
                KeyCode.Comma, KeyCode.Period, KeyCode.Slash, KeyCode.RightShift},
            new KeyCode[]{KeyCode.LeftControl, KeyCode.LeftCommand, KeyCode.Space, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow}
        };

        private static int[][] keyboardSize = new int[][]
        {
            new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,2},
            new int[]{2,1,1,1,1,1,1,1,1,1,1,1,1,1},
            new int[]{2,1,1,1,1,1,1,1,1,1,1,1,2},
            new int[]{2,1,1,1,1,1,1,1,1,1,2,2},
            new int[]{2,2,7,1,1,1,1}
        };

        private static float[] offsets = new float[] { 0, 0, 1, 2, 7 };

        private string tooltipString(List<string> info)
        {
            string str = "";
            if (info == null || info.Count == 0) { return str; }
            for (int i = 0; i < info.Count; i++)
            {
                str += info[i].ToString();
                if (i != info.Count - 1) { str += "\n"; }
            }
            return str;
        }

        private void OnGUI()
        {
            /*
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = unassignedKeyColor;
            GUILayout.Box("", GUILayout.Width(lineHeight), GUILayout.Height(lineHeight));
            GUI.backgroundColor = Color.white;
            GUILayout.Label("Unassigned Key");
            GUILayout.Space(20);
            GUI.backgroundColor = assignedKeyColor;
            GUILayout.Box("", GUILayout.Width(lineHeight), GUILayout.Height(lineHeight));
            GUI.backgroundColor = Color.white;
            GUILayout.Label("Assigned Key");
            GUILayout.EndHorizontal();*/

            for (int i = 0; i < keyboardName.Length; i++)
            {
                float x = 0;
                float y = keyboardCellSize * i + offset * (i + 1);
                string tooltip;

                for (int j = 0; j < keyboardName[i].Length; j++)
                {
                    if (j == 0) { x = offset + offset * offsets[i] / 2; }
                    else { x += keyboardCellSize * keyboardSize[i][j - 1] + offset; }

                    if (keyCodeInputSetting.ContainsKey(keyboardCode[i][j]))
                    {
                        tooltip = tooltipString(keyCodeInputSetting[keyboardCode[i][j]]);
                        var content = new GUIContent(keyboardName[i][j], tooltip);
                        GUI.backgroundColor = assignedKeyColor;
                        if (GUI.Button(new Rect(x, y, keyboardCellSize * keyboardSize[i][j], keyboardCellSize), content))
                        {
                            XInput.Instance[setIndex].KeyboardInputSetting[groupIndex].Inputs[inputIndex].InputKeyName = (KeyboardInputName)Enum.Parse(typeof(KeyboardInputName), keyboardCode[i][j].ToString());
                            this.Close();
                        }
                    }
                    else
                    {
                        var content = new GUIContent(keyboardName[i][j]);
                        GUI.backgroundColor = unassignedKeyColor;
                        if (GUI.Button(new Rect(x, y, keyboardCellSize * keyboardSize[i][j], keyboardCellSize), content))
                        {
                            XInput.Instance[setIndex].KeyboardInputSetting[groupIndex].Inputs[inputIndex].InputKeyName = (KeyboardInputName)Enum.Parse(typeof(KeyboardInputName), keyboardCode[i][j].ToString());
                            this.Close();
                        }
                    }

                }
            }
        }
    }
}