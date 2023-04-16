using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PortalCameraControllerScene6 : CommonBehaviour
//{
//    //public GameObject Player;

//    //private PlayerControllerScene5 _playerController;

//    //private Camera _playerCamera;

//    //private Camera _portalCamera;

//    public GameObject RenderSource;

//    public GameObject RenderTarget;

//    //public GameObject SlavePortal;

//    public int RenderQueueIndex = 1;

//    //private Vector3 _masterToSlavePortalOffset;

//    private void Awake()
//    {
//        //_playerController = Game.PlayerGameObject().GetComponent<PlayerControllerScene5>();

//        //_playerCamera = Player.GetComponentInChildren<Camera>();

//        //_portalCamera = GetComponent<Camera>();

//        //_masterToSlavePortalOffset = SlavePortal.transform.position - MasterPortal.transform.position;
//    }

//    private void Start()
//    {
//        //_portalCamera.CopyFrom(_portalCamera);

//        //_portalCamera.depth = _portalCamera.depth - RenderQueueIndex;

//        //if (_portalCamera.targetTexture != null)
//        //{
//        //    _portalCamera.targetTexture.Release();
//        //    _portalCamera.targetTexture = null;
//        //}

//        //var masterPortalRenderer = RenderTarget.GetComponent<Renderer>();

//        //masterPortalRenderer.material = new Material(Resources.Load<Shader>("Portal Render Target Shader"));

//        //masterPortalRenderer.material.name = RenderTarget.name + " Material";

//        //masterPortalRenderer.material.mainTexture = _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

//        //masterPortalRenderer.material.mainTexture.name = RenderTarget.name + " Render Texture";

//        //masterPortalRenderer.material.renderQueue = 2000 + RenderQueueIndex;

//        //transform.position = _masterToSlavePortalOffset;
//    }

//    void LateUpdate()
//    {
//        //var playerGameObject = Game.PlayerGameObject();
//        //var playerCamera = Game.PlayerCamera();

//        ////Log.TraceChange(playerGameObject.transform.position, $"{playerGameObject.transform.position}", "playerGameObject.transform.position");
//        ////Log.TraceChange(playerCamera.transform.position, $"{playerCamera.transform.position}", "playerCamera.transform.position");

//        ////var cameraOffset = playerCamera.transform.position - playerGameObject.transform.position;

//        //var positionOffset = new Vector3(
//        //    playerCamera.transform.position.x - RenderSource.transform.position.x, 
//        //    playerCamera.transform.position.y, 
//        //    playerCamera.transform.position.z - RenderSource.transform.position.z);

//        ////Log.TraceChange(positionOffset, $"{positionOffset}", "positionOffset");

//        //var rotationOffset = RenderSource.transform.localEulerAngles.y - RenderTarget.transform.localEulerAngles.y;

//        ////Log.TraceChange(positionOffset, $"{positionOffset}", "positionOffset");

//        //positionOffset = Quaternion.Euler(0, rotationOffset, 0) * positionOffset;

//        ////Log.TraceChange(positionOffset, $"{positionOffset}", "rotated positionOffset");

//        ////Log.TraceChange(RenderSource.transform.position, $"{RenderSource.transform.position}", "RenderSource.transform.position");

//        ////Log.TraceChange(RenderTarget.transform.position, $"{RenderTarget.transform.position}", "RenderTarget.transform.position");

//        //transform.position = new Vector3(
//        //    RenderTarget.transform.position.x - positionOffset.x, 
//        //    playerCamera.transform.position.y, 
//        //    RenderTarget.transform.position.z - positionOffset.z);

//        //transform.localEulerAngles = new Vector3(
//        //    playerCamera.transform.localEulerAngles.x,
//        //    playerGameObject.transform.localEulerAngles.y - rotationOffset,
//        //    playerCamera.transform.localEulerAngles.z);

//        ////Log.TraceChange(transform.position, $"{transform.position}", "transform.position");

//        ////Log.TraceChange(transform.localEulerAngles, $"{transform.localEulerAngles}", "transform.localEulerAngles");
//    }
//}
