using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : CommonBehaviour
{
    private PortalScene _portalScene;

    private bool _traceLogLevelIsEnabled;

    private int TargetFrameRate = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
    }

    void Start()
    {
        GeneratePortalScene();
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
            GeneratePortalScene();
        }
    }

    void LateUpdate()
    {

    }

    private void GeneratePortalScene()
    {
        if (_portalScene != null)
        {
            _portalScene.Destroy();
            _portalScene = null;
        }

        var portalsSceneGenerator = new PortalSceneGenerator();

        _portalScene = portalsSceneGenerator.Generate();
    }
}
