using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using System;

namespace Neodroid.Utilities {
  public class PlayerReactions : MonoBehaviour {

    public PlayerMotions _player_motions;
    public bool _debug = true;

    SimulationManager _simulation_manager;

    void Start () {
      _simulation_manager = FindObjectOfType<SimulationManager> ();
    }

    void Update () {
      if (_player_motions != null) {

        List<MotorMotion> motions = new List<MotorMotion> ();
        foreach (var player_motion in _player_motions._player_motions) {
          if (Input.GetKey (player_motion.key)) {
            if (_debug)
              print (String.Format ("{0}{1}{2}", player_motion.actor, player_motion.motor, player_motion.strength));
            var motion = new MotorMotion (player_motion.actor, player_motion.motor, player_motion.strength);
            motions.Add (motion);
          }
        }

        var reaction = new Reaction (motions.ToArray (), null, false);
        _simulation_manager.ExecuteReaction (reaction);
      } else {
        if (_debug)
          print ("No playermotions scriptable object assigned");
      }
    }
  }
}