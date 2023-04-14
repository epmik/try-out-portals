using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
//public class EditorStartup
//{
//    static EditorStartup()
//    {
//        EditorApplication.update += RunOnce;
//    }

//    static void RunOnce()
//    {
//        EditorApplication.update -= RunOnce;

//        if (GameManager.Instance.PortalScene == null)
//        {
//            GameManager.Instance.PortalScene = PortalSceneGenerator.GenerateDefaultPortalScene();
//        }

//        GameManager.Instance.PortalScene.CreatePortalMaterials();

//        GameManager.Instance.PortalScene.SortRenderTargets();
//    }
//}
