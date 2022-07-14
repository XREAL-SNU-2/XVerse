using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xverse.Scene;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Xverse.Scene.Scene.Login;
    }
    public override void Clear()
    {

    }
}