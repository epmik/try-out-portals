using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActivePortalController : MonoBehaviour
{
    public GameObject ActivePortal;

    private Vector3 _activePortalOffset;

    private void Update()
    {
        _activePortalOffset = transform.position - ActivePortal.transform.position;
    }

    public Vector3 ActivePortalOffset()
    {
        return _activePortalOffset;
    }
}
