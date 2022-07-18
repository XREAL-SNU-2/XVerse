using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : GameAction
{
    // any special effects or animation or whatsoever that this action uses.
    // dependency provided by the FXComponent of the character, which this action need not know.
    public Animator Anim;
    protected bool _playingWalkAnim = false;


    // implementation
    public override void BeginAction()
    {
        _playingWalkAnim = true;
    }

    public override void EndAction()
    {
        _playingWalkAnim = false;
        Anim.SetBool("Walk", false);
    }

    
    public void Update()
    {
        if (_playingWalkAnim)
        {
            Anim.SetBool("Walk", true);
        }
    }
    
}
