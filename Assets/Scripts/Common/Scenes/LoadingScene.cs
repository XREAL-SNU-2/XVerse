using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xverse.Scene;

public class LoadingScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Xverse.Scene.Scene.Loading;
    }
    public override void Clear()
    {

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
