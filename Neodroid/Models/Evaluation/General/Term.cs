
using UnityEngine;

namespace Neodroid.Evaluation {
  [System.Serializable]
  public abstract class Term : MonoBehaviour {
    //ScriptableObject {

    public abstract float Evaluate();
  }
}
