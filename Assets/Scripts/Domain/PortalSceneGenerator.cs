using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PortalSceneGeneratorSettings
{
    public int Seed;

    // this should be greater then the camera near plane distance
    public float PortalThickness = 0.2f;

    public string PortalCollisionMaterialName = "Portal Collision Material";

    //public string PortalCollisionShaderName = "Portal Collision Shader";

    //public string PortalTargetMaterialName = "Portal Render Target Material";

    //public string PortalTargetShaderName = "Portal Render Target Shader";

    public PortalSceneGeneratorSettings()
    {
        Seed = (int)DateTime.Now.Ticks;
    }
}

public class PortalSceneGenerator
{
    public PortalSceneGeneratorSettings Settings
    {
        get;
        private set;
    }

    private class GenerateCollidersAndRenderTargetsSettings
    {
        public PortalSceneNode FromNode;
        public Location FromLocation;

        public PortalSceneNode ToNode;
        public Location ToLocation;

        public bool FromExteriorToInterior = true;

        public bool CreateColliders = true;

        public bool CreateRenderTargets = true;
    }

    private LogManager _log = LogManager.Instance;

    private float _halfPortalThickness;

    private Material _portalCollisionMaterial = null;

    private System.Random _random;

    private GameManager Game { get; set; }

    private PortalScene _portalScene { get; set; }

    public PortalSceneGenerator()
    {
        _log = LogManager.Instance;

        Settings = new PortalSceneGeneratorSettings();

        Game = GameManager.Instance;
    }

    public static PortalScene GenerateDefaultPortalScene()
    {
        return new PortalSceneGenerator().Generate();
    }

    public PortalScene Generate()
    {
        SetupVariables();

        // get root
        var root = Game.GameObjectByName("Root");


        //// find node 0
        //var node0 = Game.GameObjectByName("Node 0");

        //// find node 1
        //var node1 = Game.GameObjectByName("Node 1");

        //GeneratePortals(node0, Location.West, node1, Location.West);



        // load a prefab
        var nodePrefab = Resources.Load<GameObject>("Node Prefab");



        // instantiate prefab
        
        var nodePosition = new Vector3(0, 0, 0);

        var node0 = new PortalSceneNode 
        { 
            GameObject = UnityEngine.Object.Instantiate(nodePrefab, nodePosition, Quaternion.identity),
        };

        _portalScene.AddNode(node0);

        node0.GameObject.name = "Node 0";
        node0.GameObject.transform.SetParent(root.transform, false);

        AssignRandomMaterialToFloorAndWalls(node0);



        // instantiate prefab
        
        nodePosition += new Vector3(30, 0, 0);

        var node1 = new PortalSceneNode
        {
            GameObject = UnityEngine.Object.Instantiate(nodePrefab, nodePosition, Quaternion.identity),
        };

        _portalScene.AddNode(node1);

        node1.GameObject.name = "Node 1";
        node1.GameObject.transform.SetParent(root.transform, false);

        AssignRandomMaterialToFloorAndWalls(node1);



        //// instantiate prefab

        //nodePosition += new Vector3(30, 0, 0);

        //var node2 = UnityEngine.Object.Instantiate(nodePrefab, nodePosition, Quaternion.identity);

        //node2.name = "Node 3";
        //node2.transform.SetParent(root.transform, false);

        //AssignRandomMaterialToFloorAndWalls(node2);


        GenerateCollidersAndRenderTargets(
             new GenerateCollidersAndRenderTargetsSettings
             {
                 FromNode = node0,
                 FromLocation = Location.West,
                 ToNode = node1,
                 ToLocation = Location.South,
                 FromExteriorToInterior = true,
                 CreateColliders = true,
                 CreateRenderTargets = true,
             });

        //GenerateConnectedPortals(
        //     new GenerateConnectedPortalsSettings
        //     {
        //         FromPortal = node0,
        //         FromLocation = Location.East,
        //         ToPortal = node1,
        //         ToLocation = Location.North,
        //         FromExteriorToInterior = false,
        //     });

        //GenerateConnectedPortals(
        //     new GenerateConnectedPortalsSettings
        //     {
        //         FromPortal = node0,
        //         FromLocation = Location.South,
        //         ToPortal = node4,
        //         ToLocation = Location.West,
        //         FromExteriorToInterior = true,
        //     });

        //GenerateConnectedPortals(
        //     new GenerateConnectedPortalsSettings
        //     {
        //         FromPortal = node4,
        //         FromLocation = Location.East,
        //         ToPortal = node1,
        //         ToLocation = Location.West,
        //         FromExteriorToInterior = false,
        //     });

        MovePlayerToNode(node0);

        _portalScene.GenerateRenderTargetMaterials();

        return _portalScene;
    }

