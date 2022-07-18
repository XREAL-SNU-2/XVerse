using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementFX : MonoBehaviour
{
    private PlayerInput _playerInput;

    // input action (new input system vars)
    private InputAction _movePosition;
    private InputAction _moveAction;
    private InputAction _quickMoveAction;

    // this is the visual component, so it has the animator
    private Animator _anim;

    // it also holds various references to 'Providers' that holds the logic,
    // although only one at this time.
    public MouseClickMovement MovementProvider;

    // list of actions
    // I don't like putting these actions here...
    // It's here only at this early development stage, and will be moved..
    public WalkAction WalkAction;


    protected void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _quickMoveAction = _playerInput.actions["QuickMove"];
        _movePosition = _playerInput.actions["LocoTarget"];
        _moveAction = _playerInput.actions["MoveTarget"];

        _anim = GetComponentInChildren<Animator>();

        // initialize actions
        WalkAction.Anim = _anim;
    }



    protected void Update()
    {
        // the main input processing loop
        ProcessInput();
    }



    protected void ProcessInput()
    {

        // quick move. moves 3 meters forward in 0.1 seconds
        if (_quickMoveAction.triggered)
        {
            MovementProvider.QuickMove();
        }
        // click to set move target
        if (_moveAction.triggered)
        {
            MovementProvider.ToCursorMove(_movePosition.ReadValue<Vector2>());
        }
    }

}
