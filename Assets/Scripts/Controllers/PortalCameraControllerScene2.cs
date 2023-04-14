using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraControllerScene2 : MonoBehaviour
{
    public Camera PlayerCamera;

    private PlayerActivePortalController _playerActivePortalController;

    private Camera _portalCamera;

    public GameObject RootPortal;

    public GameObject MasterPortal;

    public GameObject SlavePortal;

    public int RenderQueueIndex = 0;

    //private Vector3 _masterToSlavePortalOffset;

    private void Awake()
    {
        _playerActivePortalController = PlayerCamera.GetComponent<PlayerActivePortalController>();

        _portalCamera = GetComponent<Camera>();



        if (_portalCamera.targetTexture != null)
        {
            _portalCamera.targetTexture.Release();
            _portalCamera.targetTexture = null;
        }



        var masterPortalRenderer = MasterPortal.GetComponent<Renderer>();

        masterPortalRenderer.material = new Material(Resources.Load<Shader>("Portal Render Target Shader"));

        masterPortalRenderer.material.name = MasterPortal.name + " Material";

        masterPortalRenderer.material.mainTexture = _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        masterPortalRenderer.material.mainTexture.name = MasterPortal.name + " Render Texture";

        masterPortalRenderer.material.renderQueue = 2000 + RenderQueueIndex;


        transform.position = SlavePortal.transform.position + _playerActivePortalController.ActivePortalOffset();
        transform.rotation = PlayerCamera.transform.rotation;
    }

    private void Start()
    {
    }

    void LateUpdate()
    {
        transform.position = SlavePortal.transform.position + _playerActivePortalController.ActivePortalOffset();
        transform.rotation = PlayerCamera.transform.rotation;
    }
}
