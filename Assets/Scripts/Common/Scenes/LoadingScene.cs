using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xverse.Scene;

public class LoadingScene : BaseScene
{
    public Button LoginButton;
    public Button OpenSpaceButton;
    public Button PersonalWorldButton;
    protected override void Init()
    {
        base.Init();

        SceneType = Xverse.Scene.Scene.Loading;
        if(SceneManagerEx.Instance.PrevScene.Equals(Scene.Login))
        {
            LoginButton.gameObject.SetActive(true);
            PersonalWorldButton.gameObject.SetActive(true);
        }
        if (SceneManagerEx.Instance.PrevScene.Equals(Scene.OpenSpace))
        {
            OpenSpaceButton.gameObject.SetActive(true);
        }
        if (SceneManagerEx.Instance.PrevScene.Equals(Scene.PersonalWorld))
        {
            PersonalWorldButton.gameObject.SetActive(true);
            OpenSpaceButton.gameObject.SetActive(true);
        }

    }
    public override void Clear()
    {

    }

    public void ToLogin()
    {
        SceneManagerEx.Instance.LoadScene(Xverse.Scene.Scene.Login);
    }
    public void ToOpenSpace()
    {
        SceneManagerEx.Instance.LoadScene(Xverse.Scene.Scene.OpenSpace);
    }
    public void ToPersonalWorld()
    {
        SceneManagerEx.Instance.LoadScene(Xverse.Scene.Scene.PersonalWorld);
    }
}
