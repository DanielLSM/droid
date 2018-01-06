using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;


namespace Neodroid.Utilities {
  public class PlayerReactions : MonoBehaviour {

    public PlayerMotions _player_motions;
    public bool Debugging = false;

    SimulationManager _simulation_manager;

    void Start () {
      _simulation_manager = FindObjectOfType<SimulationManager> ();
    }

    void Update () {
      if (_player_motions != null) {

        List<MotorMotion> motions = new List<MotorMotion> ();
        foreach (var player_motion in _player_motions._player_motions) {
          if (Input.GetKey (player_motion.key)) {
            if (Debugging)
              print (System.String.Format ("{0} {1} {2}", player_motion.actor, player_motion.motor, player_motion.strength));
            var motion = new MotorMotion (player_motion.actor, player_motion.motor, player_motion.strength);
            motions.Add (motion);
          }
        }
        var step = false;
        if (motions.Count > 0)
          step = true;
        var parameters = new ReactionParameters (true, step);
        parameters.BeforeObservation = false;
        var reaction = new Reaction (parameters, motions.ToArray (), null, null);
        _simulation_manager.ReactInEnvironments (reaction);
      } else {
        if (Debugging)
          print ("No playermotions scriptable object assigned");
      }
    }
  }
}