using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PortalScene
{
    private List<GameObject> _nodes = new List<GameObject>();

    private const int RenderedPortalCount = 8;

    private Material[] _portalMaterials;

    public void CreatePortalMaterials()
    {
        if(_portalMaterials != null)
        {
            return;
        }

        _portalMaterials = new Material[RenderedPortalCount];

        for(var i = 0; i < _portalMaterials.Length; i++)
        {
            _portalMaterials[i] = new Material(Resources.Load<Shader>("Portal Render Target Shader"));

            _portalMaterials[i].name = $"Portal Material {i}";

            _portalMaterials[i].mainTexture = new RenderTexture(Screen.width, Screen.height, 24);

            _portalMaterials[i].mainTexture.name = $"Render Texture {i}";
        }
    }

    private GameManager Game { get; set; } = GameManager.Instance;

    private LogManager Log { get; set; } = LogManager.Instance;

    public void SortRenderTargets()
    {
        // find the current node which contains the player

        var materialIndex = 0;

        var depth = 1;

        var currentNode = FindCurrentNode();

        var camera = Game.ComponentByName<Camera>("Camera / Node 1 / South");

        // find any render targets in the current node
        // set depth to 1
        var renderTargets = FindRenderTargets(currentNode);

        foreach(var renderTarget in renderTargets)
        {
            var renderTargetRenderer = renderTarget.GetComponent<Renderer>();

            renderTargetRenderer.enabled = true;

            renderTargetRenderer.material = _portalMaterials[materialIndex];

            camera.targetTexture = _portalMaterials[materialIndex++].mainTexture as RenderTexture;

            camera.depth = depth++;
        }

        // find the connected render target
    }

    private GameObject FindCurrentNode()
    {
        var playerPosition = Game.PlayerGameObject().transform.position;

        foreach(var node in _nodes)
        {
            var floor = Game.GameObjectByName(node, "Floor");

            var renderer = floor.GetComponent<Renderer>();

            if(playerPosition.x < renderer.bounds.min.x || playerPosition.x > renderer.bounds.max.x 
            || playerPosition.z < renderer.bounds.min.z || playerPosition.z > renderer.bounds.max.z)
            {
                continue;
            }

            return node;
        }

        Log.Error("FindCurrentNode() failed. No node was found. Player is outside all nodes");

        return null;
    }

    private IEnumerable<GameObject> FindRenderTargets(GameObject node)
    {
        return Game.GameObjectsByName(node, "Render Target", true);
    }

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
