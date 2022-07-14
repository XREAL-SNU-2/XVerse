using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xverse.Scene;

public class SceneManagerEx
{
    private static SceneManagerEx instance = null;
    private static readonly object padlock = new object();
    public static SceneManagerEx Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance is null)
                    instance = new SceneManagerEx();
            }
            return instance;
        }
    }
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Xverse.Scene.Scene PrevScene = Xverse.Scene.Scene.Default;
    string GetSceneName(Xverse.Scene.Scene type)
    {
        string name = System.Enum.GetName(typeof(Xverse.Scene.Scene), type);
        return name;
    }

    public void LoadScene(Xverse.Scene.Scene type)
    {
        PrevScene = CurrentScene.SceneType;
        SceneManager.LoadScene(GetSceneName(type));
    }
}