    private void HideBottomBeam(PortalSceneNode node, Location location)
    {
        // find the bottom beam
        var fromBottom = Game.GameObjectByName(node.GameObject, location + " Bottom");

        // set the bottom beam to inactive: no rendering or collision detection
        fromBottom.SetActive(false);
    }

    //private GameObject PlaceholderGameObject(PortalSceneNode node, Location location)
    //{
    //    return Game.GameObjectByName(node.GameObject, location.ToString());
    //}

    private void GenerateCollidersAndRenderTargets(GenerateCollidersAndRenderTargetsSettings settings)
    {
        var fromPlaceHolder = Game.GameObjectByName(settings.FromNode.GameObject, settings.FromLocation.ToString());

        var toPlaceHolder = Game.GameObjectByName(settings.ToNode.GameObject, settings.ToLocation.ToString());

        var fromFromRenderTarget = new PortalSceneRenderTarget 
        {
            Node = settings.FromNode,
            Location = settings.FromLocation,
            GameObject = GeneratePortalRenderTarget(fromPlaceHolder, settings.FromLocation, settings.FromExteriorToInterior)
        };

        var fromToRenderSource = new PortalSceneRenderSource 
        { 
            Node = settings.ToNode,
            Location = settings.ToLocation,
            GameObject = GeneratePortalRenderSource(toPlaceHolder, settings.ToLocation, !settings.FromExteriorToInterior)
        };

        settings.FromNode.AddRenderTarget(fromFromRenderTarget);

        var toFromRenderTarget = new PortalSceneRenderTarget
        {
            Node = settings.ToNode,
            Location = settings.ToLocation,
            GameObject = GeneratePortalRenderTarget(toPlaceHolder, settings.ToLocation, !settings.FromExteriorToInterior)
        };

        var toToRenderSource = new PortalSceneRenderSource
        {
            Node = settings.FromNode,
            Location = settings.FromLocation,
            GameObject = GeneratePortalRenderSource(fromPlaceHolder, settings.FromLocation, settings.FromExteriorToInterior)
        };

        settings.ToNode.AddRenderTarget(toFromRenderTarget);

        GeneratePortalCameras(fromFromRenderTarget, fromToRenderSource);

        GeneratePortalCameras(toFromRenderTarget, toToRenderSource);

        if (settings.CreateColliders)
        {
            HideBottomBeam(settings.FromNode, settings.FromLocation);

            HideBottomBeam(settings.ToNode, settings.ToLocation);

            var fromCollider = new PortalSceneCollider
            {
                GameObject = GeneratePortalCollider(fromPlaceHolder, settings.FromLocation, settings.FromExteriorToInterior)
            };

            var toCollider = new PortalSceneCollider
            {
                GameObject = GeneratePortalCollider(toPlaceHolder, settings.ToLocation, !settings.FromExteriorToInterior)
            };

            var fromColliderComponent = fromCollider.GameObject.AddComponent<PortalColliderScene6>();
            fromColliderComponent.ConnectedPortal = toCollider.GameObject;

            var toColliderComponent = toCollider.GameObject.AddComponent<PortalColliderScene6>();
            toColliderComponent.ConnectedPortal = fromCollider.GameObject;


            fromCollider.ConnectedCollider = toCollider;
            fromCollider.Node = settings.FromNode;
            fromCollider.Location = settings.FromLocation;

            toCollider.ConnectedCollider = fromCollider;
            toCollider.Node = settings.ToNode;
            toCollider.Location = settings.ToLocation;

            settings.FromNode.AddCollider(fromCollider);
        }
    }

    private void GeneratePortalCameras(
        PortalSceneRenderTarget renderTarget,
        PortalSceneRenderSource renderSource)
    {
        var root = Game.GameObjectByName("Root");

        var playerCamera = Game.PlayerCamera();

        GeneratePortalCamera(renderTarget, renderSource, root, playerCamera);
    }

