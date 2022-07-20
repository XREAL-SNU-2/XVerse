using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacter : NetworkBehaviour
{
    public ServerCharacterMovement MovementProvider;
    public NetState NetState;
    private GameAction CurrentAction;

    private void Start()
    {
        MovementProvider = GetComponent<ServerCharacterMovement>();
        NetState = GetComponent<NetState>();
        NetState.MovementRequestReceived += OnMovementRequest;
    }


    // Action Player
    public void PlayAction(GameAction action)
    {
        CurrentAction = action;
        action.BeginAction();
    }

    public void CancelCurrentAction()
    {
        CurrentAction.EndAction();
        CurrentAction = null;
    }

    private void Update()
    {
        if (CurrentAction != null)
        {
            if (!CurrentAction.UpdateAction())
            {
                CurrentAction.EndAction();
                CurrentAction = null;
            }
        }
        if (NetState.IsMoving)
        {
            if (!MovementProvider.UpdateMovement())
            {
                // stop walking
                if (NetState.MovementState == MovementState.Walking)
                {
                    Debug.Log("[ServerCharacter] end of walk detected, changing state to idle!");
                    // tell every client to stop
                    NetState.RecvDoMovementClientRPC((uint)MovementState.Idle);
                }
                NetState.MovementState = MovementState.Idle;

            }
        }
    }


    // this runs on the server only.
    private void OnMovementRequest(Vector3 targetPosition)
    {
        // call suitable methods in the MovementProvider..
        // we assume the mouse-click move(**move towards right mouse button click!!) is being used.
        Debug.Log($"[Server Character] received movement RPC {targetPosition}");
        MovementProvider.SetTarget(targetPosition);
        if (NetState.MovementState != MovementState.Walking)
        {
            // tell every client to start moving
            NetState.RecvDoMovementClientRPC((uint)MovementState.Walking);
        }
        NetState.MovementState = MovementState.Walking;

    }


}
