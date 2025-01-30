using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using StarterAssets;

public class RespawnBehaviour : MonoBehaviour
{
    public GameObject spawnPoint = null;
    private Vector3 respawnPoint;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public float fallThreshold = -10f;

    void Start()
    {
        if (spawnPoint != null) {
            SetRespawnPoint(spawnPoint);
            Respawn();
        } else {
            var spawns = GameObject.FindGameObjectsWithTag("Spawn");
            if (spawns.Length > 0)
            {
                var id = Random.Range(0, spawns.Length);
                SetRespawnPoint(spawns[id]);
                Respawn();
            }
        }
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    public void SetRespawnPoint(GameObject spawnPoint)
    {
        respawnPoint = spawnPoint.transform.position;
    }


    bool shouldTeleport = false;
    Vector3 teleportPoint;

    void LateUpdate()
    {
        if (shouldTeleport) {
            TeleportTo(teleportPoint);
            shouldTeleport = false;
        }
    }

    public void Respawn()
    {
        teleportPoint = respawnPoint;
        shouldTeleport = true;
    }

    public void Teleport(GameObject target) {
        TeleportTo(target.transform.position);
    }

    public void TeleportTo(Vector3 targetPosition)
    {
        var characterController = GetComponent<CharacterController>();
        if (characterController != null) {
            var currentCameraPosition = Vector3.zero;
            if (cinemachineVirtualCamera != null) {
                currentCameraPosition = cinemachineVirtualCamera.m_Follow.transform.position;
            }

            characterController.enabled = false;
            transform.position = targetPosition;
            characterController.enabled = true;
            characterController.Move(Vector3.zero);             

            if (cinemachineVirtualCamera != null) {
                cinemachineVirtualCamera.OnTargetObjectWarped(cinemachineVirtualCamera.m_Follow, cinemachineVirtualCamera.m_Follow.transform.position - currentCameraPosition);
            }
        }
    }

    System.Collections.IEnumerator Death() {
        //Respawn();
        GetComponent<Animator>().SetTrigger("Dead");
        GetComponent<ThirdPersonController>().SetControlEnabled(false);
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().SetTrigger("Respawn");
        GetComponent<ThirdPersonController>().SetControlEnabled(true);
        Respawn();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Spawn")) {
            SetRespawnPoint(hit.gameObject);
        }
        if (hit.collider.CompareTag("Lava")) {
            StartCoroutine(Death());
        }
    }
}
