using UnityEngine;

public class KeyEventEnabler : MonoBehaviour {
  public GameObject _game_object;

  public KeyCode _key;

  private void Update() {
    if (Input.GetKeyDown(_key)) _game_object.SetActive(!_game_object.activeSelf);
  }
}
