using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;

public class PlayerReactions : MonoBehaviour {

  [System.Serializable]
  public struct PlayerMotion {
    public KeyCode key;
    public string actor;
    public string motor;
    public float strength;
  }


  public PlayerMotion[] _player_motions;

  SimulationManager _simulation_manager;

  void Start () {
    _simulation_manager = FindObjectOfType<SimulationManager> ();
  }

  void Update () {
    List<MotorMotion> motions = new List<MotorMotion> ();
    foreach (var player_motion in _player_motions) {
      if (Input.GetKey (player_motion.key)) {
        var motion = new MotorMotion (player_motion.actor, player_motion.motor, player_motion.strength);
        motions.Add (motion);
      }
    }

    var reaction = new Reaction (motions.ToArray (), null, false);
    _simulation_manager.OnReceiveCallback (reaction);
  }


}
