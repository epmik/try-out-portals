using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RootController : CommonBehaviour
{
    private bool _traceLogLevelIsEnabled;

    private int TargetFrameRate = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
    }

    void Start()
    {
        if(Game.PortalScene == null)
        {
            Game.PortalScene = PortalSceneGenerator.GenerateDefaultPortalScene();
        }

        Game.PortalScene.CreatePortalMaterials();

        Game.PortalScene.SortRenderTargets();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F12))
        {
            if(_traceLogLevelIsEnabled)
            {
                LogManager.Instance.EnableLogLevel(LogManager.LogLevel.Error);
            }
            else
            {
                LogManager.Instance.EnableLogLevel(LogManager.LogLevel.Trace);
            }
            _traceLogLevelIsEnabled = !_traceLogLevelIsEnabled;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if(Game.PortalScene != null)
            {
                Game.PortalScene.Destroy();
                Game.PortalScene = null;
            }

            Game.PortalScene = PortalSceneGenerator.GenerateDefaultPortalScene();
        }

        Game.PortalScene.SortRenderTargets();
    }

    void LateUpdate()
    {

    }
}
