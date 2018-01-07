using System.Collections.Generic;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Configurables {
  public class ObjectSpawnerConfigurable : ConfigurableGameObject {
    public int _amount;
    public Axis _axis;
    public GameObject _object_to_spawn;

    private List<GameObject> _spawned_objects;

    protected override void Start() {
      DestroyObjects();
      _spawned_objects = new List<GameObject>();
      SpawnObjects();
    }

    private void DestroyObjects() {
      if (_spawned_objects != null)
        foreach (var o in _spawned_objects)
          Destroy(o);
      foreach (Transform c in transform) Destroy(c.gameObject);
    }

    private void SpawnObjects() {
      if (_object_to_spawn) {
        var dir = Vector3.up;
        if (_axis == Axis.X)
          dir = Vector3.right;
        else if (_axis == Axis.Z)
          dir = Vector3.forward;
        for (var i = 0; i < _amount; i++)
          _spawned_objects.Add(
                               Instantiate(
                                           _object_to_spawn,
                                           transform.position + dir * i,
                                           Random.rotation,
                                           transform));
      }
    }

    private void OnApplicationQuit() { DestroyObjects(); }
  }
}
