using System;
using UnityEngine;



namespace Neodroid.Environments {
  //public interface Resetable {
  [ExecuteInEditMode]
  public abstract class Resetable : MonoBehaviour {
    public abstract void Reset ();

    public abstract string ResetableIdentifier{ get; }
  }
}

