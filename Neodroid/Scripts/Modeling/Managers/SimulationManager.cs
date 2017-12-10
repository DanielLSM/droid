using System.Collections;
using System.Collections.Generic;
using Neodroid.Configurations;
using Neodroid.Environments;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;
using System;

namespace Neodroid.Managers {
  public class SimulationManager : MonoBehaviour, HasRegister<LearningEnvironment> {

    #region PublicMembers

    public string _ip_address = "127.0.0.1";
    public int _port = 5555;
    public bool _continue_lastest_reaction_on_disconnect = false;
    public int _episode_length = 1000;
    public int _frame_skips = 0;
    public int _resets = 6;
    public bool _wait_for_reaction_every_frame = false;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Dictionary<string, LearningEnvironment> _environments = new Dictionary<string, LearningEnvironment> ();
    MessageServer _message_server;

    bool _is_simulation_updated = false;
    bool _client_connected = false;
    Reaction _lastest_reaction = null;
    bool _waiting_for_reaction = true;

    #endregion

    #region UnityCallbacks

    void Start () {
      FetchCommmandLineArguments ();
      StartMessagingServer ();
    }

    void FixedUpdate () {
      if (_wait_for_reaction_every_frame) {
        PauseSimulation ();
      }
    }

    void LateUpdate () {
      _is_simulation_updated = true;
    }

    void Update () {
      if (!_waiting_for_reaction && _lastest_reaction != null && !_is_simulation_updated) {
        ResumeSimulation ();

        ExecuteReaction (_lastest_reaction);
        SendEnvironmentStates (GatherStates ());
      }
    }

    #endregion

    #region PublicMethods

    public LearningEnvironment ExecuteReaction (Reaction reaction) {
      LearningEnvironment last = null;
      foreach (var environment in _environments.Values) {
        environment.ExecuteReaction (reaction);
        environment.UpdateObserversData ();
        last = environment;
      }
      return last;
    }

    public bool IsSimulationUpdated () {
      return _is_simulation_updated;
    }

    public bool IsSimulationPaused () {
      return Time.timeScale == 0;
    }

    public string GetStatus () {
      if (_client_connected)
        return "Connected";
      else
        return "Not Connected";
    }

    #region Registration

    public void Register (LearningEnvironment environment) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, environment.GetEnvironmentIdentifier ()));
      _environments.Add (environment.GetEnvironmentIdentifier (), environment);
    }

    public void Register (LearningEnvironment environment, string identifier) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, identifier));
      _environments.Add (identifier, environment);
    }

    #endregion

    #endregion

    #region PrivateMethods

    void SendEnvironmentState (EnvironmentState state) {
      _message_server.SendEnvironmentState (state);
    }

    void SendEnvironmentStates (EnvironmentState[] states) {
      _message_server.SendEnvironmentStates (states);
    }

    EnvironmentState[] GatherStates () {
      var states = new EnvironmentState[_environments.Values.Count];
      var i = 0;
      foreach (var environment in _environments.Values) {
        states [i++] = environment.GetCurrentState ();
      }
      return states;
    }


    void PauseSimulation () {
      Time.timeScale = 0;
    }

    void ResumeSimulation () {
      Time.timeScale = 1;
      _is_simulation_updated = false;
      _waiting_for_reaction = true;
    }

    void FetchCommmandLineArguments () {
      string[] arguments = System.Environment.GetCommandLineArgs ();

      for (int i = 0; i < arguments.Length; i++) {
        if (arguments [i] == "-ip") {
          _ip_address = arguments [i + 1];
        }
        if (arguments [i] == "-port") {
          _port = int.Parse (arguments [i + 1]);
        }
      }
    }

    void StartMessagingServer () {
      if (_ip_address != "" || _port != 0)
        _message_server = new MessageServer (_ip_address, _port);
      else
        _message_server = new MessageServer ();

      _message_server.ListenForClientToConnect (OnConnectCallback);
    }


    #endregion

    #region Callbacks

    void OnReceiveCallback (Reaction reaction) {
      _client_connected = true;
      if (_debug)
        Debug.Log ("Received: " + reaction.ToString ());
      _lastest_reaction = reaction;
      _waiting_for_reaction = false;
    }

    /*void OnResetCallback (EnvironmentConfiguration configuration) {
          _client_connected = true;
          if (_debug)
            Debug.Log ("Received: " + reaction.ToString ());
          _lastest_reaction = reaction;
          _waiting_for_reaction = false;
          _has_stepped_since_reaction = false;
        }*/

    void OnDisconnectCallback () {
      _client_connected = false;
      if (_debug)
        Debug.Log ("Client disconnected.");
    }

    void OnErrorCallback (string error) {
      if (_debug)
        Debug.Log ("ErrorCallback: " + error);
    }

    void OnConnectCallback () {
      if (_debug)
        Debug.Log ("Client connected.");
      _message_server.StartReceiving (OnReceiveCallback, OnDisconnectCallback, OnErrorCallback);
    }

    void OnInterruptCallback () {

    }

    #endregion

    #region Deconstruction

    private void OnApplicationQuit () {
      _message_server.KillPollingAndListenerThread ();
    }

    private void OnDestroy () { //Deconstructor
      _message_server.Destroy ();
    }

    #endregion
  }
}
