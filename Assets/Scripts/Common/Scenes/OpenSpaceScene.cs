using Xverse.Scene;

public class OpenSpaceScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Xverse.Scene.Scene.OpenSpace;
    }

    public override void Clear()
    {
    }
}