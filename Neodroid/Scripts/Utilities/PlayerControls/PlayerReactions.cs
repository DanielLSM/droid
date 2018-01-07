using System.Collections.Generic;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Utilities {
  public class PlayerReactions : MonoBehaviour {
    public PlayerMotions _player_motions;

    private SimulationManager _simulation_manager;
    public bool Debugging;

    private void Start() { _simulation_manager = FindObjectOfType<SimulationManager>(); }

    private void Update() {
      if (_player_motions != null) {
        var motions = new List<MotorMotion>();
        foreach (var player_motion in _player_motions._player_motions)
          if (Input.GetKey(player_motion.key)) {
            if (Debugging)
              print(
                    string.Format(
                                  "{0} {1} {2}",
                                  player_motion.actor,
                                  player_motion.motor,
                                  player_motion.strength));
            var motion = new MotorMotion(
                                         player_motion.actor,
                                         player_motion.motor,
                                         player_motion.strength);
            motions.Add(motion);
          }

        var step = motions.Count > 0;
        var parameters = new ReactionParameters(
                                                true,
                                                step) {
                                                        IsExternal = false
                                                      };
        var reaction = new Reaction(
                                    parameters,
                                    motions.ToArray(),
                                    null,
                                    null);
        _simulation_manager.ReactInEnvironments(reaction);
      } else {
        if (Debugging)
          print("No playermotions scriptable object assigned");
      }
    }
  }
}
