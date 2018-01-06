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
  public enum WaitOn {
    Never,
    FixedUpdate,
    Update
    // Frame
  }

  public class SimulationManager : MonoBehaviour, HasRegister<LearningEnvironment> {

    #region Fields

    [Header ("Development", order = 99)]
    [SerializeField]
    bool _debugging = false;
    [SerializeField]
    bool _test_motors = false;

    [Header ("Connection", order = 100)]
    [SerializeField]
    string _ip_address = "127.0.0.1";
    [SerializeField]
    int _port = 5555;

    [Header ("General", order = 100)]
    [SerializeField]
    WaitOn _wait_every = WaitOn.FixedUpdate;
    [SerializeField]
    bool _update_fixed_time_scale = false;
    // When true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField]
    bool _continue_on_disconnect = false;
    [SerializeField]
    int _frame_skips = 0;
    [SerializeField]
    float _simulation_time_scale = 1;
    [SerializeField]
    int _reset_iterations = 10;

    bool _reply = false;

    //When resetting transforms we run multiple times to ensure that we properly reset hierachies of objects


    #endregion

    #region Getters

    public bool TestMotors {
      get {
        return _test_motors;
      }
      set {
        _test_motors = value;
      }
    }

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

    public bool ContinueOnDisconnect {
      get {
        return _continue_on_disconnect;
      }
      set { 
        _continue_on_disconnect = value;
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

    public int ResetIterations {
      get {
        return _reset_iterations;
      }
      set { 
        _reset_iterations = value;
      }
    }

    public Reaction CurrentReaction {
      get {
        return _reaction;
      }
      set { 
        _reaction = value;
      }
    }

    public WaitOn WaitEvery {
      get {
        return _wait_every;
      }
      set { 
        _wait_every = value;
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

    public bool IsSimulationPaused () {
      return Time.timeScale == 0;
    }

    #endregion

    #region PrivateMembers

    Dictionary<string, LearningEnvironment> _environments = new Dictionary<string, LearningEnvironment> ();
    MessageServer _message_server;
    System.Random _random_generator;
    Reaction _reaction = new Reaction ();


    #endregion

    #region UnityCallbacks

    void Start () {
      FetchCommmandLineArguments ();
      StartMessagingServer ();
      _random_generator = new System.Random ();
    }

    void FixedUpdate () {
      if (WaitEvery == WaitOn.FixedUpdate) {
        PauseSimulation ();
      }
    }

    void LateUpdate () {
      PostUpdate ();
    }

    void Update () {
      if (WaitEvery == WaitOn.Update) {
        PauseSimulation ();
      }
      if (TestMotors) {
        ResumeSimulation (_simulation_time_scale);
        ReactInEnvironments (SampleTestReaction ());
        return;
      }
      if (WaitEvery == WaitOn.Never || CurrentReaction.Parameters.Step) {
        ResumeSimulation (_simulation_time_scale);
      }
      if (_reply) {
        SendEnvironmentStates (ReactInEnvironments (CurrentReaction));
        _reply = false;
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
      if (_message_server._client_connected)
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

    Reaction SampleTestReaction () {
      var motions = new List<MotorMotion> ();
      foreach (var environment in _environments) {
        foreach (var actor in environment.Value.Actors) {
          foreach (var motor in actor.Value.Motors) {
            var strength = _random_generator.Next ((int)(motor.Value.ValidInput.min_value), (int)(motor.Value.ValidInput.max_value + 1));
            motions.Add (new MotorMotion (actor.Key, motor.Key, strength));
          }
        }
        break;
      }
      var rp = new ReactionParameters (true, true);
      rp.BeforeObservation = false;
      return new Reaction (rp, motions.ToArray (), null, null);
    }

    void SendEnvironmentStates (EnvironmentState[] states) {
      _message_server.SendEnvironmentStates (states);
    }

    void PostUpdate () {
      foreach (var environment in _environments.Values) {
        environment.PostUpdate ();
      }
    }

    void SetDefaultReaction () {
      CurrentReaction = new Reaction ();
      CurrentReaction.Parameters.BeforeObservation = false;
    }


    void PauseSimulation () {
      Time.timeScale = 0;
      if (_update_fixed_time_scale)
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    void ResumeSimulation (float simulation_time_scale) {
      if (simulation_time_scale > 0) {
        Time.timeScale = simulation_time_scale;
        if (_update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      } else {
        Time.timeScale = 1;
        if (_update_fixed_time_scale)
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
      if (Debugging)
        print ("Received: " + reaction.ToString ());
      CurrentReaction = reaction;
      CurrentReaction.Parameters.BeforeObservation = true;
      _reply = true;
    }

    void OnDisconnectCallback () {
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
