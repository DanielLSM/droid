using UnityEngine;

namespace UnityStandardAssets.Utility {
  public class TimedObjectDestructor : MonoBehaviour {
    [SerializeField] bool m_DetachChildren;

    [SerializeField] float m_TimeOut = 1.0f;

    void Awake() {
      this.Invoke(
                  methodName : "DestroyNow",
                  time : this.m_TimeOut);
    }

    void DestroyNow() {
      if (this.m_DetachChildren) this.transform.DetachChildren();
      DestroyObject(obj : this.gameObject);
    }
  }
}
