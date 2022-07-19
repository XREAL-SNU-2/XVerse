using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xverse.Scene;

public class OpenSpaceScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Xverse.Scene.Scene.OpenSpace;
        //Debug.Log(SceneManagerEx.Instance.CurrentScene.name);
        //Debug.Log(SceneManagerEx.Instance.PrevScene.ToString());
    }
    public override void Clear()
    {

    }
}
