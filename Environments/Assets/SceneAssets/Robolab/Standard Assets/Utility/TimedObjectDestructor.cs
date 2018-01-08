using UnityEngine;

namespace UnityStandardAssets.Utility {
  public class TimedObjectDestructor : MonoBehaviour {
    [SerializeField] bool m_DetachChildren;

    [SerializeField] float m_TimeOut = 1.0f;

    void Awake() { this.Invoke("DestroyNow", this.m_TimeOut); }

    void DestroyNow() {
      if (this.m_DetachChildren) this.transform.DetachChildren();
      DestroyObject(this.gameObject);
    }
  }
}
