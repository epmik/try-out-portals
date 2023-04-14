using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraControllerScene5 : CommonBehaviour
{
    //public GameObject Player;

    //private PlayerControllerScene5 _playerController;

    //private Camera _playerCamera;

    private Camera _portalCamera;

    public GameObject MasterPortal;

    public GameObject SlavePortal;

    public int RenderQueueIndex = 1;

    private Vector3 _masterToSlavePortalOffset;

    private void Awake()
    {
        //_playerController = Game.PlayerGameObject().GetComponent<PlayerControllerScene5>();

        //_playerCamera = Player.GetComponentInChildren<Camera>();

        _portalCamera = GetComponent<Camera>();

        _masterToSlavePortalOffset = SlavePortal.transform.position - MasterPortal.transform.position;
    }

    private void Start()
    {
        _portalCamera.CopyFrom(_portalCamera);

        _portalCamera.depth = _portalCamera.depth - RenderQueueIndex;

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

        transform.position = _masterToSlavePortalOffset;
    }

    void LateUpdate()
    {
        transform.position = Game.PlayerCamera().transform.position + _masterToSlavePortalOffset;
        transform.rotation = Quaternion.Euler(Game.PlayerCamera().transform.eulerAngles.x, Game.PlayerController().transform.eulerAngles.y, 0);
    }
}
