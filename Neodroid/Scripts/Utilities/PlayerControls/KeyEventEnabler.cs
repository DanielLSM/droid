using UnityEngine;

public class KeyEventEnabler : MonoBehaviour {

  public KeyCode _key;
  public GameObject _game_object;

  void Update () {
    if (Input.GetKeyDown (_key)) {
      _game_object.SetActive (!_game_object.activeSelf);
    }
  }
}
