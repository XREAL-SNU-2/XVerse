using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetState : NetworkBehaviour
{

    // current movement state of the character
    // this is NOT a syncVar!
    public MovementState MovementState;
    
    public override void OnStartLocalPlayer()
    {
        Debug.Log("callback: started player");
    }


    /// <summary>
    /// Client should tell the server where it wants to go.
    /// Use the Vector3 variable any way you want 
    /// - send just the direction to the server and have the server calculates the destination..
    /// - send the target position of the character..
    /// </summary>
    public event Action<Vector3> MovementRequestReceived;

    [Command]
    public void MovementRequestServerRpc(Vector3 targetPos)
    {
        MovementRequestReceived?.Invoke(targetPos);
    }

    /// <summary>
    /// The client who wishes to play an action
    /// should request the server, which will make decisions and then if the action can be played,
    /// broadcast all clients to play that action
    /// </summary>
    public event Action<GameActionRequest> ActionRequestReceived;

    [Command]
    public void ActionRequestServerRpc(GameActionRequest request)
    {
        ActionRequestReceived?.Invoke(request);
    }



    /// <summary>
    /// Server to Client RPC that broadcasts this action play to all clients.
    /// The action itself will, in its course of update in the server, call this RPC to all clients.
    /// The ClientVisualization component should subscribe to this!
    /// </summary>
    public event Action<GameActionRequest> ActionPlayReceivedClient;

    [ClientRpc]
    public void RecvDoActionClientRPC(GameActionRequest data)
    {
        ActionPlayReceivedClient?.Invoke(data);
    }

    public event Action<MovementState> MovementChangeReceivedClient;
    /// <summary>
    /// movement requires some events to be broadcasted to the client,
    /// besides the everyday transform and rotation stuff - these are taken care of by special Mirror components.
    /// </summary>
    /// <param name="state"> the new updated MovementState </param>
    [ClientRpc]
    public void RecvDoMovementClientRPC(uint state)
    {
        MovementChangeReceivedClient?.Invoke((MovementState)state);
    }

    public bool IsMoving
    {
        get => MovementState == MovementState.Walking;
    }

    public bool IsLocalPlayer
    {
        get => isLocalPlayer;
    }
}

// ForcedMovement means that the character is being 'animated', not controlled by player.
// for example, climbing a ladder.
public enum MovementState : uint
{
    Idle,
    Walking,
    ForcedMovement
}