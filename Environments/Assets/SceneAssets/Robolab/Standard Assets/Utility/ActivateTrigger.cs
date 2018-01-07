
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityStandardAssets.Utility {
  public class ActivateTrigger : MonoBehaviour {
    // A multi-purpose script which causes an action to occur when
    // a trigger collider is entered.
    public enum Mode {
      Trigger = 0,
      // Just broadcast the action on to the target
      Replace = 1,
      // replace target with source
      Activate = 2,
      // Activate the target GameObject
      Enable = 3,
      // Enable a component
      Animate = 4,
      // Start animation on target
      Deactivate = 5
      // Decativate target GameObject
    }

    public Mode action = Mode.Activate;
    // The action to accomplish
    public bool repeatTrigger;
    public GameObject source;
    public Object target;
    // The game object to affect. If none, the trigger work on this game object
    public int triggerCount = 1;

    private void DoActivateTrigger () {
      triggerCount--;

      if (triggerCount == 0 || repeatTrigger) {
        var current_target = target ?? gameObject;
        var target_behaviour = current_target as Behaviour;
        var target_game_object = current_target as GameObject;
        if (target_behaviour != null)
          target_game_object = target_behaviour.gameObject;

        switch (action) {
        case Mode.Trigger:
          if (target_game_object != null)
            target_game_object.BroadcastMessage ("DoActivateTrigger");
          break;
        case Mode.Replace:
          if (source != null)
          if (target_game_object != null) {
            Instantiate (
              source,
              target_game_object.transform.position,
              target_game_object.transform.rotation);
            DestroyObject (target_game_object);
          }

          break;
        case Mode.Activate:
          if (target_game_object != null)
            target_game_object.SetActive (true);
          break;
        case Mode.Enable:
          if (target_behaviour != null)
            target_behaviour.enabled = true;
          break;
        case Mode.Animate:
          if (target_game_object != null)
            target_game_object.GetComponent<Animation> ().Play ();
          break;
        case Mode.Deactivate:
          if (target_game_object != null)
            target_game_object.SetActive (false);
          break;
        default:
          throw new System.ArgumentOutOfRangeException ();
        }
      }
    }

    private void OnTriggerEnter (Collider other) {
      DoActivateTrigger ();
    }
  }
}
