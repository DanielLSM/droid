using System.Collections.Generic;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Managers;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.PlayerControls {
  public class PlayerReactions : MonoBehaviour {
    [SerializeField]  PlayerMotions _player_motions;
    [SerializeField]
    SimulationManager _simulation_manager;
    [SerializeField]  bool Debugging;

    void Start() { this._simulation_manager = FindObjectOfType<SimulationManager>(); }

    void Update() {
      if (this._player_motions != null) {
        var motions = new List<MotorMotion>();
        foreach (var player_motion in this._player_motions.Motions)
          if (Input.GetKey(key : player_motion.Key)) {
            if (this.Debugging)
              print(
                    message : string.Format(
                                            format : "{0} {1} {2}",
                                            arg0 : player_motion.Actor,
                                            arg1 : player_motion.Motor,
                                            arg2 : player_motion.Strength));
            var motion = new MotorMotion(
                                         actor_name : player_motion.Actor,
                                         motor_name : player_motion.Motor,
                                         strength : player_motion.Strength);
            motions.Add(item : motion);
          }

        var step = motions.Count > 0;
        var parameters = new ReactionParameters(
                                                terminable : true,
                                                step : step) {
                                                               IsExternal = false
                                                             };
        var reaction = new Reaction(
                                    parameters : parameters,
                                    motions : motions.ToArray(),
                                    configurations : null,
                                    unobservables : null);
        this._simulation_manager.ReactInEnvironments(reaction : reaction);
      } else {
        if (this.Debugging)
          print(message : "No playermotions scriptable object assigned");
      }
    }
  }
}
