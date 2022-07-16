using System.Collections.Generic;
using UnityEngine;

namespace XVerse.Player.Input
{
    [System.Serializable]
    public class InputSetting
    {
        public string InputSettingName;
        public List<KeyboardInputGroup> KeyboardInputSetting;
        public List<MouseInputGroup> MouseInputSetting;

        public InputSetting(string name, List<KeyboardInputGroup> keySets, List<MouseInputGroup> mouseSets)
        {
            InputSettingName = name;
            KeyboardInputSetting = keySets;
            MouseInputSetting = mouseSets;
        }

        public InputSetting()
        {
            KeyboardInputSetting = new List<KeyboardInputGroup>();
            MouseInputSetting = new List<MouseInputGroup>();
        }

        public void CopyFrom(InputSetting inputSetting)
        {
            if (inputSetting == null) { return; }
            InputSettingName = inputSetting.InputSettingName;
            foreach (KeyboardInputGroup keyboardInputGroup in inputSetting.KeyboardInputSetting)
            {
                KeyboardInputGroup inputGroup = new KeyboardInputGroup();
                inputGroup.CopyFrom(keyboardInputGroup);
                KeyboardInputSetting.Add(inputGroup);
            }
            foreach (MouseInputGroup mouseInputGroup in inputSetting.MouseInputSetting)
            {
                MouseInputGroup inputGroup = new MouseInputGroup();
                inputGroup.CopyFrom(mouseInputGroup);
                MouseInputSetting.Add(inputGroup);
            }
        }

        private string getString<T, U>(string name) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            return new U() is KeyboardInput ? "KeyboardInput" : "MouseInput";
        }

        public U GetInput<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            string str = getString<T, U>(name);

            U input = null;
            foreach (T t in group)
            {
                input = t.GetInput(name);
                if (input != null) break;
            }
            if (input == null) { Debug.LogError($"{str} with name {name} doesn't exist"); }
            return input;
        }

        public T GetInputGroup<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            string str = getString<T, U>(name);

            T input = null;
            foreach (T t in group)
            {
                if (t.InputGroupName == name) { input = t; break; }
            }
            if (input == null) { Debug.LogError($"{str} with name {name} doesn't exist"); }
            return input;
        }

        public void InputLockAll<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(name, group).InputLockAll();
        }

        public void InputLockAll<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputLockAll();
            }
        }

        public void InputLockAll()
        {
            InputLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            InputLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
        }

        public void InputUnLockAll<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(name, group).InputUnLockAll();
        }

        public void InputUnLockAll<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputUnLockAll();
            }
        }

        public void InputUnLockAll()
        {
            InputUnLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            InputUnLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
        }

        public void InputUnLockOnly<T, U>(string name, List<T> group, bool isGroupName = true) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            if (isGroupName)
            {
                foreach (T t in group)
                {
                    if (t.InputGroupName.Equals(name)) { t.InputUnLockAll(); break; }
                    else { t.InputLockAll(); }
                }
            }
            else
            {
                foreach (T t in group)
                {
                    if (t.GetInput(name) != null) { t.InputUnLockOnly(name); break; }
                    else { t.InputLockAll(); }
                }
            }
        }

        public void InputUnLockOnly<T, U>(string groupname, string inputname, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(groupname, group).InputUnLockOnly(inputname);
        }

        public void InputUnLockOnly<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputUnLockAll();
            }
        }

        public void InputLockOnly<T, U>(string name, List<T> group, bool isGroupName = true) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            if (isGroupName)
            {
                foreach (T t in group)
                {
                    if (t.InputGroupName.Equals(name)) { t.InputLockAll(); break; }
                    else { t.InputUnLockAll(); }
                }
            }
            else
            {
                foreach (T t in group)
                {
                    if (t.GetInput(name) != null) { t.InputLockOnly(name); break; }
                    else { t.InputUnLockAll(); }
                }
            }
        }

        public void InputLockOnly<T, U>(string groupname, string inputname, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(groupname, group).InputLockOnly(inputname);
        }

        public void InputLockOnly<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputLockAll();
            }
        }

        public static InputSetting XtownLobbyInputSettingGroup
        {
            get
            {
                InputSetting inputSetting = new InputSetting();
                inputSetting.InputSettingName = "XtownLobbyInputSetting";

                KeyboardInputGroup MovementPlayerKeyboard = new KeyboardInputGroup();
                MovementPlayerKeyboard.InputGroupName = "Movement";
                MovementPlayerKeyboard.Inputs = new List<KeyboardInput>();
                MovementPlayerKeyboard.Inputs.Add(new KeyboardInput("Front", KeyboardInputName.W, KeyboardInputType.KeyDown));
                MovementPlayerKeyboard.Inputs.Add(new KeyboardInput("Back", KeyboardInputName.S, KeyboardInputType.KeyDown));
                MovementPlayerKeyboard.Inputs.Add(new KeyboardInput("Left", KeyboardInputName.A, KeyboardInputType.KeyDown));
                MovementPlayerKeyboard.Inputs.Add(new KeyboardInput("Right", KeyboardInputName.D, KeyboardInputType.KeyDown));
                MovementPlayerKeyboard.Inputs.Add(new KeyboardInput("Jump", KeyboardInputName.Space, KeyboardInputType.Key));

                KeyboardInputGroup EmotionPlayerKeyboard = new KeyboardInputGroup();
                EmotionPlayerKeyboard.InputGroupName = "Emotion";
                EmotionPlayerKeyboard.Inputs = new List<KeyboardInput>();
                EmotionPlayerKeyboard.Inputs.Add(new KeyboardInput("EmotionToggle", KeyboardInputName.T, KeyboardInputType.KeyDown));
                EmotionPlayerKeyboard.Inputs.Add(new KeyboardInput("Emotion1", KeyboardInputName.Alpha1, KeyboardInputType.KeyDown));
                EmotionPlayerKeyboard.Inputs.Add(new KeyboardInput("Emotion2", KeyboardInputName.Alpha2, KeyboardInputType.KeyDown));
                EmotionPlayerKeyboard.Inputs.Add(new KeyboardInput("Emotion3", KeyboardInputName.Alpha3, KeyboardInputType.KeyDown));
                EmotionPlayerKeyboard.Inputs.Add(new KeyboardInput("Emotion4", KeyboardInputName.Alpha4, KeyboardInputType.KeyDown));

                KeyboardInputGroup CameraPlayerKeyboard = new KeyboardInputGroup();
                CameraPlayerKeyboard.InputGroupName = "Camera";
                CameraPlayerKeyboard.Inputs = new List<KeyboardInput>();
                CameraPlayerKeyboard.Inputs.Add(new KeyboardInput("CameraViewChange", KeyboardInputName.G, KeyboardInputType.KeyDown));

                inputSetting.KeyboardInputSetting = new List<KeyboardInputGroup>
                {
                    MovementPlayerKeyboard,
                    EmotionPlayerKeyboard,
                    CameraPlayerKeyboard
                };

                MouseInputGroup CameraPlayerMouse = new MouseInputGroup();
                CameraPlayerMouse.InputGroupName = "Camera";
                CameraPlayerMouse.Inputs = new List<MouseInput>();
                CameraPlayerMouse.Inputs.Add(new MouseInput("CameraDrag", MouseInputName.Right, MouseInputType.MouseDown));
                CameraPlayerMouse.Inputs.Add(new MouseInput("CameraDragExit", MouseInputName.Right, MouseInputType.MouseUp));

                inputSetting.MouseInputSetting = new List<MouseInputGroup>
                {
                    CameraPlayerMouse
                };

                return inputSetting;
            }
        }
    }
}