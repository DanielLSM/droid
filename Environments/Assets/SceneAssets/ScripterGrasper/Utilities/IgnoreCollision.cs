using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities {
  public class IgnoreCollision : MonoBehaviour {
    // Use this for initialization
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    private void OnCollisionEnter(Collision collision) {
      if (collision.gameObject.tag == "ignored_by_sub_collider_fish")
        Physics.IgnoreCollision(
                                GetComponent<Collider>(),
                                collision.collider);
    }

    private void OnCollisionExit(Collision collision) {
      if (collision.gameObject.tag == "ignored_by_sub_collider_fish")
        Physics.IgnoreCollision(
                                GetComponent<Collider>(),
                                collision.collider);
    }

    private void OnCollisionStay(Collision collision) {
      if (collision.gameObject.tag == "ignored_by_sub_collider_fish")
        Physics.IgnoreCollision(
                                GetComponent<Collider>(),
                                collision.collider);
    }
  }
}
