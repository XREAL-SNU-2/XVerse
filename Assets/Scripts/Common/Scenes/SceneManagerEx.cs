using UnityEngine;
using UnityEngine.SceneManagement;
using Xverse.Scene;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene
    { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    private string GetSceneName(Xverse.Scene.Scene type)
    {
        string name = System.Enum.GetName(typeof(Xverse.Scene.Scene), type);
        return name;
    }

    public void LoadScene(Xverse.Scene.Scene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    public void LoadPersonal() // erase later
    {
        LoadScene(Xverse.Scene.Scene.PersonalWorld);
    }

    public void LoadOpen() // erase later
    {
        LoadScene(Xverse.Scene.Scene.OpenSpace);
    }
}