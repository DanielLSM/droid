using System;
using UnityEngine;

namespace Neodroid.Models.Evaluation {
  [Serializable]
  public abstract class Term : MonoBehaviour {
    public abstract float Evaluate();
  }
}
