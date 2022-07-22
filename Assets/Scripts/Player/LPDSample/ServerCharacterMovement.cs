using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterMovement : MonoBehaviour
{
    private Vector3 _targetPos;
    private float _distanceCheck = 0.05f;
    private float _walkSpeed = 1.00f;
    public void SetTarget(Vector3 targetPos)
    {
        _targetPos = targetPos;
    }

    // main movement loop
    public bool UpdateMovement()
    {
        if (Vector3.Magnitude(transform.position - _targetPos) < _distanceCheck)
        {
            // arrived. cancel move.
            Debug.Log("arrived! stop moving!");
            return false;
        }
        // should move towards target
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _walkSpeed);
        transform.LookAt(_targetPos, Vector3.up);
        return true;
    }

    /*protected IEnumerator QuickMove(float time, float dist)
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
    */
}
