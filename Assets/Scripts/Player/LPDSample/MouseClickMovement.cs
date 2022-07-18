using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickMovement : MonoBehaviour
{
    protected MovementFX _movementFX;
    protected Vector3 _targetPos;
    protected bool _isMoving;

    // Movement settings, can be fetched from a 'Datasource'
    protected const float _distanceCheck = 0.05f;
    protected float _walkSpeed = 2.0f;
    protected float _quickMoveDist = 4.0f;
    protected float _quickMoveDuration = 0.2f; 

    // Start is called before the first frame update
    void Start()
    {
        _movementFX = GetComponent<MovementFX>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving) return;
        if(Vector3.Magnitude(transform.position - _targetPos) < _distanceCheck)
        {
            // arrived. cancel move.
            _movementFX.WalkAction.EndAction();
            _isMoving = false;
        }
        else
        {
            // should move towards target
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _walkSpeed);
            
        }
    }


    public void ToCursorMove(Vector2 screenPos) {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out hit, 500.0f, LayerMask.GetMask("Ground")))
        {
            _targetPos = hit.point;
            _isMoving = true;

            transform.LookAt(_targetPos, Vector3.up);
            _movementFX.WalkAction.BeginAction();
        }
    }

    public void QuickMove()
    {
        StartCoroutine(QuickMove(_quickMoveDuration, _quickMoveDist));
    }


    protected IEnumerator QuickMove(float time, float dist)
    {
        float t = 0.0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + transform.TransformDirection(Vector3.forward) * dist;
        while (t < time)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t / time * (2 - t / time));
            t += Time.deltaTime;
            yield return null;
        }
    }


}