    private void GeneratePortalCamera(
        PortalSceneRenderTarget renderTarget,
        PortalSceneRenderSource renderSource, 
        GameObject parent, 
        Camera playerCamera)
    {
        var portalCamera = new PortalSceneCamera
        {
            GameObject = new GameObject($"Camera {renderSource} / {renderTarget}"),
            RenderTarget = renderTarget,
            RenderSource = renderSource,
        };

        renderTarget.Camera = portalCamera;
        renderSource.Camera = portalCamera;

        //toPortal.AddCamera(portalCamera);



        // create a plane and add it to the portal
        // this will become the rendering texture target

        //portalCamera.RenderTarget = new PortalSceneRenderTarget
        //{
        //    Portal = fromPortal,
        //    GameObject = GeneratePortalRenderTarget(fromPortal.GameObject, settings.FromLocation, settings.FromExteriorToInterior)
        //};

        //// create a plane and add it to the west portal
        //// this will become the rendering texture target
        //portalCamera.RenderSource = new PortalSceneRenderSource
        //{
        //    Portal = toPortal,
        //    GameObject = GeneratePortalRenderTarget(toPortal.GameObject, settings.ToLocation, !settings.FromExteriorToInterior),
        //};

        portalCamera.RenderTarget.GameObject.AddComponent<PortalRenderTargetScene6>();

        var cameraComponent = portalCamera.GameObject.AddComponent<Camera>();

        cameraComponent.CopyFrom(playerCamera);

        portalCamera.GameObject.transform.parent = parent.transform;

        //var cameraControllerComponent = portalCamera.GameObject.AddComponent<PortalCameraControllerScene6>();

        //cameraControllerComponent.RenderSource = portalCamera.RenderTarget.GameObject;
        //cameraControllerComponent.RenderTarget = portalCamera.RenderSource.GameObject;
    }

    //private string NodeNameAndLocation(PortalScenePortal portal)
    //{
    //    return portal.Node.GameObject.name + " / " + portal.Location;
    //}

    //private void ConnectPortals(PortalScenePortal fromPortal, PortalScenePortal toPortal)
    //{
    //    fromPortal.ConnectedPortal = toPortal;
    //    toPortal.ConnectedPortal = fromPortal;

    //    _log.Trace($"connected quad ({fromPortal.GameObject.name}) to  {toPortal.GameObject.name}");
    //    _log.Trace($"connected quad ({toPortal.GameObject.name}) to  {fromPortal.GameObject.name}");
    //}

    //private void GenerateColliderComponents(PortalScenePortal fromPortal, PortalScenePortal toPortal)
    //{
    //}

    //private void GenerateRenderTargetComponents(PortalScenePortal fromPortal, PortalScenePortal toPortal)
    //{
    //    fromPortal.RenderTarget.GameObject.AddComponent<PortalRenderTargetScene6>();

    //    toPortal.RenderTarget.GameObject.AddComponent<PortalRenderTargetScene6>();
    //}

    private GameObject GeneratePortalRenderTarget(
        GameObject locationPlaceholder,
        Location location,
        bool fromExteriorToInterior)
    {
        // create a plane and add it to the locationPlaceholder
        // this will become the rendering texture target

        var renderTargetQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        renderTargetQuad.name = "Render Target";
        renderTargetQuad.transform.localPosition = RenderTargetPositionFor(location, fromExteriorToInterior);
        renderTargetQuad.transform.localEulerAngles = RenderTargetEulerAnglesFor(location, fromExteriorToInterior);
        renderTargetQuad.transform.localScale = ScaleFor(location, fromExteriorToInterior);

        renderTargetQuad.transform.SetParent(locationPlaceholder.transform, false);

        // disable the default collider
        //renderTargetQuad.GetComponent<MeshCollider>().enabled = false;

        // disabled for now
        renderTargetQuad.GetComponent<Renderer>().enabled = false;

        _log.Trace($"created render target quad ({renderTargetQuad.name}) at world {renderTargetQuad.transform.position} {renderTargetQuad.transform.eulerAngles}");
        _log.Trace($"created render target quad ({renderTargetQuad.name}) at local {renderTargetQuad.transform.localPosition} {renderTargetQuad.transform.localEulerAngles}");

        return renderTargetQuad;
    }

