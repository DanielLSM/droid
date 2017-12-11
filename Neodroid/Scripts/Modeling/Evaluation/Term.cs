using System;
using UnityEngine;

namespace Neodroid.Evaluation {
  [Serializable]
  public abstract class Term: MonoBehaviour {
    //ScriptableObject {


    public abstract float evaluate ();
  }
}