using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Intersection
{
    public static bool QuadVectorIntersection(GameObject quad, Vector3 position, Vector3 direction)
    {
        var mesh = quad.GetComponent<MeshFilter>().mesh;
        var n = quad.transform.TransformDirection(mesh.normals[0]);
        var p = quad.transform.TransformPoint(mesh.vertices[0]);

        var plane = new Plane(n, p);

        var distance = plane.GetDistanceToPoint(position);

        if (distance <= 0)
        {
            return false;
        }

        var ray = new Ray(position, direction);

        plane.Raycast(ray, out float enter);

        if (enter <= 0)
        {
            return false;
        }

        var l = direction.magnitude;

        if (enter > direction.magnitude)
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
