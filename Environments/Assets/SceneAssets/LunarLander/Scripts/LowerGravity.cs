using UnityEngine;

public class LowerGravity : MonoBehaviour {
  // Use this for initialization
  private void Start() { Physics.gravity = Vector3.down * 3.33f; }

  // Update is called once per frame
  private void Update() { }
}
