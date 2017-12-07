using Neodroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neodroid.Observers;
using Neodroid.Environments;

namespace Neodroid.Utilities {
  public class StatusUpdater : MonoBehaviour {

    public LearningEnvironment _environment;
    Text _status_text;

    // Use this for initialization
    void Start () {
      if (!_environment)
        _environment = FindObjectOfType<LearningEnvironment> ();
      _status_text = GetComponent<Text> ();
    }
	
    // Update is called once per frame
    void Update () {
      _status_text.text = _environment.GetStatus ();
    }
  }
}