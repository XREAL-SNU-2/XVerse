using System;
using UnityEngine;
using System.Collections.Generic;

namespace XPlayer.Input.Keyboard
{
    public enum KeyboardInputName
    {
        BackQuote, Alpha1, Alpha2, Alpha3, Alpha4, Alpha5,
        Alpha6, Alpha7, Alpha8, Alpha9, Alpha0, Minus, Plus, Backspace,
        Tab, Q, W, E, R, T, Y, U, I, O, P, LeftBracket, RightBracket, Backslash,
        CapsLock, A, S, D, F, G, H, J, K, L, Semicolon, Quote, Return,
        LeftShift, Z, X, C, V, B, N, M, Comma, Period, Slash, RightShift,
        LeftControl, LeftCommand, Space, UpArrow, LeftArrow, DownArrow, RightArrow
    };

    public enum KeyboardInputType
    {
        KeyDown, Key, KeyUp
    }

    [Serializable]
    public class KeyboardInput : PlayerInput
    {
        public KeyboardInputName inputKeyName;
        public KeyboardInputType inputKeyType;
        public static string InputKeyName_Prop_Name => nameof(inputKeyName);
        public static string InputKeyType_Prop_Name => nameof(inputKeyType);

        public bool IsInput
        {
            get
            {
                switch (inputKeyType)
                {
                    case KeyboardInputType.Key:
                        return isActiveInput && UnityEngine.Input.GetKey(ToKeyCode());
                    case KeyboardInputType.KeyDown:
                        return isActiveInput && UnityEngine.Input.GetKeyDown(ToKeyCode());
                    case KeyboardInputType.KeyUp:
                        return isActiveInput && UnityEngine.Input.GetKeyUp(ToKeyCode());
                    default:
                        Debug.LogError("KeyboardInputType Error");
                        return false;
                }
            }
        }

        public KeyboardInput(string name, KeyboardInputName keyName, KeyboardInputType keyType)
        {
            InputName = name;
            isActiveInput = true;
            if (Enum.IsDefined(typeof(KeyCode), inputKeyName.ToString()))
            {
                inputKeyName = keyName;
                inputKeyType = keyType;
            }
        }

        public KeyboardInput()
        {
            isActiveInput = true;
            inputKeyName = KeyboardInputName.W;
            inputKeyType = KeyboardInputType.Key;
        }

        public KeyCode ToKeyCode()
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), inputKeyName.ToString());
        }

        public void SetKeyboardInputName(KeyboardInputName keyName)
        {
            if (Enum.IsDefined(typeof(KeyCode), inputKeyName.ToString()))
            {
                inputKeyName = keyName;
            }
        }

        public void CopyFrom(KeyboardInput keyboard)
        {
            if (keyboard == null) { return; }
            InputName = keyboard.InputName;
            inputKeyName = keyboard.inputKeyName;
            inputKeyType = keyboard.inputKeyType;
        }
    }

    [Serializable]
    public class KeyboardInputGroup : PlayerInputGroup<KeyboardInput>
    {
        public KeyboardInputGroup(string name) : this()
        {
            InputGroupName = name;
        }

        public KeyboardInputGroup(string name, List<KeyboardInput> keySet) : this(name)
        {
            Inputs = keySet;
        }

        public KeyboardInputGroup()
        {
            Inputs = new List<KeyboardInput>();
        }

        public void CopyFrom(KeyboardInputGroup keyboardGroup)
        {
            if (keyboardGroup == null) { return; }
            foreach(KeyboardInput keyboard in keyboardGroup.Inputs)
            {
                KeyboardInput input = new KeyboardInput();
                input.CopyFrom(keyboard);
                Inputs.Add(input);
            }
            InputGroupName = keyboardGroup.InputGroupName;
        }
    }

}