using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : GameAction
{
    protected bool _playingWalkAnim = false;

    public WalkAction(ref GameActionRequest req, ServerCharacter parent): base(ref req, parent)
    {
        
    }

    // implementation
    public override void BeginAction()
    {
        _playingWalkAnim = true;
        // this causes all characters to play the action FX.
        _serverCharacter.NetState.RecvDoActionClientRPC(_requestData);
    }

    public override void EndAction()
    {
        _playingWalkAnim = false;
    }

    
    public override bool UpdateAction()
    {
        return true;
    }

}