    private GameObject GeneratePortalRenderSource(
        GameObject locationPlaceholder,
        Location location,
        bool fromExteriorToInterior)
    {
        // create a plane and add it to the locationPlaceholder
        // this will become the rendering texture target

        var renderTargetQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        renderTargetQuad.name = "Render Source";
        renderTargetQuad.transform.localPosition = RenderTargetPositionFor(location, fromExteriorToInterior);
        renderTargetQuad.transform.localEulerAngles = RenderTargetEulerAnglesFor(location, fromExteriorToInterior);
        renderTargetQuad.transform.localScale = ScaleFor(location, fromExteriorToInterior);

        renderTargetQuad.transform.SetParent(locationPlaceholder.transform, false);

        // disable the default collider
        //renderTargetQuad.GetComponent<MeshCollider>().enabled = false;

        // disabled for now
        renderTargetQuad.GetComponent<Renderer>().enabled = false;

        _log.Trace($"created render source quad ({renderTargetQuad.name}) at world {renderTargetQuad.transform.position} {renderTargetQuad.transform.eulerAngles}");
        _log.Trace($"created render source quad ({renderTargetQuad.name}) at local {renderTargetQuad.transform.localPosition} {renderTargetQuad.transform.localEulerAngles}");

        return renderTargetQuad;
    }

    private GameObject GeneratePortalCollider(
        GameObject locationPlaceholder,
        Location location,
        bool fromExteriorToInterior)
    {
        // create a plane and add it to the locationPlaceholder
        // this will become the collision detection plane

        var colliderQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        colliderQuad.name = "Collider";
        colliderQuad.transform.localPosition = CollisionPositionFor(location, fromExteriorToInterior);
        colliderQuad.transform.localEulerAngles = CollisionEulerAnglesFor(location, fromExteriorToInterior);
        colliderQuad.transform.localScale = ScaleFor(location, fromExteriorToInterior);

        colliderQuad.transform.SetParent(locationPlaceholder.transform, false);
        // disable the default collider
        colliderQuad.GetComponent<MeshCollider>().enabled = false;

        colliderQuad.GetComponent<Renderer>().material = _portalCollisionMaterial;

        _log.Trace($"created collider quad ({colliderQuad.name}) at world {colliderQuad.transform.position} {colliderQuad.transform.eulerAngles}");
        _log.Trace($"created collider quad ({colliderQuad.name}) at local {colliderQuad.transform.localPosition} {colliderQuad.transform.localEulerAngles}");

        return colliderQuad;
    }

    private void MovePlayerToNode(PortalSceneNode node)
    {
        var player = Game.PlayerGameObject();

        player.transform.position = node.GameObject.transform.position + new Vector3(-3, 0, -8);

        Physics.SyncTransforms();
    }

    private Vector3 CollisionPositionFor(Location location, bool fromExteriorToInterior)
    {
        switch(location)
        {
            case Location.North:
                return new Vector3(0, 0, fromExteriorToInterior ? _halfPortalThickness : -_halfPortalThickness);
            case Location.East:
                return new Vector3(fromExteriorToInterior ? _halfPortalThickness : -_halfPortalThickness, 0, 0);
            case Location.South:
                return new Vector3(0, 0, fromExteriorToInterior ? -_halfPortalThickness : _halfPortalThickness);
            default:
                return new Vector3(fromExteriorToInterior ? -_halfPortalThickness : _halfPortalThickness, 0, 0);
        }
    }

    private Vector3 ScaleFor(Location location, bool fromExteriorToInterior)
    {
        return new Vector3(3, 3, 1);

        //switch (location)
        //{
        //    case Location.North:
        //    case Location.South:
        //        return new Vector3(1, 3, 3);
        //    default:
        //        return new Vector3(3, 3, 1);
        //}
    }

    private Vector3 CollisionEulerAnglesFor(Location location, bool fromExteriorToInterior)
    {
        switch (location)
        {
            case Location.North:
                return new Vector3(0, fromExteriorToInterior ? 180 : 0, 0);
            case Location.East:
                return new Vector3(0, fromExteriorToInterior ? 270 : 90, 0);
            case Location.South:
                return new Vector3(0, fromExteriorToInterior ? 0 : 180, 0);
            default:
                return new Vector3(0, fromExteriorToInterior ? 90 : 270, 0);
        }
    }

