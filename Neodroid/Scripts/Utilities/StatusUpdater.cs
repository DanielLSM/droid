using Neodroid.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Utilities {
  public class StatusUpdater : MonoBehaviour {
    public SimulationManager _simulation_manager;
    private Text _status_text;

    // Use this for initialization
    private void Start() {
      if (!_simulation_manager)
        _simulation_manager = FindObjectOfType<SimulationManager>();
      _status_text = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update() {
      if (_simulation_manager) _status_text.text = _simulation_manager.GetStatus();
    }
  }
}
