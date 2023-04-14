using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorStartup
{
    private static GameManager Game { get; set; } = GameManager.Instance;

    static EditorStartup()
    {
        if (Game.PortalScene == null)
        {
            Game.PortalScene = PortalSceneGenerator.GenerateDefaultPortalScene();
        }

        Game.PortalScene.CreatePortalMaterials();

        Game.PortalScene.SortRenderTargets();
    }
}
