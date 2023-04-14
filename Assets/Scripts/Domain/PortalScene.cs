using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PortalScene
{
    private List<GameObject> _nodes = new List<GameObject>();

    private List<GameObject> _cameras = new List<GameObject>();

    public void AddNode(GameObject node)
    {
        _nodes.Add(node);
    }

    public void AddCamera(GameObject camera)
    {
        _cameras.Add(camera);
    }

    public void Destroy()
    {
        foreach(var p in _cameras)
        {
            GameObject.Destroy(p);
        }

        foreach (var n in _nodes)
        {
            GameObject.Destroy(n);
        }
    }
}
