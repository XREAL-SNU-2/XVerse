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
        [SerializeField] private float maxCameraRadius = 4f;
        [SerializeField] private Vector2 cameraVelocity = new Vector2(20f, 5f);
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private CinemachineFreeLook virtualCamera = null;

        private void Awake()
        {
            virtualCamera.m_XAxis.m_MaxSpeed *= cameraVelocity.x;
            virtualCamera.m_YAxis.m_MaxSpeed *= cameraVelocity.y;
            virtualCamera.m_Orbits[0].m_Radius = maxCameraRadius;
            virtualCamera.m_Orbits[1].m_Radius = maxCameraRadius;
            virtualCamera.m_Orbits[2].m_Radius = maxCameraRadius;
            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
        }
    }

    // Below is script for Mirror Network version.
    /*
    public class PlayerCameraControllerV1 : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private float maxCameraRadius = 4f;
        [SerializeField] private Vector2 cameraVelocity = new Vector2(20f, 5f);
        [SerializeField] private Transform playerTransform= null;
        
        private CinemachineFreeLook virtualCamera;

        private void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);
            enabled = true;
            virtualCamera.m_XAxis.m_MaxSpeed *= cameraVelocity.x;
            virtualCamera.m_YAxis.m_MaxSpeed *= cameraVelocity.y;
            virtualCamera.m_Orbits[0].m_Radius = maxCameraRadius;
            virtualCamera.m_Orbits[1].m_Radius = maxCameraRadius;
            virtualCamera.m_Orbits[2].m_Radius = maxCameraRadius;
            virtualCamera.m_Follow = playerTransform;
            virtualCamera.m_LookAt = playerTransform;
        }
    }
    */
}
