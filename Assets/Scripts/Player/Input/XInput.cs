using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XPlayer.Input.InputSetting;
using XPlayer.Input.Keyboard;
using XPlayer.Input.Mouse;

#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace XPlayer.Input.InputManager
{
    public class XInput : ScriptableObject
    {
        private const string SettingFileDirectory = "Assets/Resources";
        private const string SettingFilePath = "Assets/Resources/XInput.asset";

        // lazy initialize singleton
        private static XInput _instance;
        public static XInput Instance
        {
            get
            {
                if (_instance != null)
                {
                    if(_instance.PlayerInputSettings == null)
                    {
                        _instance.PlayerInputSettings = new List<InputSetting.InputSetting>();
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
                        /*_instance.PlayerInputSetting = ScriptableObject.CreateInstance<InputSetting.InputSetting>();
                        AssetDatabase.AddObjectToAsset(_instance.PlayerInputSetting, SettingFilePath);*/
                        AssetDatabase.CreateAsset(_instance, SettingFilePath);
                        AssetDatabase.ImportAsset(SettingFilePath);
                        _instance.PlayerInputSettings = new List<InputSetting.InputSetting>();
                        _instance.PresentIndex = 0;
                    }
                }
#endif

                return _instance;
            }
        }

        public List<InputSetting.InputSetting> PlayerInputSettings;
        public int PresentIndex;
        public static string PlayerInputSettings_Prop_Name => nameof(PlayerInputSettings);
        public static string PresentIndex_Prop_Name => nameof(PresentIndex);

        public InputSetting.InputSetting this[int index]
        {
            get
            {
                if(index < 0 || index >= PlayerInputSettings.Count) { return null; }
                else { return PlayerInputSettings[index]; }
            }
        }
        
        public void SetInputSetting(int index)
        {
            PresentIndex = index;
            this[index].InputUnLockAll();
        }

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
            if(isValid) { this[PresentIndex].InputUnLockAll(); }
        }

        /*
        public KeyboardInput GetKey(string name)
        {
            return PlayerInputSetting.GetInput<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting);
        }

        public MouseInput GetMouse(string name)
        {
            return PlayerInputSetting.GetInput<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting);
        }

        public KeyboardInputGroup GetKeyGroup(string name)
        {
            return PlayerInputSetting.GetInputGroup<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting);
        }

        public MouseInputGroup GetMouseGroup(string name)
        {
            return PlayerInputSetting.GetInputGroup<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting);
        }

        public bool KeyInput(string name)
        {
            if (GetKey(name) == null) return false;
            return GetKey(name).IsInput;
        }

        public bool KeyInput(string group, string key)
        {
            if (GetKeyGroup(group) == null || GetKeyGroup(group).GetInput(name) == null) return false;
            return GetKeyGroup(group).GetInput(key).IsInput;
        }

        public bool MouseInput(string name)
        {
            if (GetMouse(name) == null) return false;
            return GetMouse(name).IsInput;
        }

        public bool MouseInput(string group, string mouse)
        {
            if (GetMouseGroup(group) == null || GetMouseGroup(group).GetInput(name) == null) return false;
            return GetMouseGroup(group).GetInput(mouse).IsInput;
        }

        public void InputLockAll()
        {
            PlayerInputSetting.InputLockAll();
        }

        public void KeyInputLockAll(string name = null)
        {
            if (name == null)
            {
                PlayerInputSetting.InputLockAll<KeyboardInputGroup, KeyboardInput>(PlayerInputSetting.KeyboardInputSetting);
            }
            else
            {
                PlayerInputSetting.InputLockAll<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting);
            }
        }

        public void MouseInputLockAll(string name = null)
        {
            if (name == null)
            {
                PlayerInputSetting.InputLockAll<MouseInputGroup, MouseInput>(PlayerInputSetting.MouseInputSetting);
            }
            else
            {
                PlayerInputSetting.InputLockAll<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting);
            }
        }

        public void InputUnLockAll()
        {
            PlayerInputSetting.InputUnLockAll();
        }

        public void KeyInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                PlayerInputSetting.InputUnLockAll<KeyboardInputGroup, KeyboardInput>(PlayerInputSetting.KeyboardInputSetting);
            }
            else
            {
                PlayerInputSetting.InputUnLockAll<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting);
            }
        }

        public void MouseInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                PlayerInputSetting.InputUnLockAll<MouseInputGroup, MouseInput>(PlayerInputSetting.MouseInputSetting);
            }
            else
            {
                PlayerInputSetting.InputUnLockAll<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting);
            }
        }

        public void KeyInputUnLockOnly(string groupName, string inputName)
        {
            PlayerInputSetting.InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, PlayerInputSetting.KeyboardInputSetting);
        }

        public void KeyInputUnLockOnly(string name, bool isGroupName = true)
        {
            PlayerInputSetting.InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting, isGroupName);
        }

        public void MouseInputUnLockOnly(string groupName, string inputName)
        {
            PlayerInputSetting.InputUnLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, PlayerInputSetting.MouseInputSetting);
        }

        public void MouseInputUnLockOnly(string name, bool isGroupName = true)
        {
            PlayerInputSetting.InputUnLockOnly<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting, isGroupName);
        }
        public void KeyInputLockOnly(string groupName, string inputName)
        {
            PlayerInputSetting.InputLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, PlayerInputSetting.KeyboardInputSetting);
        }

        public void KeyInputLockOnly(string name, bool isGroupName = true)
        {
            PlayerInputSetting.InputLockOnly<KeyboardInputGroup, KeyboardInput>(name, PlayerInputSetting.KeyboardInputSetting, isGroupName);
        }

        public void MouseInputLockOnly(string groupName, string inputName)
        {
            PlayerInputSetting.InputLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, PlayerInputSetting.MouseInputSetting);
        }

        public void MouseInputLockOnly(string name, bool isGroupName = true)
        {
            PlayerInputSetting.InputLockOnly<MouseInputGroup, MouseInput>(name, PlayerInputSetting.MouseInputSetting, isGroupName);
        }
        */
    }
}