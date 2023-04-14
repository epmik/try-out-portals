using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalColliderScene4 : MonoBehaviour
{
    public GameObject Player;

    private PlayerControllerScene4 _playerController;

    public GameObject ConnectedNode;

    public GameObject ConnectedPortal;

    public bool TriggerIsActive = true;

    private Vector3 _lastPlayerPosition;

    private void Awake()
    {
        _playerController = Player.GetComponent<PlayerControllerScene4>();
    }

    private void LateUpdate()
    {
        if(!TriggerIsActive || ConnectedPortal == null)
        {
            return;
        }

        var direction = _playerController.CameraPosition - _lastPlayerPosition;

        if (Intersects(gameObject, _lastPlayerPosition, direction))
        {
            Debug.Log("Jump!");

            _playerController.TeleportTo(ConnectedNode, ConnectedPortal);
        }

        _lastPlayerPosition = _playerController.CameraPosition;
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
