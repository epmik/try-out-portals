using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMaterialController : MonoBehaviour
{
    //private Renderer _renderer;

    //private Material _material;

    //private void Awake()
    //{
    //    _renderer = GetComponent<Renderer>();

    //    _material = _renderer.GetComponent<Material>();

    //    if (_material == null)
    //    {
    //        _material = new Material(Resources.Load<Shader>("Portal Shader"));
    //    }

    //    if (_material.shader.name != "Portal Shader")
    //    {
    //        _material.shader = Resources.Load<Shader>("Portal Shader");
    //    }

    //    _renderer.material = _material;
    //}

    public Material InitializeMaterial()
    {
        var renderer = GetComponent<Renderer>();

        var material = renderer.GetComponent<Material>();

        if (material == null)
        {
            material = new Material(Resources.Load<Shader>("Portal Render Target Shader"));
        }

        if (material.shader.name != "Private/Portal Render Target Shader")
        {
            material.shader = Resources.Load<Shader>("Private/Portal Render Target Shader");
        }

        renderer.material = material;

        return material;
    }
}
