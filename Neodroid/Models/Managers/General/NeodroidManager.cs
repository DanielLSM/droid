using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Managers {
  public class NeodroidManager : MonoBehaviour,
                                 IHasRegister<LearningEnvironment> {

    [Header (
      "Development",
      order = 99)]
    [SerializeField]
    private bool _debugging;

    [Header (
      "Connection",
      order = 100)]
    [SerializeField]
    private string _ip_address = "localhost";

    [SerializeField]
    private int _port = 5555;

    protected bool _reply;

    [SerializeField]
    private bool _testing_motors;

    private void FetchCommmandLineArguments () {
      var arguments = System.Environment.GetCommandLineArgs ();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments [i] == "-ip")
          _ip_address = arguments [i + 1];
        if (arguments [i] == "-port")
          _port = int.Parse (arguments [i + 1]);
      }
    }

    private void StartMessagingServer () {
      if (_ip_address != "" || _port != 0)
        _message_server = new MessageServer (
          _ip_address,
          _port,
          false,
          Debugging);
      else
        _message_server = new MessageServer (Debugging);

      _message_server.ListenForClientToConnect (OnConnectCallback);
    }

    #region Getter Setters

    public Reaction CurrentReaction { get { return _reaction; } set { _reaction = value; } }

    public bool TestMotors { get { return _testing_motors; } set { _testing_motors = value; } }

    public string IPAddress { get { return _ip_address; } set { _ip_address = value; } }

    public int Port { get { return _port; } set { _port = value; } }

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    #endregion

    #region PrivateMembers

    protected Dictionary<string, LearningEnvironment> _environments =
      new Dictionary<string, LearningEnvironment> ();

    protected MessageServer _message_server;
    protected Random _random_generator;
    protected Reaction _reaction = new Reaction ();

    #endregion

    #region UnityCallbacks

    private void Start () {
      FetchCommmandLineArguments ();
      StartMessagingServer ();
      _random_generator = new Random ();
    }

    private void Update () {
      if (_reply && CurrentReaction.Parameters.Phase == ExecutionPhase.Before) {
        SendEnvironmentStates (ReactInEnvironments (CurrentReaction));
        _reply = false;
      }

      InnerUpdate ();
      if (_reply && CurrentReaction.Parameters.Phase == ExecutionPhase.Middle) {
        SendEnvironmentStates (ReactInEnvironments (CurrentReaction));
        _reply = false;
      }
    }

    private void LateUpdate () {
      PostUpdate ();
      if (_reply && CurrentReaction.Parameters.Phase == ExecutionPhase.After) {
        SendEnvironmentStates (ReactInEnvironments (CurrentReaction));
        _reply = false;
      }
    }

    #endregion

    #region PrivateMethods

    protected virtual void InnerUpdate () {
    }

    protected void PostUpdate () {
      foreach (var environment in _environments.Values)
        environment.PostUpdate ();
      SetDefaultReaction ();
    }

    protected Reaction SampleTestReaction () {
      var motions = new List<MotorMotion> ();
      foreach (var environment in _environments) {
        foreach (var actor in environment.Value.Actors)
          foreach (var motor in actor.Value.Motors) {
            var strength = _random_generator.Next (
                             (int)motor.Value.ValidInput.min_value,
                             (int)(motor.Value.ValidInput.max_value + 1));
            motions.Add (
              new MotorMotion (
                actor.Key,
                motor.Key,
                strength));
          }

        break;
      }

      var rp = new ReactionParameters (
                 true,
                 true) {
        IsExternal = false
      };
      return new Reaction (
        rp,
        motions.ToArray (),
        null,
        null);
    }

    protected void SendEnvironmentStates (EnvironmentState[] states) {
      _message_server.SendEnvironmentStates (states);
    }

    private void SetDefaultReaction () {
      CurrentReaction = new Reaction ();
      CurrentReaction.Parameters.IsExternal = false;
    }

    #endregion

    #region PublicMethods

    public EnvironmentState[] ReactInEnvironments (Reaction reaction) {
      var states = new EnvironmentState[_environments.Values.Count];
      var i = 0;
      foreach (var environment in _environments.Values)
        states [i++] = environment.React (reaction);
      return states;
    }

    public string GetStatus () {
      return _message_server.ClientConnected ? "Connected" : "Not Connected";
    }

    #endregion

    #region HasRegister implementation

    public void Register (LearningEnvironment environment) {
      if (Debugging)
        Debug.Log (
          string.Format (
            "Manager {0} has environment {1}",
            name,
            environment.EnvironmentIdentifier));
      _environments.Add (
        environment.EnvironmentIdentifier,
        environment);
    }

    public void Register (LearningEnvironment environment, string identifier) {
      if (Debugging)
        Debug.Log (
          string.Format (
            "Manager {0} has environment {1}",
            name,
            identifier));
      _environments.Add (
        identifier,
        environment);
    }

    #endregion

    #region MessageServerCallbacks

    private void OnReceiveCallback (Reaction reaction) {
      if (Debugging)
        print ("Received: " + reaction);
      CurrentReaction = reaction;
      CurrentReaction.Parameters.IsExternal = true;
      _reply = true;
    }

    private void OnDisconnectCallback () {
      if (Debugging)
        Debug.Log ("Client disconnected.");
    }

    private void OnDebugCallback (string error) {
      if (Debugging)
        Debug.Log ("DebugCallback: " + error);
    }

    private void OnConnectCallback () {
      if (Debugging)
        Debug.Log ("Client connected.");
      _message_server.StartReceiving (
        OnReceiveCallback,
        OnDisconnectCallback,
        OnDebugCallback);
    }

    private void OnInterruptCallback () {
    }

    #endregion

    #region Deconstruction

    private void OnApplicationQuit () {
      _message_server.KillPollingAndListenerThread ();
    }

    private void OnDestroy () {
      //Deconstructor
      _message_server.Destroy ();
    }

    #endregion
  }
}
