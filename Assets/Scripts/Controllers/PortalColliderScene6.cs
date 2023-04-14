using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalColliderScene6 : CommonBehaviour
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
        if(!TriggerIsActive)
        {
            return;
        }

        var position = Game.PlayerGameObject().transform.position;

        // TODO pre cull if the position is to far from the quad

        var direction = position - _lastPlayerPosition;

        Log.TraceChange(position, $"Game.PlayerGameObject().transform.position: {position}", "Game.PlayerGameObject().transform.position");
        Log.TraceChange(_lastPlayerPosition, $"LastPlayerPosition: {_lastPlayerPosition}", "LastPlayerPosition");

        if (direction.magnitude == 0)
        {
            return;
        }

        //if(position.x <= 28.10f && _lastPlayerPosition.x >= 28.10f)
        //{
        //    var t = 0;
        //}

        if (Intersection.QuadVectorIntersection(gameObject, _lastPlayerPosition, direction))
        {
            Log.Trace($"Intersects(gameObject, LastPlayerPosition, direction) was true");

            Debug.Log($"Jump! (Frame: {Time.frameCount})");
            Log.Trace($"Jump! (Frame: {Time.frameCount})");

            Log.Trace($"Game.PlayerGameObject().transform.position: {position}");
            Log.Trace($"LastPlayerPosition: {_lastPlayerPosition}");
            Log.Trace($"direction: {direction}");
            Log.Trace($"direction.magnitude: {direction.magnitude}");

            if (ConnectedPortal != null)
            {
                Game.PlayerController().TeleportFromToPortal(gameObject, ConnectedPortal);

                SyncLastPlayerPositionForAllPortal(Game.PlayerGameObject().transform.position);
            }
        }

        _lastPlayerPosition = Game.PlayerGameObject().transform.position;

        Log.TraceChange(Game.PlayerGameObject().transform.position, $"Game.PlayerGameObject().transform.position: {Game.PlayerGameObject().transform.position}", "Game.PlayerGameObject().transform.position");
    }

    private void SyncLastPlayerPositionForAllPortal(Vector3 position)
    {
        Log.Trace($"SyncLastPlayerPositionForAllPortal({position}) (Frame: {Time.frameCount})");

        var portalColliders = Game.ComponentsByName<PortalColliderScene6>("Collider");

        foreach(var portalCollider in portalColliders)
        {
            portalCollider._lastPlayerPosition = position;
        }
    }
}
