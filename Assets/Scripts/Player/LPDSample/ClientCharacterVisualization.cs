using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCharacterVisualization : MonoBehaviour
{
    // any visual components, animator, sound player, particles,, should be set here.
    // you must make them accessible(i.e. public)

    public Animator Anim;

    // current actionFX being played.
    /* as of now, we assume ALL ACTION FX are BLOCKING.
    *  which means at any given time a character plays exactly one FX
    *  and if any other action in requested, it will either be ignored
    *  or stop the current action play immediately.
    */
    public GameActionFX CurrentActionFX;

    private void Start()
    {
        Anim = GetComponent<Animator>();

        // this in fact should go in OnSpawn callback.
        NetState netState = GetComponentInParent<NetState>();
        netState.ActionPlayReceivedClient += OnGameActionPlay;
        netState.MovementChangeReceivedClient += OnMovementChange;
    }

    // CALLBACKS
    private void OnMovementChange(MovementState state)
    {
        Debug.Log($"[ClientVis]: state = {state}");
        if (state == MovementState.Walking) Anim.SetBool("Walk", true);
        else if (state == MovementState.Idle) Anim.SetBool("Walk", false);
    }

    // CALLBACKS
    private void OnGameActionPlay(GameActionRequest data)
    {
        CurrentActionFX = GameActionFX.CreateActionFXFromData(ref data, this);
        CurrentActionFX.BeginAction();
    }

    private void Update()
    {
        if (CurrentActionFX != null && !CurrentActionFX.UpdateAction())
        {
            CurrentActionFX.EndAction();
            CurrentActionFX = null;
        }
    }
}
