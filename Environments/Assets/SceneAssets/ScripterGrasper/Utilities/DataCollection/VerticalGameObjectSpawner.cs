﻿using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities.DataCollection {
  public class VerticalGameObjectSpawner : MonoBehaviour {
    public GameObject _game_object;
    public int _spawn_count = 10;

    public void SpawnGameObjectsVertically(
      GameObject game_object,
      Transform at_tranform,
      int count,
      float spacing = 0.5f) {
      var y = at_tranform.position.y;
      var new_position = new Vector3(
                                     at_tranform.position.x,
                                     y,
                                     at_tranform.position.z);
      for (var i = 0; i < count; i++) {
        new_position.y = y;
        var new_game_object = Instantiate(
                                          game_object,
                                          new_position,
                                          at_tranform.rotation);
        new_game_object.name = new_game_object.name + i;
        y += spacing;
      }
    }

    private void Start() {
      SpawnGameObjectsVertically(
                                 _game_object,
                                 transform,
                                 _spawn_count);
    }
  }
}
