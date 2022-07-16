using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace XVerse.Player.Input
{
    /// <summary>
    /// XInput is Input Manager which is Singleton ScriptableObject
    /// </summary>
    public class XInput : ScriptableObject
    {
        private const string SettingFileDirectory = "Assets/Resources";
        private const string SettingFilePath = "Assets/Resources/XInput.asset";

        // lazy initialize singleton
        private static XInput _instance;

        /// <summary> XInput Singleton Instance </summary>
        public static XInput Instance
        {
            get
            {
                if (_instance != null)
                {
                    if (_instance.PlayerInputSettings == null)
                    {
                        _instance.PlayerInputSettings = new List<InputSetting>();
                    }
                    _instance.PresentIndex = 0;
                    return _instance;
                }
                // if _instance is null and UISetting.asset exist in Resources folder, you get UISetting
                _instance = Resources.Load<XInput>("XInput");

                // but, UISetting.asset has never been made, make it the first time you approach it automatically
                // UISetting is used in Project(Editortime), not Runtime
#if UNITY_EDITOR
                if (_instance == null)
                {
                    // AssetDatabase is in UnityEditor and it can manage Unity Project Assets
                    // use AssetDatabase to create a meta file immediately
                    if (!AssetDatabase.IsValidFolder(SettingFileDirectory))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }
                    // unexpected error
                    _instance = AssetDatabase.LoadAssetAtPath<XInput>(SettingFilePath);
                    // if file doesnt exist in SettingFilePath
                    if (_instance == null)
                    {
                        _instance = CreateInstance<XInput>();
                        AssetDatabase.CreateAsset(_instance, SettingFilePath);
                        AssetDatabase.ImportAsset(SettingFilePath);
                        _instance.PlayerInputSettings = new List<InputSetting>();
                        _instance.PresentIndex = 0;
                    }
                }
#endif

                return _instance;
            }
        }

        /// <summary>
        /// List of PlayerInputSetting
        /// </summary>
        public List<InputSetting> PlayerInputSettings;

        /// <summary>
        /// index of Present PlayerInputSetting
        /// </summary>
        public int PresentIndex;

        /// <summary> return PlayerInputSettings[index] </summary>
        public InputSetting this[int index]
        {
            get
            {
                if (index < 0 || index >= PlayerInputSettings.Count) { return null; }
                else { return PlayerInputSettings[index]; }
            }
        }

        /// <summary>
        /// set PresentIndex to {index}
        /// </summary>
        /// <param name="index"></param>
        public void SetInputSetting(int index)
        {
            PresentIndex = index;
            this[index].InputUnLockAll();
        }

        /// <summary>
        /// set PresentIndex to index of InputSetting with name {name}
        /// </summary>
        /// <param name="name"></param>
        public void SetInputSetting(string name)
        {
            bool isValid = false;
            for (int i = 0; i < PlayerInputSettings.Count; i++)
            {
                if (PlayerInputSettings[i].InputSettingName.Equals(name))
                {
                    PresentIndex = i;
                    isValid = true;
                    break;
                }
            }
            if (isValid) { this[PresentIndex].InputUnLockAll(); }
            else { Debug.LogError($"Input Setting with name {name} doesn't exist."); }
        }

        private KeyboardInput GetKey(string name)
        {
            return this[PresentIndex].GetInput<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting);
        }

        private MouseInput GetMouse(string name)
        {
            return this[PresentIndex].GetInput<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting);
        }

        private KeyboardInputGroup GetKeyGroup(string name)
        {
            return this[PresentIndex].GetInputGroup<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting);
        }

        private MouseInputGroup GetMouseGroup(string name)
        {
            return this[PresentIndex].GetInputGroup<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting);
        }

        /// <summary>
        /// Change KeyInputName of Present InputSetting's KeyInput with KeyInput name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="input"></param>
        public void ChangeKeyInput(string name, KeyboardInputName input)
        {
            if (GetKey(name) != null) { GetKey(name).InputKeyName = input; }
        }

        /// <summary>
        /// Change KeyInputName of Present InputSetting's KeyInput with KeyInput name and KeyInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        public void ChangeKeyInput(string group, string key, KeyboardInputName input)
        {
            if (GetKeyGroup(group) != null && GetKeyGroup(group).GetInput(key) != null) { GetKeyGroup(group).GetInput(key).InputKeyName = input; }
        }

        /// <summary>
        /// Change MouseInputName of Present InputSetting's MouseInput with MouseInput name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="input"></param>
        public void ChangeMouseInput(string name, MouseInputName input)
        {
            if (GetMouse(name) != null) { GetMouse(name).InputMouseName = input; }
        }

        /// <summary>
        /// Change MouseInputName of Present InputSetting's MouseInput with MouseInput name and MouseInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="mouse"></param>
        /// <param name="input"></param>
        public void ChangeMouseInput(string group, string mouse, MouseInputName input)
        {
            if (GetMouseGroup(group) != null && GetMouseGroup(group).GetInput(mouse) != null) { GetMouseGroup(group).GetInput(mouse).InputMouseName = input; }
        }

        /// <summary>
        /// return IsInput of KeyInput with KeyInput name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool KeyInput(string name)
        {
            if (GetKey(name) == null) return false;
            return GetKey(name).IsInput;
        }

        /// <summary>
        /// return IsInput of KeyInput with KeyInput name and KeyInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyInput(string group, string key)
        {
            if (GetKeyGroup(group) == null || GetKeyGroup(group).GetInput(key) == null) return false;
            return GetKeyGroup(group).GetInput(key).IsInput;
        }

        /// <summary>
        /// return IsInput of MouseInput with MouseInput name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool MouseInput(string name)
        {
            if (GetMouse(name) == null) return false;
            return GetMouse(name).IsInput;
        }

        /// <summary>
        /// return IsInput of MouseInput with MouseInput name and MouseInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public bool MouseInput(string group, string mouse)
        {
            if (GetMouseGroup(group) == null || GetMouseGroup(group).GetInput(mouse) == null) return false;
            return GetMouseGroup(group).GetInput(mouse).IsInput;
        }

        /// <summary>
        /// lock all PlayerInputs of Present InputSetting
        /// </summary>
        public void InputLockAll()
        {
            this[PresentIndex].InputLockAll();
        }

        /// <summary>
        /// lock all KeyboardInputs of Present InputSetting
        /// <para>if {name} is not null, lock all KeyboardInputs of the KeyboardInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void KeyInputLockAll(string name = null)
        {
            if (name == null)
            {
                this[PresentIndex].InputLockAll<KeyboardInputGroup, KeyboardInput>(this[PresentIndex].KeyboardInputSetting);
            }
            else
            {
                this[PresentIndex].InputLockAll<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting);
            }
        }

        /// <summary>
        /// lock all MouseInputs of Present InputSetting
        /// <para>if {name} is not null, lock all MouseInputs of the MouseInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void MouseInputLockAll(string name = null)
        {
            if (name == null)
            {
                this[PresentIndex].InputLockAll<MouseInputGroup, MouseInput>(this[PresentIndex].MouseInputSetting);
            }
            else
            {
                this[PresentIndex].InputLockAll<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting);
            }
        }

        /// <summary>
        /// unlock all PlayerInputs of Present InputSetting
        /// </summary>
        public void InputUnLockAll()
        {
            this[PresentIndex].InputUnLockAll();
        }

        /// <summary>
        /// unlock all KeyboardInputs of Present InputSetting
        /// <para>if {name} is not null, unlock all KeyboardInputs of the KeyboardInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void KeyInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                this[PresentIndex].InputUnLockAll<KeyboardInputGroup, KeyboardInput>(this[PresentIndex].KeyboardInputSetting);
            }
            else
            {
                this[PresentIndex].InputUnLockAll<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting);
            }
        }

        /// <summary>
        /// unlock all MouseInputs of Present InputSetting
        /// <para>if {name} is not null, unlock all MouseInputs of the MouseInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void MouseInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                this[PresentIndex].InputUnLockAll<MouseInputGroup, MouseInput>(this[PresentIndex].MouseInputSetting);
            }
            else
            {
                this[PresentIndex].InputUnLockAll<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting);
            }
        }

        /// <summary>
        /// unlock only KeyboardInput with KeyboardInputGroup name and KeyboardInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void KeyInputUnLockOnly(string groupName, string inputName)
        {
            this[PresentIndex].InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, this[PresentIndex].KeyboardInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, unlock all KeyboardInputs with KeyboardInputGroup name
        /// <para>else, unlock only KeyboardInput with KeyboardInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void KeyInputUnLockOnly(string name, bool isGroupName = true)
        {
            this[PresentIndex].InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting, isGroupName);
        }

        /// <summary>
        /// unlock only MouseInput with MouseInputGroup name and MouseInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void MouseInputUnLockOnly(string groupName, string inputName)
        {
            this[PresentIndex].InputUnLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, this[PresentIndex].MouseInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, unlock all MouseInputs with MouseInputGroup name
        /// <para>else, unlock only MouseInput with MouseInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void MouseInputUnLockOnly(string name, bool isGroupName = true)
        {
            this[PresentIndex].InputUnLockOnly<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting, isGroupName);
        }

        /// <summary>
        /// lock only KeyboardInput with KeyboardInputGroup name and KeyboardInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void KeyInputLockOnly(string groupName, string inputName)
        {
            this[PresentIndex].InputLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, this[PresentIndex].KeyboardInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, lock all KeyboardInputs with KeyboardInputGroup name
        /// <para>else, lock only KeyboardInput with KeyboardInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void KeyInputLockOnly(string name, bool isGroupName = true)
        {
            this[PresentIndex].InputLockOnly<KeyboardInputGroup, KeyboardInput>(name, this[PresentIndex].KeyboardInputSetting, isGroupName);
        }

        /// <summary>
        /// lock only MouseInput with MouseInputGroup name and MouseInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void MouseInputLockOnly(string groupName, string inputName)
        {
            this[PresentIndex].InputLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, this[PresentIndex].MouseInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, lock all MouseInputs with MouseInputGroup name
        /// <para>else, lock only MouseInput with MouseInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void MouseInputLockOnly(string name, bool isGroupName = true)
        {
            this[PresentIndex].InputLockOnly<MouseInputGroup, MouseInput>(name, this[PresentIndex].MouseInputSetting, isGroupName);
        }
    }
}