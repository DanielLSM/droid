using UnityEngine;

namespace Neodroid.Utilities {
  public class ChildSensor : MonoBehaviour {
    //public GameObject Caller{ get { return _caller; } set { _caller = value; } }

    public delegate void OnChildCollisionEnterDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildCollisionExitDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildCollisionStayDelegate(GameObject child_game_object, Collision collision);

    public delegate void OnChildTriggerEnterDelegate(GameObject child_game_object, Collider collider);

    public delegate void OnChildTriggerExitDelegate(GameObject child_game_object, Collider collider);

    public delegate void OnChildTriggerStayDelegate(GameObject child_game_object, Collider collider);

    [SerializeField]
    public Component _caller;

    private OnChildCollisionEnterDelegate _on_collision_enter_delegate;

    private OnChildCollisionExitDelegate _on_collision_exit_delegate;

    private OnChildCollisionStayDelegate _on_collision_stay_delegate;

    private OnChildTriggerEnterDelegate _on_trigger_enter_delegate;

    private OnChildTriggerExitDelegate _on_trigger_exit_delegate;

    private OnChildTriggerStayDelegate _on_trigger_stay_delegate;

    public OnChildCollisionEnterDelegate OnCollisionEnterDelegate {
      set { _on_collision_enter_delegate = value; }
    }

    public OnChildTriggerEnterDelegate OnTriggerEnterDelegate { set { _on_trigger_enter_delegate = value; } }

    public OnChildTriggerStayDelegate OnTriggerStayDelegate { set { _on_trigger_stay_delegate = value; } }

    public OnChildCollisionStayDelegate OnCollisionStayDelegate {
      set { _on_collision_stay_delegate = value; }
    }

    public OnChildCollisionExitDelegate OnCollisionExitDelegate {
      set { _on_collision_exit_delegate = value; }
    }

    public OnChildTriggerExitDelegate OnTriggerExitDelegate { set { _on_trigger_exit_delegate = value; } }

    private void OnCollisionEnter(Collision collision) {
      if (_on_collision_enter_delegate != null)
        _on_collision_enter_delegate(
                                     gameObject,
                                     collision);
    }

    private void OnTriggerEnter(Collider other) {
      if (_on_trigger_enter_delegate != null)
        _on_trigger_enter_delegate(
                                   gameObject,
                                   other);
    }

    private void OnTriggerStay(Collider other) {
      if (_on_trigger_stay_delegate != null)
        _on_trigger_stay_delegate(
                                  gameObject,
                                  other);
    }

    private void OnCollisionStay(Collision collision) {
      if (_on_collision_stay_delegate != null)
        _on_collision_stay_delegate(
                                    gameObject,
                                    collision);
    }

    private void OnTriggerExit(Collider other) {
      if (_on_trigger_exit_delegate != null)
        _on_trigger_exit_delegate(
                                  gameObject,
                                  other);
    }

    private void OnCollisionExit(Collision collision) {
      if (_on_collision_exit_delegate != null)
        _on_collision_exit_delegate(
                                    gameObject,
                                    collision);
    }
  }
}
