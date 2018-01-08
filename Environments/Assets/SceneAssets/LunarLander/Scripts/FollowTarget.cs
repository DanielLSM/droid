using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  [ExecuteInEditMode]
  public class FollowTarget : MonoBehaviour {
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);

    public Transform target;

    void LateUpdate() { this.transform.position = this.target.position + this.offset; }
  }
}
