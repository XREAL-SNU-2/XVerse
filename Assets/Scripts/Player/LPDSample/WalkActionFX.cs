using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkActionFX : GameActionFX
{

    private Animator _anim;
    public WalkActionFX(ref GameActionRequest req, ClientCharacterVisualization character): base(ref req, character)
    {
        // cache everything you need often.
        // remember, even if you don't cache, you can still access visual components
        // via _characterVisual
        _anim = character.Anim;
    }

    public override void BeginAction()
    {
        throw new System.NotImplementedException();
    }

    public override void EndAction()
    {
        throw new System.NotImplementedException();
    }

    
}
