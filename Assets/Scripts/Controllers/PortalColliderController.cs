using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalColliderController : MonoBehaviour
{
    public GameObject Player;

    public GameObject ConnectedPortal;

    public bool TriggerIsActive = true;

    private Vector3 _lastPlayerPosition;

    //private void Awake()
    //{

    //    var renderer = GetComponent<Renderer>();

    //    _colliderPlane.Set3Points(
    //        new Vector3(renderer.bounds.min.x, renderer.bounds.min.y, renderer.bounds.min.z),
    //        new Vector3(renderer.bounds.min.x, renderer.bounds.max.y, renderer.bounds.min.z),
    //        new Vector3(renderer.bounds.max.x, renderer.bounds.max.y, renderer.bounds.min.z));

    //    // yes
    //    Intersects(gameObject, new Vector3(0, 2, -0.1f), new Vector3(0, 0, 0.2f));

    //    // no, off on x-axis
    //    Intersects(gameObject, new Vector3(10, 2, -0.1f), new Vector3(0, 0, 0.2f));

    //    // yes, just touches
    //    Intersects(gameObject, new Vector3(0, 2, -0.1f), new Vector3(0, 0, 0.1f));

    //    // no, 0.01 off on z-axis
    //    Intersects(gameObject, new Vector3(0, 2, -0.1f), new Vector3(0, 0, 0.09f));

    //    // no, behind quad plane
    //    Intersects(gameObject, new Vector3(0, 2, 0.1f), new Vector3(0, 0, 0.2f));

    //    // no, start in quad plane
    //    Intersects(gameObject, new Vector3(0, 2, 0f), new Vector3(0, 0, 0.2f));
    //}

    private void LateUpdate()
    {
        if(!TriggerIsActive || ConnectedPortal == null)
        {
            return;
        }

        var direction = Player.transform.position - _lastPlayerPosition;

        if (Intersects(gameObject, _lastPlayerPosition, direction))
        {
            Debug.Log("Jump!");

            Player.transform.position += ConnectedPortal.transform.position - transform.position;

            var playerActivePortalController = Player.GetComponent<PlayerActivePortalController>();

            playerActivePortalController.ActivePortal = ConnectedPortal;
        }

        _lastPlayerPosition = Player.transform.position;
    }

    private bool Intersects(GameObject quad, Vector3 position, Vector3 direction)
    {
        var mesh = quad.GetComponent<MeshFilter>().mesh;
        var n = quad.transform.TransformDirection(mesh.normals[0]);
        var p = quad.transform.TransformPoint(mesh.vertices[0]);

        var plane = new Plane(n, p);

        var distance = plane.GetDistanceToPoint(position);

        if(distance <= 0)
        {
            return false;
        }

        var ray = new Ray(position, direction);

        plane.Raycast(ray, out float enter);

        if(enter <= 0)
        {
            return false;
        }

        var l = direction.magnitude;

        if(enter > direction.magnitude)
        {
            return false;
        }

        var intersection = position + direction.normalized * enter;

        var bounds = quad.GetComponent<Renderer>().bounds;

        if (intersection.x > bounds.max.x || intersection.x < bounds.min.x)
        {
            return false;
        }

        if (intersection.y > bounds.max.y || intersection.y < bounds.min.y)
        {
            return false;
        }

        if (intersection.z > bounds.max.z || intersection.z < bounds.min.z)
        {
            return false;
        }

        return true;
    }
}
