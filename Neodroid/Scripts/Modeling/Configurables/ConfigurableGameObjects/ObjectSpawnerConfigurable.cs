using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Configurables {

  public class ObjectSpawnerConfigurable : ConfigurableGameObject {
    public GameObject _object_to_spawn;
    public int _amount;
    public Axis _axis;

    List<GameObject> _spawned_objects;

    protected override void Start () {
      DestroyObjects ();
      _spawned_objects = new List<GameObject> ();
      SpawnObjects ();
    }

    void DestroyObjects () {
      if (_spawned_objects != null) {
        foreach (var o in _spawned_objects) {
          Destroy (o);
        }
      }
      foreach (Transform c in transform) {
        Destroy (c.gameObject);
      }
    }

    void SpawnObjects () {
      if (_object_to_spawn) {
        var dir = Vector3.up;
        if (_axis == Axis.X) {
          dir = Vector3.right;
        } else if (_axis == Axis.Z) {
          dir = Vector3.forward;
        }
        for (var i = 0; i < _amount; i++) {
          _spawned_objects.Add (Instantiate (_object_to_spawn, this.transform.position + (dir * i), Random.rotation, this.transform));
        }
      }
    }

    void OnApplicationQuit () {
      DestroyObjects ();
    }
  }
}