using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xverse.Scene;

public class PersonalWorldScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Xverse.Scene.Scene.PersonalWorld;
        //Debug.Log(SceneManagerEx.Instance.CurrentScene.name);
        //Debug.Log(SceneManagerEx.Instance.PrevScene.ToString());
    }
    public override void Clear()
    {

    }
}