    private Vector3 RenderTargetEulerAnglesFor(Location location, bool fromExteriorToInterior)
    {
        switch (location)
        {
            case Location.North:
                return new Vector3(0, fromExteriorToInterior ? 180 : 0, 0);
            case Location.East:
                return new Vector3(0, fromExteriorToInterior ? 270 : 90, 0);
            case Location.South:
                return new Vector3(0, fromExteriorToInterior ? 0 : 180, 0);
            default:
                return new Vector3(0, fromExteriorToInterior ? 90 : 270, 0);
        }
    }

    private Vector3 RenderTargetPositionFor(Location location, bool fromExteriorToInterior)
    {
        switch (location)
        {
            case Location.North:
                return new Vector3(0, 0, fromExteriorToInterior ? -_halfPortalThickness : _halfPortalThickness);
            case Location.East:
                return new Vector3(fromExteriorToInterior ? -_halfPortalThickness : _halfPortalThickness, 0, 0);
            case Location.South:
                return new Vector3(0, 0, fromExteriorToInterior ? _halfPortalThickness : -_halfPortalThickness);
            default:
                return new Vector3(fromExteriorToInterior ? _halfPortalThickness : -_halfPortalThickness, 0, 0);
        }
    }

    private void SetupVariables()
    {
        _portalScene = new PortalScene();

        _random = new System.Random(Settings.Seed);

        _halfPortalThickness = Settings.PortalThickness * 0.5f;

        _portalCollisionMaterial = Resources.Load<Material>(Settings.PortalCollisionMaterialName);
    }

    private void AssignRandomMaterialToFloorAndWalls(PortalSceneNode node)
    {
        var exterior = Game.GameObjectByName(node.GameObject, "Exterior");

        var whiteMaterial = Resources.Load<Material>("White Material");

        var material = new Material(whiteMaterial) 
        { 
            color = Color.HSVToRGB((float)_random.NextDouble(), 0.95f, 0.85f) 
        };

        material.name = material.color.ToString() + " Node Material";

        foreach (var r in Game.Components<Renderer>(exterior, false))
        {
            r.material = material;
        }
    }

    //private void GeneratePortals()
    //{
    //    var game = GameManager.Instance;

    //    // find node 0
    //    var node0 = game.GameObjectByName("Node 0");

    //    // find the west placeholder
    //    var node0WestPlaceholder = game.GameObjectByName(node0, "West");

    //    // find the bottom beam
    //    var node0WestBottom = game.GameObjectByName(node0, "West Bottom");

    //    // set the bottom beam to inactive: no rendering or collision detection
    //    node0WestBottom.SetActive(false);


    //    //// find the renderer component
    //    //var node0WestBottomRenderer = node0WestBottom.GetComponent<Renderer>();

    //    //node0WestBottomRenderer = game.ComponentByName<Renderer>(node0, "West Bottom");

    //    //// disable the bottom beam from rendering
    //    //node0WestBottomRenderer.enabled = false;


    //    // create a plane and add it to the west portal
    //    // this will become the rendering texture target

    //    var node0WestRenderTargetQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //    node0WestRenderTargetQuad.name = "Target";
    //    node0WestRenderTargetQuad.transform.localPosition = new Vector3(0, 0, _halfPortalThickness);
    //    // no need to set the scale, the correct scale is set in the parent node
    //    node0WestRenderTargetQuad.transform.SetParent(node0WestPlaceholder.transform, false);
    //    // disable the default collider
    //    node0WestRenderTargetQuad.GetComponent<MeshCollider>().enabled = false;

    //    _log.Trace($"created quad ({node0WestRenderTargetQuad.name}) at world {node0WestRenderTargetQuad.transform.position} {node0WestRenderTargetQuad.transform.eulerAngles}");
    //    _log.Trace($"created quad ({node0WestRenderTargetQuad.name}) at local {node0WestRenderTargetQuad.transform.localPosition} {node0WestRenderTargetQuad.transform.localEulerAngles}");

    //    // create a plane and add it to the west portal
    //    // this will become the collision detection plane

    //    var node0WestCollisionQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //    node0WestCollisionQuad.name = "Collision";
    //    node0WestCollisionQuad.transform.localPosition = new Vector3(0, 0, -_halfPortalThickness);
    //    // no need to set the scale, the correct scale is set in the parent node
    //    node0WestCollisionQuad.transform.SetParent(node0WestPlaceholder.transform, false);
    //    // disable the default collider
    //    node0WestCollisionQuad.GetComponent<MeshCollider>().enabled = false;

