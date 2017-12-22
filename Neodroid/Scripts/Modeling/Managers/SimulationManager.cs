using System.Collections;
using System.Collections.Generic;
using Neodroid.Configurables;
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
    public bool _continue_reaction_on_disconnect = false;
    public int _episode_length = 1000;
    public int _frame_skips = 0;
    public float _simulation_time_scale = 1;
    public int _resets = 10;
    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects
    public bool _wait_for_reaction_every_frame = false;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Dictionary<string, LearningEnvironment> _environments = new Dictionary<string, LearningEnvironment> ();
    MessageServer _message_server;

    bool _is_simulation_updated = false;
    bool _client_connected = false;
    Reaction _reaction = null;
    bool _waiting_for_reaction = true;

    #endregion

    #region UnityCallbacks

    void Start () {
      FetchCommmandLineArguments ();
      StartMessagingServer ();
      ResumeSimulation ();
    }

    void FixedUpdate () {
      if (_wait_for_reaction_every_frame) {
        PauseSimulation ();
      }
    }

    void LateUpdate () {
      if (!_is_simulation_updated) {
        _is_simulation_updated = true;
      }
      PostUpdate ();
    }

    void Update () {
      if (!_waiting_for_reaction && _reaction != null) {
        ResumeSimulation ();

        var states = Step (_reaction);
        SendEnvironmentStates (states);
        _waiting_for_reaction = true;
      }
      if (!_wait_for_reaction_every_frame) {
        ResumeSimulation ();
      }
    }

    #endregion

    #region PublicMethods

    public EnvironmentState[] Step (Reaction reaction) {
      foreach (var environment in _environments.Values) {
        environment.ExecuteReaction (reaction);
      }
      return GatherStates ();
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


    public bool WaitForReaction {
      get{ return _waiting_for_reaction; }
      set {
        _wait_for_reaction_every_frame = value;
      }
    }

    #region Registration

    public void Register (LearningEnvironment environment) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, environment.EnvironmentIdentifier));
      _environments.Add (environment.EnvironmentIdentifier, environment);
    }

    public void Register (LearningEnvironment environment, string identifier) {
      if (_debug)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, identifier));
      _environments.Add (identifier, environment);
    }

    #endregion

    #endregion

    #region PrivateMethods

    void SendEnvironmentStates (EnvironmentState[] states) {
      _message_server.SendEnvironmentStates (states);
    }

    void PostUpdate () {
      foreach (var environment in _environments.Values) {
        environment.PostUpdate ();
      }
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
      Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    void ResumeSimulation () {
      if (_simulation_time_scale > 0) {
        Time.timeScale = _simulation_time_scale;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
      } else {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
      }
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
      _reaction = reaction;
      _waiting_for_reaction = false;
    }

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
