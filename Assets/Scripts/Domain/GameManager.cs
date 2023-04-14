using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    private GameObject _rootGameObject;

    private GameObject _playerGameObject;

    private Camera _playerCamera;

    private PlayerControllerScene6 _playerController;

    private static readonly GameManager _instance = new GameManager();

    public bool LogUpdateCalls = false;

    public bool LogLateUpdateCalls = false;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static GameManager()
    {
    }

    private GameManager()
    {
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }





    public GameObject RootGameObject()
    {
        if (_rootGameObject == null)
        {
            _rootGameObject = GameObjectByTag("Root");
        }

        return _rootGameObject;
    }

    public GameObject PlayerGameObject()
    {
        if (_playerGameObject == null)
        {
            _playerGameObject = GameObjectByTag("Player");
        }

        return _playerGameObject;
    }

    public PlayerControllerScene6 PlayerController()
    {
        if (_playerController == null)
        {
            _playerController = PlayerGameObject().GetComponent<PlayerControllerScene6>();
        }

        return _playerController;
    }

    public Camera PlayerCamera()
    {
        if (_playerCamera == null)
        {
            _playerCamera = PlayerGameObject().GetComponentInChildren<Camera>();
        }

        return _playerCamera;
    }

    public GameObject GameObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    public GameObject[] GameObjectsByTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }

    public GameObject GameObjectByName(string name)
    {
        return GameObject.Find(name);
    }

    public IEnumerable<GameObject> GameObjectsByName(string name)
    {
        return GameObject.FindObjectsOfType<GameObject>().Where(o => o.name == name);
    }

    public TComponent ComponentByName<TComponent>(string gameObjectName) where TComponent : class
    {
        var gameObject = GameObjectByName(gameObjectName);

        if(gameObject == null)
        {
            return null;
        }

        return gameObject.GetComponent<TComponent>();
    }

    public IEnumerable<TComponent> ComponentsByName<TComponent>(string gameObjectName) where TComponent : class
    {
        var gameObjects = GameObjectsByName(gameObjectName);

        foreach(var gameObject in gameObjects)
        {
            var c = gameObject.GetComponent<TComponent>();

            if(c != null)
            {
                yield return c;
            }
        }
    }

    public GameObject GameObjectByName(GameObject parentGameObject, string name)
    {
        if (parentGameObject == null)
        {
            return null;
        }

        var transform = parentGameObject.transform.Find(name);

        if (transform != null)
        {
            return transform.gameObject;
        }

        foreach (Transform child in parentGameObject.transform)
        {
            var result = GameObjectByName(child.gameObject, name);

            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public TComponent ComponentByName<TComponent>(GameObject parentGameObject, string gameObjectName) where TComponent : class
    {
        var gameObject = GameObjectByName(parentGameObject, gameObjectName);

        if (gameObject == null)
        {
            return null;
        }

        return gameObject.GetComponent<TComponent>();
    }

    public IEnumerable<GameObject> GameObjects(GameObject parentGameObject, bool recursive = false)
    {
        foreach(Transform transform in parentGameObject.transform)
        {
            yield return transform.gameObject;

            if(recursive)
            {
                foreach (GameObject g in GameObjects(transform.gameObject, recursive))
                {
                    yield return g;
                }
            }
        }
    }

    public IEnumerable<TComponent> Components<TComponent>(GameObject parentGameObject, bool recursive = false) where TComponent : class
    {
        foreach (var gameObject in GameObjects(parentGameObject, recursive))
        {
            var c = gameObject.GetComponent<TComponent>();

            if (c != null)
            {
                yield return c;
            }
        }
    }

}