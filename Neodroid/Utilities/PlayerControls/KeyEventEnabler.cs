using UnityEngine;

namespace Neodroid.Scripts.Utilities.PlayerControls {
  public class KeyEventEnabler : MonoBehaviour {
    [SerializeField] GameObject _game_object;

    [SerializeField] KeyCode _key;

    void Update() {
      if (Input.GetKeyDown(this._key)) this._game_object.SetActive(!this._game_object.activeSelf);
    }
  }
}
