using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControllerScene4 : MonoBehaviour
{
    public GameObject ActivePortal = null;

    private Camera _camera = null;

    //private Transform _root = null;

    //private Vector3 _cameraRootOffset = Vector3.zero;

    //private Vector3 _activePortalOffset;

    public Vector3 CameraPosition
    {
        get { return _camera.transform.position; }
    }

    public Quaternion CameraLocalRotation
    {
        get { return _camera.transform.localRotation; }
    }

    public Camera Camera
    {
        get { return _camera; }
    }

    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        //_root = GetComponentsInChildren<Transform>().Single(o => o.name == "Root");
        //_cameraRootOffset = GetComponentsInChildren<Transform>().Single(o => o.name == "Camera Offset").localPosition;
    }

    void Update()
    {
        //_activePortalOffset = _camera.transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);
    }

    //public Vector3 ActivePortalOffset()
    //{
    //    return _activePortalOffset;
    //}

    public void TeleportTo(GameObject node, GameObject portal)
    {
        //transform.position += portal.transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);

        transform.SetParent(node.transform, false);

        ActivePortal = portal;

        //_activePortalOffset = _camera.transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);
    }
}
