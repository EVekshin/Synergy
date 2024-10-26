// CameraSetup.cs
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform player;
    
    private void Start()
    {
        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;
        
        // Настройка параметров камеры
        var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_DeadZoneWidth = 0.1f;
        composer.m_DeadZoneHeight = 0.1f;
    }
}