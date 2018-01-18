using Neodroid.Managers;
using Neodroid.Models.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Scripts.Utilities {
  public class StatusUpdater : MonoBehaviour {
    [SerializeField] Text _status_text;
    [SerializeField] PausableManager _time_simulation_manager;

    // Use this for initialization
    void Start() {
      if (!this._time_simulation_manager)
        this._time_simulation_manager = FindObjectOfType<PausableManager>();
      this._status_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
      if (this._time_simulation_manager) this._status_text.text = this._time_simulation_manager.GetStatus();
    }
  }
}
