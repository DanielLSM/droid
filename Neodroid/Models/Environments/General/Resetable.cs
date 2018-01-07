using UnityEngine;

namespace Neodroid.Environments {
  //public interface Resetable {
  [ExecuteInEditMode]
  public abstract class Resetable : MonoBehaviour {
    public abstract string ResetableIdentifier { get; }
    public abstract void Reset();
  }
}
