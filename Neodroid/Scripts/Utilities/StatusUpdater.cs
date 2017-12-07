using Neodroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neodroid.Observers;
using Neodroid.Managers;

namespace Neodroid.Utilities {
  public class StatusUpdater : MonoBehaviour {

    public SimulationManager _simulation_manager;
    Text _status_text;

    // Use this for initialization
    void Start () {
      if (!_simulation_manager)
        _simulation_manager = FindObjectOfType<SimulationManager> ();
      _status_text = GetComponent<Text> ();
    }
	
    // Update is called once per frame
    void Update () {
      _status_text.text = _simulation_manager.GetStatus ();
    }
  }
}