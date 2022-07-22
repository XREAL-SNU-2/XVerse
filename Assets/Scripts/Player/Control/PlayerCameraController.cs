using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

using XVerse.Player.Input;

namespace XVerse.Player.Control
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Vector2 cameraDistance = new Vector2(2f, 10f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(5f, 5f);
        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private CinemachineFreeLook virtualCamera;

        private GameObject playerCamera;
        private float distance;
        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            enabled = true;
            virtualCamera.gameObject.SetActive(true);
            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
            virtualCamera.m_XAxis.m_MaxSpeed *= cameraVelocity.x;
            virtualCamera.m_YAxis.m_MaxSpeed *= cameraVelocity.y;
            virtualCamera.m_Orbits[0].m_Radius = cameraDistance.y;
            virtualCamera.m_Orbits[1].m_Radius = cameraDistance.y;
            virtualCamera.m_Orbits[2].m_Radius = cameraDistance.y;

            distance = virtualCamera.m_Orbits[0].m_Radius;

            Controls.Player.MouseScroll.performed += ctx => Zoom(ctx.ReadValue<float>());
        }

        private void Zoom(float scrollDelta)
        {
            distance = virtualCamera.m_Orbits[0].m_Radius;
            distance -= scrollDelta * zoomSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, cameraDistance.x, cameraDistance.y);

            for (int i = 0; i < 3; i++)
            {
                virtualCamera.m_Orbits[i].m_Radius = distance;
            }
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();

        [ClientCallback]
        private void OnDisable() => Controls.Disable();
    }


}
