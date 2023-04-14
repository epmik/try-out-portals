using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraController : MonoBehaviour
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

        var masterPortalMaterialController = MasterPortal.GetComponent<PortalMaterialController>();

        if (masterPortalMaterialController != null)
        {
            // used in scene 2
            var masterPortalMaterial = masterPortalMaterialController.InitializeMaterial();

            masterPortalMaterial.mainTexture = _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        }
        //else
        //{
        //    // for scene 0

        //    var masterPortalRenderer = MasterPortal.GetComponent<Renderer>();

        //    var masterPortalMaterial = masterPortalRenderer.material;

        //    if (masterPortalMaterial.shader.name != "Private/Portal Shader")
        //    {
        //        masterPortalMaterial.shader = Resources.Load<Shader>("Portal Shader");
        //    }
        //}


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
