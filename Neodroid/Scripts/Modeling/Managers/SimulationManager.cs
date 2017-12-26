using System.Collections;
using System.Collections.Generic;
using Neodroid.Configurables;
using Neodroid.Environments;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Neodroid.Managers {
  public class SimulationManager : MonoBehaviour, HasRegister<LearningEnvironment> {

    #region Fields

    [SerializeField]
    string _ip_address = "127.0.0.1";
    [SerializeField]
    int _port = 5555;
    [SerializeField]
    bool _continue_reaction_on_disconnect = false;
    [SerializeField]
    int _episode_length = 1000;
    [SerializeField]
    int _frame_skips = 0;
    [SerializeField]
    float _simulation_time_scale = 1;
    [SerializeField]
    int _resets = 10;
    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects
    [SerializeField]
    bool _wait_for_reaction_every_frame = false;
    [SerializeField]
    bool _debugging = false;

    #endregion

    #region Getters

    public string IpAddress {
      get {
        return _ip_address;
      }
      set { 
        _ip_address = value;
      }
    }

    public int Port {
      get {
        return _port;
      }
      set { 
        _port = value;
      }
    }

    public bool ContinueReactionOnDisconnect {
      get {
        return _continue_reaction_on_disconnect;
      }
      set { 
        _continue_reaction_on_disconnect = value;
      }
    }

    public int EpisodeLength {
      get {
        return _episode_length;
      }
      set { 
        _episode_length = value;
      }
    }

    public int FrameSkips {
      get {
        return _frame_skips;
      }
      set { 
        _frame_skips = value;
      }
    }

    public float SimulationTimeScale {
      get {
        return _simulation_time_scale;
      }
      set { 
        _simulation_time_scale = value;
      }
    }

    public int Resets {
      get {
        return _resets;
      }
      set { 
        _resets = value;
      }
    }

    public bool WaitForReactionEveryFrame {
      get {
        return _wait_for_reaction_every_frame;
      }
      set { 
        _wait_for_reaction_every_frame = value;
      }
    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set { 
        _debugging = value;
      }
    }

    public bool IsSimulationUpdated () {
      return _is_simulation_updated;
    }

    public bool IsSimulationPaused () {
      return Time.timeScale == 0;
    }

    public bool WaitForReaction {
      get{ return _waiting_for_reaction; }
      set {
        _wait_for_reaction_every_frame = value;
      }
    }

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
      if (!_wait_for_reaction_every_frame || (_reaction != null && _reaction.Step)) {
        ResumeSimulation (_simulation_time_scale);
      }
      if (!_waiting_for_reaction) {
        var states = ReactInEnvironments (_reaction);
        SendEnvironmentStates (states);
        _waiting_for_reaction = true;
      }
    }

    #endregion

    #region PublicMethods

    public EnvironmentState[] ReactInEnvironments (Reaction reaction) {
      var states = new EnvironmentState[_environments.Values.Count];
      var i = 0;
      foreach (var environment in _environments.Values) {
        states [i++] = environment.React (reaction);
      }
      return states;
    }



    public string GetStatus () {
      if (_client_connected)
        return "Connected";
      else
        return "Not Connected";
    }

    #region Registration

    public void Register (LearningEnvironment environment) {
      if (Debugging)
        Debug.Log (string.Format ("Manager {0} has environment {1}", name, environment.EnvironmentIdentifier));
      _environments.Add (environment.EnvironmentIdentifier, environment);
    }

    public void Register (LearningEnvironment environment, string identifier) {
      if (Debugging)
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


    void PauseSimulation () {
      Time.timeScale = 0;
      Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    void ResumeSimulation (float simulation_time_scale) {
      if (simulation_time_scale > 0) {
        Time.timeScale = simulation_time_scale;
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
        _message_server = new MessageServer (_ip_address, _port, false, Debugging);
      else
        _message_server = new MessageServer (Debugging);

      _message_server.ListenForClientToConnect (OnConnectCallback);
    }


    #endregion

    #region Callbacks

    void OnReceiveCallback (Reaction reaction) {
      _client_connected = true;
      if (Debugging)
        Debug.Log ("Received: " + reaction.ToString ());
      _reaction = reaction;
      _waiting_for_reaction = false;
    }

    void OnDisconnectCallback () {
      _client_connected = false;
      if (Debugging)
        Debug.Log ("Client disconnected.");
    }

    void OnDebugCallback (string error) {
      if (Debugging)
        Debug.Log ("DebugCallback: " + error);
    }

    void OnConnectCallback () {
      if (Debugging)
        Debug.Log ("Client connected.");
      _message_server.StartReceiving (OnReceiveCallback, OnDisconnectCallback, OnDebugCallback);
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
