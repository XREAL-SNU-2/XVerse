using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

using XVerse.Player.Input;

namespace XVerse.Player.Control
{
    // Below is script for Single Play version without Mirror Network.
    public class PlayerCameraControllerV1 : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(20f, 5f);
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }
        private CinemachineTransposer transposer;



        void Awake()
        {
            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        private void OnEnable() => Controls.Enable();
        private void OnDisable() => Controls.Disable();

        private void Look(Vector2 lookAxis)
        {
            float deltaTime = Time.deltaTime;

            float followOffset = Mathf.Clamp(
                transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * deltaTime),
                maxFollowOffset.x,
                maxFollowOffset.y);

            transposer.m_FollowOffset.y = followOffset;

            playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
        }
    }

    // Below is script for Mirror Network version.
    /*
    public class PlayerCameraControllerV1 : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(20f, 5f);
        [SerializeField] private Transform playerTransform= null;
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }
        private CinemachineTransposer transposer;

        public override void OnStartAuthority()
        {
            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            virtualCamera.gameObject.SetActive(true);
            enabled = true;

            Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        private void OnDisable() => Controls.Disable();

        private void Look(Vector2 lookAxis)
        {
            float deltaTime = Time.deltaTime;

            float followOffset = Mathf.Clamp(
                transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * deltaTime),
                maxFollowOffset.x,
                maxFollowOffset.y);

            transposer.m_FollowOffset.y = followOffset;

            playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
        }
    }
    */
}
