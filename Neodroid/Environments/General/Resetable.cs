using Neodroid.Models.Environments;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Environments.General {
  //public interface Resetable {
  [ExecuteInEditMode]
  public abstract class Resetable : MonoBehaviour {
    public PrototypingEnvironment ParentEnvironment;

    public abstract string ResetableIdentifier { get; }

    public abstract void Reset();

    protected virtual void Awake() { this.RegisterComponent(); }

    protected virtual void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }
  }
}
