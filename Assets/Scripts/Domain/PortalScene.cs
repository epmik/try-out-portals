using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PortalSceneObject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public GameObject GameObject { get; set; }

    public TComponent Component<TComponent>()
    {
        return GameObject.GetComponent<TComponent>();
    }
}

public class PortalSceneNode : PortalSceneObject
{
    //private List<PortalScenePortal> _portals = new List<PortalScenePortal>();

    //public PortalScenePortal[] Portals { get { return _portals.ToArray(); } }

    private List<PortalSceneCollider> _colliders = new List<PortalSceneCollider>();

    public PortalSceneCollider[] Colliders { get { return _colliders.ToArray(); } }


    private List<PortalSceneRenderTarget> _renderTargets = new List<PortalSceneRenderTarget>();

    public PortalSceneRenderTarget[] RenderTargets { get { return _renderTargets.ToArray(); } }



    //private List<PortalScenePortal> _portals = new List<PortalScenePortal>();

    //public PortalScenePortal[] Portals { get { return _portals.ToArray(); } }

    private Bounds? _bounds;

    public Bounds Bounds 
    {
        get
        {
            if(_bounds == null)
            {
                var floor = GameManager.Instance.GameObjectByName(GameObject, "Floor");

                var renderer = floor.GetComponent<Renderer>();

                _bounds = renderer.bounds;
            }

            return _bounds.Value;
        }
    }

    public void AddCollider(PortalSceneCollider collider)
    {
        _colliders.Add(collider);
    }

    public void AddRenderTarget(PortalSceneRenderTarget renderTarget)
    {
        _renderTargets.Add(renderTarget);
    }

    public override string ToString()
    {
        return GameObject.name;
    }
}

public class PortalSceneCamera : PortalSceneObject
{
    public PortalSceneRenderTarget RenderTarget { get; set; }

    public PortalSceneRenderSource RenderSource { get; set; }

    private Camera _cameraComponent;

    public Camera CameraComponent()
    {
        if(_cameraComponent == null )
        {
            _cameraComponent = Component<Camera>();
        }
        return _cameraComponent;
    }

    public override string ToString()
    {
        return RenderSource + " / " + RenderTarget;
    }

}

public class PortalSceneRenderTarget : PortalSceneObject
{
    public PortalSceneNode Node { get; set; }
    public Location Location { get; set; }
    public PortalSceneCamera Camera { get; set; }

    public override string ToString()
    {
        return $"Target: {Node} {Location}";
    }
}

public class PortalSceneRenderSource : PortalSceneObject
{
    public PortalSceneNode Node { get; set; }
    public Location Location { get; set; }
    public PortalSceneCamera Camera { get; set; }

    public override string ToString()
    {
        return $"Source: {Node} {Location}";
    }
}

public class PortalSceneCollider : PortalSceneObject
{
    public PortalSceneNode Node { get; set; }
    public Location Location { get; set; }
    public PortalSceneCollider ConnectedCollider { get; set; }

    public override string ToString()
    {
        return $"Collider: {Node} {Location}";
    }
}

public class PortalScene
{
    private List<PortalSceneNode> _nodes = new List<PortalSceneNode>();

    private const int RenderedPortalCount = 16;

    private Material[] _renderTargetMaterials;

    public void GenerateRenderTargetMaterials()
    {
        if(_renderTargetMaterials != null)
        {
            return;
        }

        _renderTargetMaterials = new Material[RenderedPortalCount];

        for(var i = 0; i < _renderTargetMaterials.Length; i++)
        {
            _renderTargetMaterials[i] = new Material(Resources.Load<Shader>("Portal Render Target Shader"));

            _renderTargetMaterials[i].name = $"Portal Material {i}";

            _renderTargetMaterials[i].mainTexture = new RenderTexture(Screen.width, Screen.height, 24);

            _renderTargetMaterials[i].mainTexture.name = $"Render Texture {i}";
        }
    }

    private GameManager Game { get; set; } = GameManager.Instance;

    private LogManager Log { get; set; } = LogManager.Instance;

    public void SortRenderTargets()
    {
        var node = FindCurrentNode();

        var visitedNodes = new List<PortalSceneNode>();

        SortRenderTargetsInternal(
            node,
            Game.PlayerCamera(),
            0,
            1,
            visitedNodes);
    }

