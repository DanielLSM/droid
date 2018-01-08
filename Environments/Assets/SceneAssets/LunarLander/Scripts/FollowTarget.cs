using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  [ExecuteInEditMode]
  public class FollowTarget : MonoBehaviour {
    public Vector3 offset = new Vector3(
                                        x : 0f,
                                        y : 7.5f,
                                        z : 0f);

    public Transform target;

    void LateUpdate() { this.transform.position = this.target.position + this.offset; }
  }
}
