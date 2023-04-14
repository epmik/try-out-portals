using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        Debug.Log("Editor Loaded");

        if(GameManager.Instance.PortalScene == null)
        {
            //GameManager.Instance.GeneratePortalScene();
        }
    }
}