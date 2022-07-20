using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

// Client character does input handling
// and send commands on server on inputs.
public class ClientCharacter : NetworkBehaviour
{
    private NetState _netState;
    private ClientCharacterVisualization _visualization; 
    private InputAction _movePosition;
    private InputAction _moveAction;
    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        _movePosition = playerInput.actions["LocoTarget"];
        _moveAction = playerInput.actions["MoveTarget"];


        _netState = GetComponent<NetState>();
        _netState.MovementChangeReceivedClient += OnMovementStateChanged;
    }

    public void OnMovementStateChanged(MovementState state)
    {
        Debug.Log($"[ClientCharacter] changed state:{state}, local state{_netState.MovementState}");
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (!_netState.IsLocalPlayer) return;

        //logic: Client's Input -> the vector3 input to the server's movement provider
        if (_moveAction.triggered)
        {
            Vector2 screenPos = _movePosition.ReadValue<Vector2>();
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out hit, 500.0f, LayerMask.GetMask("Ground")))
            {
                _netState.MovementRequestServerRpc(hit.point);
            }
        }

    }
}
