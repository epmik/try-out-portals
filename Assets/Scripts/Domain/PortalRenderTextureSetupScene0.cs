using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRenderTextureSetupScene0 : MonoBehaviour
{
    public Camera Camera;

    public Material TargetMaterial;

    void Awake()
    {
        if (Camera.targetTexture != null)
        {
            Camera.targetTexture.Release();
            Camera.targetTexture = null;
        }

        TargetMaterial.mainTexture = Camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
    }
}
