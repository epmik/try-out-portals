using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalColliderScene5 : CommonBehaviour
{
    //public GameObject Player;

    //private PlayerControllerScene5 _playerController;

    public GameObject ConnectedPortal;

    public bool TriggerIsActive = true;

    private Vector3 _lastPlayerPosition;

    private void Awake()
    {
        //_playerController = Player.GetComponent<PlayerControllerScene5>();
    }

    private void LateUpdate()
    {
        if(Game.LogLateUpdateCalls)
        {
            Debug.Log(this.name + "LateUpdate");
        }

        if(!TriggerIsActive || ConnectedPortal == null)
        {
            return;
        }

        var direction = Game.PlayerGameObject().transform.position - _lastPlayerPosition;

        if(direction.magnitude == 0)
        {
            return;
        }

        if (Intersects(gameObject, _lastPlayerPosition, direction))
        {
            //Game.LogUpdateCalls = true;

            Debug.Log("Jump!");

            Debug.Log("Player.transform.position " + Game.PlayerGameObject().transform.position);

            Debug.Log("_lastPlayerPosition " + _lastPlayerPosition);
            Debug.Log("_lastPlayerPosition direction " + direction);

            Game.PlayerController().TeleportFromToPortal(gameObject, ConnectedPortal);

            Debug.Log("Player.transform.position " + Game.PlayerGameObject().transform.position);
        }

        _lastPlayerPosition = Game.PlayerGameObject().transform.position;
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

        //if (intersection.y > bounds.max.y || intersection.y < bounds.min.y)
        //{
        //    return false;
        //}

        if (intersection.z > bounds.max.z || intersection.z < bounds.min.z)
        {
            return false;
        }

        return true;
    }
}
