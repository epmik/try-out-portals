using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControllerScene6 : CommonBehaviour
{
    //public GameObject ActivePortal = null;

    //private Camera _camera = null;

    //private Transform _root = null;

    //private Vector3 _cameraRootOffset = Vector3.zero;

    //private Vector3 _activePortalOffset;

    //public Vector3 CameraPosition
    //{
    //    get { return _camera.transform.position; }
    //}

    //public Quaternion CameraLocalRotation
    //{
    //    get { return _camera.transform.localRotation; }
    //}

    //public Camera Camera
    //{
    //    get { return _camera; }
    //}

    void Awake()
    {
        //_camera = GetComponentInChildren<Camera>();
        //_root = GetComponentsInChildren<Transform>().Single(o => o.name == "Root");
        //_cameraRootOffset = GetComponentsInChildren<Transform>().Single(o => o.name == "Camera Offset").localPosition;
    }

    void Update()
    {
        //_activePortalOffset = transform.position + _camera.transform.localPosition - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);
        //_activePortalOffset = transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);
    }

    //public Vector3 ActivePortalOffset()
    //{
    //    return _activePortalOffset;
    //}

    public void TeleportFromToPortal(GameObject fromPortal, GameObject toPortal)
    {
        var positionOffset = fromPortal.transform.position - transform.position;
        var rotationOffset = fromPortal.transform.localEulerAngles.y - toPortal.transform.localEulerAngles.y;

        transform.position = toPortal.transform.position - positionOffset;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + rotationOffset, transform.localEulerAngles.z);

        //if (Intersection.QuadVectorIntersection(gameObject, _lastPlayerPosition, direction))
        //{

        //}

            //var offset = toPortal.transform.position - fromPortal.transform.position;// - Game.PlayerCamera().transform.localPosition;

            //transform.position += offset;

            Physics.SyncTransforms();

        //transform.position += portal.transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);

        //transform.SetPositionAndRotation(
        //    portal.transform.position + _activePortalOffset,
        //    transform.rotation);

        //Debug.Log(name + ": " + transform.position);

        //transform.SetParent(node.transform, false);

        //ActivePortal = toPortal;

        //_activePortalOffset = transform.position + _camera.transform.localPosition - ActivePortal.transform.position;
        //_activePortalOffset = transform.position - ActivePortal.transform.position;

        //Debug.Log("_activePortalOffset " + _activePortalOffset);
        //Debug.Log("ActivePortal " + ActivePortal.name);

        //_activePortalOffset = _camera.transform.position - (ActivePortal != null ? ActivePortal.transform.position : Vector3.zero);
    }
}
