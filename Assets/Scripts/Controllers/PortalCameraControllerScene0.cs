using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraControllerScene0 : MonoBehaviour
{
    public Camera PlayerCamera;

    private Camera _portalCamera;

    public GameObject MasterPortal;

    public GameObject SlavePortal;

    private Vector3 _masterToSlavePortalOffset;

    private void Awake()
    {
        _portalCamera = GetComponent<Camera>();


        if (_portalCamera.targetTexture != null)
        {
            _portalCamera.targetTexture.Release();
            _portalCamera.targetTexture = null;
        }


        _masterToSlavePortalOffset = SlavePortal.transform.position - MasterPortal.transform.position;

        transform.position = _masterToSlavePortalOffset + PlayerCamera.transform.position;
        transform.rotation = PlayerCamera.transform.rotation;
    }

    private void Start()
    {
    }

    void LateUpdate()
    {
        transform.position = _masterToSlavePortalOffset + PlayerCamera.transform.position;
        transform.rotation = PlayerCamera.transform.rotation;
    }
}
