using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Xverse.Scene
{ 
    public enum Scene
    {
    Default,
    Login,
    Loading,
    PersonalWorld,
    OpenSpace,
    }
    public abstract class BaseScene : MonoBehaviour
    {
        public Scene SceneType { get; protected set; } = Scene.Default;
        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public abstract void Clear();
        public void ToLoadingScene()
        {
            SceneManagerEx.Instance.LoadScene(Scene.Loading);
        }
    }
}