    //    // add a custom collidor and set the ConnectedPortal property
    //    var node0WestPortalCollider = node0WestCollisionQuad.AddComponent<PortalColliderScene6>();
    //    // node0WestPortalCollider.ConnectedPortal = null;

    //    _log.Trace($"created quad ({node0WestPortalCollider.name}) at world {node0WestPortalCollider.transform.position} {node0WestPortalCollider.transform.eulerAngles}");
    //    _log.Trace($"created quad ({node0WestPortalCollider.name}) at local {node0WestPortalCollider.transform.localPosition} {node0WestPortalCollider.transform.localEulerAngles}");


    //    ///////////
    //    //
    //    // create a portal with rendering and clipping plane in the west placeholder
    //    // in node 0
    //    //

    //    // find node 0
    //    var node1 = game.GameObjectByName("Node 1");

    //    // find the west placeholder
    //    var node1WestPlaceholder = game.GameObjectByName(node1, "West");

    //    // find the bottom beam
    //    var node1WestBottom = game.GameObjectByName(node1, "West Bottom");

    //    // set the bottom beam to inactive: no rendering or collision detection
    //    node1WestBottom.SetActive(false);


    //    //// find the renderer component
    //    //var node0WestBottomRenderer = node0WestBottom.GetComponent<Renderer>();

    //    //node0WestBottomRenderer = game.ComponentByName<Renderer>(node0, "West Bottom");

    //    //// disable the bottom beam from rendering
    //    //node0WestBottomRenderer.enabled = false;


    //    // create a plane and add it to the west portal
    //    // this will become the rendering texture target

    //    var node1WestRenderTargetQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //    node1WestRenderTargetQuad.name = "Target";
    //    node1WestRenderTargetQuad.transform.localPosition = new Vector3(0, 0, -_halfPortalThickness);
    //    node1WestRenderTargetQuad.transform.localEulerAngles = new Vector3(0, 180, 0);
    //    // no need to set the scale, the correct scale is set in the parent node
    //    node1WestRenderTargetQuad.transform.SetParent(node1WestPlaceholder.transform, false);
    //    // disable the default collider
    //    node1WestRenderTargetQuad.GetComponent<MeshCollider>().enabled = false;

    //    _log.Trace($"created quad ({node1WestRenderTargetQuad.name}) at world {node1WestRenderTargetQuad.transform.position} {node1WestRenderTargetQuad.transform.eulerAngles}");
    //    _log.Trace($"created quad ({node1WestRenderTargetQuad.name}) at local {node1WestRenderTargetQuad.transform.localPosition} {node1WestRenderTargetQuad.transform.localEulerAngles}");

    //    // create a plane and add it to the west portal
    //    // this will become the collision detection plane

    //    var node1WestCollisionQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //    node1WestCollisionQuad.name = "Collision";
    //    node1WestCollisionQuad.transform.localPosition = new Vector3(0, 0, _halfPortalThickness);
    //    node1WestCollisionQuad.transform.localEulerAngles = new Vector3(0, 180, 0);
    //    // no need to set the scale, the correct scale is set in the parent node
    //    node1WestCollisionQuad.transform.SetParent(node1WestPlaceholder.transform, false);
    //    // disable the default collider
    //    node1WestCollisionQuad.GetComponent<MeshCollider>().enabled = false;

    //    // add a custom collidor and set the ConnectedPortal property
    //    var node1WestPortalCollider = node1WestCollisionQuad.AddComponent<PortalColliderScene6>();

    //    _log.Trace($"created quad ({node1WestPortalCollider.name}) at world {node1WestPortalCollider.transform.position} {node1WestPortalCollider.transform.eulerAngles}");
    //    _log.Trace($"created quad ({node1WestPortalCollider.name}) at local {node1WestPortalCollider.transform.localPosition} {node1WestPortalCollider.transform.localEulerAngles}");


    //    node0WestPortalCollider.ConnectedPortal = node1WestCollisionQuad;
    //    node1WestPortalCollider.ConnectedPortal = node0WestCollisionQuad;

    //    _log.Trace($"connected quad ({node0WestPortalCollider.name}) to  {node0WestPortalCollider.ConnectedPortal.name}");
    //    _log.Trace($"connected quad ({node1WestPortalCollider.name}) to  {node1WestPortalCollider.ConnectedPortal.name}");
    //}


}