    private void SortRenderTargetsInternal(
        PortalSceneNode node, 
        Camera parentCamera,
        int materialIndex, 
        int depth, 
        List<PortalSceneNode> visitedNodes)
    {
        if(visitedNodes.Any(o => o.Id == node.Id))
        {
            return;
        }

        visitedNodes.Add(node);

        foreach (var renderTarget in node.RenderTargets)
        {
            var rendererComponent = renderTarget.Component<Renderer>();

            rendererComponent.enabled = true;

            rendererComponent.material = _renderTargetMaterials[materialIndex];

            var cameraComponent = renderTarget.Camera.Component<Camera>();

            cameraComponent.targetTexture = _renderTargetMaterials[materialIndex++].mainTexture as RenderTexture;

            cameraComponent.depth = depth;

            PositionCamera(
                parentCamera, 
                cameraComponent,
                renderTarget.Camera.RenderTarget.GameObject,
                renderTarget.Camera.RenderSource.GameObject);
        }

        foreach (var renderTarget in node.RenderTargets)
        {
            SortRenderTargetsInternal(
                renderTarget.Camera.RenderSource.Node,
                renderTarget.Camera.Component<Camera>(),
                materialIndex,
                depth + 1,
                visitedNodes);
        }
    }

    private void PositionCamera(
        Camera parentCamera,
        Camera cameraToPosition,
        GameObject renderTarget,
        GameObject renderSource)
    {
        var playerGameObject = parentCamera.gameObject;

        //Log.TraceChange(playerGameObject.transform.position, $"{playerGameObject.transform.position}", "playerGameObject.transform.position");
        //Log.TraceChange(playerCamera.transform.position, $"{playerCamera.transform.position}", "playerCamera.transform.position");

        //var cameraOffset = playerCamera.transform.position - playerGameObject.transform.position;

        var positionOffset = new Vector3(
            parentCamera.transform.position.x - renderTarget.transform.position.x,
            parentCamera.transform.position.y,
            parentCamera.transform.position.z - renderTarget.transform.position.z);

        //Log.TraceChange(positionOffset, $"{positionOffset}", "positionOffset");

        var rotationOffset = renderTarget.transform.localEulerAngles.y - renderSource.transform.localEulerAngles.y;

        //Log.TraceChange(positionOffset, $"{positionOffset}", "positionOffset");

        positionOffset = Quaternion.Euler(0, rotationOffset, 0) * positionOffset;

        //Log.TraceChange(positionOffset, $"{positionOffset}", "rotated positionOffset");

        //Log.TraceChange(RenderSource.transform.position, $"{RenderSource.transform.position}", "RenderSource.transform.position");

        //Log.TraceChange(RenderTarget.transform.position, $"{RenderTarget.transform.position}", "RenderTarget.transform.position");

        cameraToPosition.transform.position = new Vector3(
            renderSource.transform.position.x - positionOffset.x,
            parentCamera.transform.position.y,
            renderSource.transform.position.z - positionOffset.z);

        cameraToPosition.transform.localEulerAngles = new Vector3(
            parentCamera.transform.localEulerAngles.x,
            playerGameObject.transform.localEulerAngles.y - rotationOffset,
            parentCamera.transform.localEulerAngles.z);

        //Log.TraceChange(transform.position, $"{transform.position}", "transform.position");

        //Log.TraceChange(transform.localEulerAngles, $"{transform.localEulerAngles}", "transform.localEulerAngles");
    }

    private PortalSceneNode FindCurrentNode()
    {
        var playerPosition = Game.PlayerGameObject().transform.position;

        foreach(var node in _nodes)
        {
            if(playerPosition.x < node.Bounds.min.x || playerPosition.x > node.Bounds.max.x 
            || playerPosition.z < node.Bounds.min.z || playerPosition.z > node.Bounds.max.z)
            {
                continue;
            }

            return node;
        }

        Log.Error("FindCurrentNode() failed. No node was found. Player is outside all nodes");

        return null;
    }

    //private IEnumerable<GameObject> FindRenderTargets(GameObject node)
    //{
    //    return Game.GameObjectsByName(node, "Render Target", true);
    //}

    //private List<GameObject> _cameras = new List<GameObject>();

    public void AddNode(PortalSceneNode node)
    {
        _nodes.Add(node);
    }

    //public void AddCamera(GameObject camera)
    //{
    //    _cameras.Add(camera);
    //}

    public void Destroy()
    {
        //foreach(var p in _cameras)
        //{
        //    GameObject.Destroy(p);
        //}

        //foreach (var n in _nodes)
        //{
        //    GameObject.Destroy(n);
        //}
    }
}
