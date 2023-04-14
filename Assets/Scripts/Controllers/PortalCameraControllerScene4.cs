using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraControllerScene4 : MonoBehaviour
{
    public GameObject Player;

    private PlayerControllerScene4 _playerController;

    private Camera _portalCamera;

    public GameObject RenderTargetPortal;

    //public GameObject SlavePortal;

    public int RenderQueueIndex = 1;

    //private Vector3 _masterToSlavePortalOffset;

    private void Awake()
    {
        _playerController = Player.GetComponent<PlayerControllerScene4>();

        _portalCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        _portalCamera.CopyFrom(_playerController.Camera);

        _portalCamera.depth = _playerController.Camera.depth - RenderQueueIndex;

        if (_portalCamera.targetTexture != null)
        {
            _portalCamera.targetTexture.Release();
            _portalCamera.targetTexture = null;
        }

        var masterPortalRenderer = RenderTargetPortal.GetComponent<Renderer>();

        masterPortalRenderer.material = new Material(Resources.Load<Shader>("Portal Shader"));

        masterPortalRenderer.material.name = RenderTargetPortal.name + " Material";

        masterPortalRenderer.material.mainTexture = _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        masterPortalRenderer.material.mainTexture.name = RenderTargetPortal.name + " Render Texture";

        masterPortalRenderer.material.renderQueue = 2000 + RenderQueueIndex;

        //_masterToSlavePortalOffset = MasterPortal.transform.position - SlavePortal.transform.position;
    }

    void LateUpdate()
    {
        transform.localPosition = _playerController.CameraPosition;
        transform.localRotation = _playerController.CameraLocalRotation;
    }
}
