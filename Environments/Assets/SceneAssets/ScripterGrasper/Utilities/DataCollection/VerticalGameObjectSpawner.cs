using UnityEngine;

namespace SceneAssets.ScripterGrasper.Utilities.DataCollection {
  public class VerticalGameObjectSpawner : MonoBehaviour {
    [SerializeField]  GameObject _game_object;
    [SerializeField]  int _spawn_count = 10;

    public void SpawnGameObjectsVertically(
      GameObject game_object,
      Transform at_tranform,
      int count,
      float spacing = 0.5f) {
      var y = at_tranform.position.y;
      var new_position = new Vector3(
                                     x : at_tranform.position.x,
                                     y : y,
                                     z : at_tranform.position.z);
      for (var i = 0; i < count; i++) {
        new_position.y = y;
        var new_game_object = Instantiate(
                                          original : game_object,
                                          position : new_position,
                                          rotation : at_tranform.rotation);
        new_game_object.name = new_game_object.name + i;
        y += spacing;
      }
    }

    void Start() {
      this.SpawnGameObjectsVertically(
                                      game_object : this._game_object,
                                      at_tranform : this.transform,
                                      count : this._spawn_count);
    }
  }
}
