using UnityEngine;

namespace Xverse.Scene
{
    public enum Scene
    {
        Default,
        Login,
        PersonalWorld,
        OpenSpace,
    }

    public abstract class BaseScene : MonoBehaviour
    {
        public Scene SceneType { get; protected set; } = Scene.Default;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public abstract void Clear();
    }
}