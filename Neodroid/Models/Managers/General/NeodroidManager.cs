using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Scripts.Messaging;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Models.Managers.General {
  public class NeodroidManager : MonoBehaviour,
                                 IHasRegister<LearningEnvironment> {
    [Header(
      header : "Development",
      order = 99)]
    [SerializeField]
    bool _debugging;

    [Header(
      header : "Connection",
      order = 100)]
    [SerializeField]
    string _ip_address = "localhost";

    [SerializeField] int _port = 5555;

    protected bool _reply;

    [SerializeField] bool _testing_motors;

    void FetchCommmandLineArguments() {
      var arguments = Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") this._ip_address = arguments[i + 1];
        if (arguments[i] == "-port") this._port = int.Parse(s : arguments[i + 1]);
      }
    }

    void StartMessagingServer() {
      if (this._ip_address != "" || this._port != 0)
        this._message_server = new MessageServer(
                                                 ip_address : this._ip_address,
                                                 port : this._port,
                                                 use_inter_process_communication : false,
                                                 debug : this.Debugging);
      else
        this._message_server = new MessageServer(debug : this.Debugging);

      this._message_server.ListenForClientToConnect(callback : this.OnConnectCallback);
    }

    #region Getter Setters

    public Reaction CurrentReaction { get { return this._reaction; } set { this._reaction = value; } }

    public bool TestMotors { get { return this._testing_motors; } set { this._testing_motors = value; } }

    public string IPAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    public int Port { get { return this._port; } set { this._port = value; } }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #region PrivateMembers

    protected Dictionary<string, LearningEnvironment> _environments =
      new Dictionary<string, LearningEnvironment>();

    protected MessageServer _message_server;
    protected Random _random_generator;
    protected Reaction _reaction = new Reaction();

    #endregion

    #region UnityCallbacks

    void Start() {
      this.FetchCommmandLineArguments();
      this.StartMessagingServer();
      this._random_generator = new Random();
    }

    void Update() {
      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Before) {
        this.SendEnvironmentStates(states : this.ReactInEnvironments(reaction : this.CurrentReaction));
        this._reply = false;
      }

      this.InnerUpdate();
      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Middle) {
        this.SendEnvironmentStates(states : this.ReactInEnvironments(reaction : this.CurrentReaction));
        this._reply = false;
      }
    }

    void LateUpdate() {
      this.PostUpdate();
      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.After) {
        this.SendEnvironmentStates(states : this.ReactInEnvironments(reaction : this.CurrentReaction));
        this._reply = false;
      }
    }

    #endregion

    #region PrivateMethods

    protected virtual void InnerUpdate() { }

    protected void PostUpdate() {
      foreach (var environment in this._environments.Values)
        environment.PostUpdate();
      this.SetDefaultReaction();
    }

    protected Reaction SampleTestReaction() {
      var motions = new List<MotorMotion>();
      foreach (var environment in this._environments) {
        foreach (var actor in environment.Value.Actors)
          foreach (var motor in actor.Value.Motors) {
            var strength = this._random_generator.Next(
                                                       minValue : (int)motor.Value.ValidInput.MinValue,
                                                       maxValue : (int)(motor.Value.ValidInput.MaxValue
                                                                        + 1));
            motions.Add(
                        item : new MotorMotion(
                                               actor_name : actor.Key,
                                               motor_name : motor.Key,
                                               strength : strength));
          }

        break;
      }

      var rp = new ReactionParameters(
                                      terminable : true,
                                      step : true) {
                                                     IsExternal = false
                                                   };
      return new Reaction(
                          parameters : rp,
                          motions : motions.ToArray(),
                          configurations : null,
                          unobservables : null);
    }

    protected void SendEnvironmentStates(EnvironmentState[] states) {
      this._message_server.SendEnvironmentStates(environment_states : states);
    }

    void SetDefaultReaction() {
      this.CurrentReaction = new Reaction();
      this.CurrentReaction.Parameters.IsExternal = false;
    }

    #endregion

    #region PublicMethods

    public EnvironmentState[] ReactInEnvironments(Reaction reaction) {
      var states = new EnvironmentState[this._environments.Values.Count];
      var i = 0;
      foreach (var environment in this._environments.Values)
        states[i++] = environment.React(reaction : reaction);
      return states;
    }

    public string GetStatus() { return this._message_server.ClientConnected ? "Connected" : "Not Connected"; }

    #endregion

    #region HasRegister implementation

    public void Register(LearningEnvironment environment) {
      if (this.Debugging)
        Debug.Log(
                  message : string.Format(
                                          format : "Manager {0} has environment {1}",
                                          arg0 : this.name,
                                          arg1 : environment.EnvironmentIdentifier));
      this._environments.Add(
                             key : environment.EnvironmentIdentifier,
                             value : environment);
    }

    public void Register(LearningEnvironment environment, string identifier) {
      if (this.Debugging)
        Debug.Log(
                  message : string.Format(
                                          format : "Manager {0} has environment {1}",
                                          arg0 : this.name,
                                          arg1 : identifier));
      this._environments.Add(
                             key : identifier,
                             value : environment);
    }

    #endregion

    #region MessageServerCallbacks

    void OnReceiveCallback(Reaction reaction) {
      if (this.Debugging)
        print(message : "Received: " + reaction);
      this.CurrentReaction = reaction;
      this.CurrentReaction.Parameters.IsExternal = true;
      this._reply = true;
    }

    void OnDisconnectCallback() {
      if (this.Debugging)
        Debug.Log(message : "Client disconnected.");
    }

    void OnDebugCallback(string error) {
      if (this.Debugging)
        Debug.Log(message : "DebugCallback: " + error);
    }

    void OnConnectCallback() {
      if (this.Debugging)
        Debug.Log(message : "Client connected.");
      this._message_server.StartReceiving(
                                          cmd_callback : this.OnReceiveCallback,
                                          disconnect_callback : this.OnDisconnectCallback,
                                          debug_callback : this.OnDebugCallback);
    }

    void OnInterruptCallback() { }

    #endregion

    #region Deconstruction

    void OnApplicationQuit() { this._message_server.KillPollingAndListenerThread(); }

    void OnDestroy() {
      //Deconstructor
      this._message_server.Destroy();
    }

    #endregion
  }
}
