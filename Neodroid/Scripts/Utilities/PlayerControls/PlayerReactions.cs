using System.Collections.Generic;
using Neodroid.Models.Managers;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.PlayerControls {
  public class PlayerReactions : MonoBehaviour {
    [SerializeField] PlayerMotions _player_motions;

    [SerializeField] SimulationManager _simulation_manager;

    [SerializeField] bool Debugging;

    void Start() { this._simulation_manager = FindObjectOfType<SimulationManager>(); }

    void Update() {
      if (this._player_motions != null) {
        var motions = new List<MotorMotion>();
        foreach (var player_motion in this._player_motions.Motions) {
          if (Input.GetKey(player_motion.Key)) {
            if (this.Debugging) {
              print(
                  string.Format(
                      "{0} {1} {2}",
                      player_motion.Actor,
                      player_motion.Motor,
                      player_motion.Strength));
            }

            var motion = new MotorMotion(player_motion.Actor, player_motion.Motor, player_motion.Strength);
            motions.Add(motion);
          }
        }

        var step = motions.Count > 0;
        var parameters = new ReactionParameters(true, step) {IsExternal = false};
        var reaction = new Reaction(parameters, motions.ToArray(), null, null);
        this._simulation_manager.ReactInEnvironments(reaction);
      } else {
        if (this.Debugging)
          print("No playermotions scriptable object assigned");
      }
    }
  }
}
