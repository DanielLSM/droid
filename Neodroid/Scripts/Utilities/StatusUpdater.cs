using Neodroid.Models.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Scripts.Utilities {
  public class StatusUpdater : MonoBehaviour {
    [SerializeField]
    SimulationManager _simulation_manager;
    [SerializeField]
    Text _status_text;

    // Use this for initialization
    void Start() {
      if (!this._simulation_manager) this._simulation_manager = FindObjectOfType<SimulationManager>();
      this._status_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
      if (this._simulation_manager) this._status_text.text = this._simulation_manager.GetStatus();
    }
  }
}
