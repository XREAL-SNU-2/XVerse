using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using XVerse.Player.Input;

namespace XVerse.Player.Control
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(NetworkTransform))]
    public class PlayerController : MonoBehaviour //NetworkBehaviour
    {
        private const float SPEED_MIN = 2f;
        private const float SPEED_MAX = 10f;
        private const float JUMP_DELAY = 0.3f;

        public enum STATE
        {
            NONE = -1,
            MOVE = 0,
            JUMP,
            FLOAT,
            STOP,
        }

        [Header("PlayerBody")]


        [Header("Movement Settings")]
        [Range(SPEED_MIN, SPEED_MAX)]
        public float WalkSpeed = 4f;
        [Range(SPEED_MIN, SPEED_MAX)]
        public float RunSpeed = 7f;
        public float JumpForce = 3f;
        public float TurnSensitivity = 5f;

        [Header("Diagnostics")]
        public STATE State = STATE.NONE;
        public bool IsGrounded = true;
        public bool CanJump = true;
        public float MoveSpeed;
        public int Horizontal;
        public int Vertical;

        private bool isEnabled = false;
        private Vector3 velocity;

        private void OnValidate()
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<NetworkTransform>().clientAuthority = true;
        }

        /*
        public override void OnStartLocalPlayer()
        {
            isEnabled = true;
        }
        */

        private void Start()
        {
            reset();
        }

        private void Update()
        {
            //if (!isLocalPlayer || isEnabled) return;

            if (IsGrounded)
            {
                if (XInput.Instance.KeyInput("Movement", "Front") || XInput.Instance.KeyInput("Movement", "Left") || XInput.Instance.KeyInput("Movement", "Right") || XInput.Instance.KeyInput("Movement", "Back"))
                {
                    State = STATE.MOVE;
                    if (XInput.Instance.KeyInput("Movement", "Run")) { MoveSpeed = RunSpeed; }
                    else if (MoveSpeed != WalkSpeed) { MoveSpeed = WalkSpeed; }
                }
                else if (velocity.magnitude < 0.1f) { State = STATE.STOP; }
                if (CanJump && XInput.Instance.KeyInput("Movement", "Jump"))
                {
                    State = STATE.JUMP;
                    GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    XInput.Instance.InputLockAll();
                    CanJump = false;
                    IsGrounded = false;
                }
            }
            else
            {
                State = STATE.FLOAT;
            }

        }

        private void FixedUpdate()
        {
            //if (!isLocalPlayer || isEnabled) return;

            if (State == STATE.MOVE)
            {
                if (XInput.Instance.KeyInput("Movement", "Front")) { Vertical = 1; }
                if (XInput.Instance.KeyInput("Movement", "Back")) { Vertical = -1; }
                if (XInput.Instance.KeyInput("Movement", "Front") && XInput.Instance.KeyInput("Movement", "Back")) { Vertical = 0; }
                if (!XInput.Instance.KeyInput("Movement", "Front") && !XInput.Instance.KeyInput("Movement", "Back")) { Vertical = 0; }

                if (XInput.Instance.KeyInput("Movement", "Left")) { Horizontal = 1; }
                if (XInput.Instance.KeyInput("Movement", "Right")) { Horizontal = -1; }
                if (XInput.Instance.KeyInput("Movement", "Left") && XInput.Instance.KeyInput("Movement", "Right")) { Horizontal = 0; }
                if (!XInput.Instance.KeyInput("Movement", "Left") && !XInput.Instance.KeyInput("Movement", "Right")) { Horizontal = 0; }

                Vector3 dir = new Vector3(Horizontal, 0f, Vertical);
                transform.position += dir * MoveSpeed * Time.fixedDeltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), TurnSensitivity * Time.fixedDeltaTime);
            }

            velocity = GetComponent<Rigidbody>().velocity;
        }

        private void reset()
        {
            StopAllCoroutines();
            State = STATE.NONE;
            IsGrounded = true;
            CanJump = true;
            MoveSpeed = WalkSpeed;
            XInput.Instance.SetInputSetting(0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGrounded = true;
                XInput.Instance.InputUnLockAll();
                if (!CanJump) { StartCoroutine(CanRunToTrue()); }
            }
        }

        private IEnumerator CanRunToTrue()
        {
            yield return new WaitForSeconds(JUMP_DELAY);
            CanJump = true;
        }

    }
}