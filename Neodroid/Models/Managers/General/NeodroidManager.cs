using System;
using System.Collections.Generic;
using Neodroid.Models.Environments;
using Neodroid.Scripts.Messaging;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Models.Managers.General {
  public class NeodroidManager : MonoBehaviour,
                                 IHasRegister<LearningEnvironment> {
    [Header("Development", order = 99)]
    [SerializeField]
    bool _debugging;

    [Header("Connection", order = 100)]
    [SerializeField]
    string _ip_address = "localhost";

    [SerializeField] int _port = 5555;

    protected bool _reply;

    [SerializeField] bool _testing_motors;

    void FetchCommmandLineArguments() {
      var arguments = Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") this._ip_address = arguments[i + 1];
        if (arguments[i] == "-port") this._port = int.Parse(arguments[i + 1]);
      }
    }

    void StartMessagingServer() {
      if (this._ip_address != "" || this._port != 0)
        this._message_server = new MessageServer(this._ip_address, this._port, false, this.Debugging);
      else
        this._message_server = new MessageServer(this.Debugging);

      this._message_server.ListenForClientToConnect(this.OnConnectCallback);
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
      this.InnerStart();
    }

    void Update() {
      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Before) {
        this.SendEnvironmentStates(this.ReactInEnvironments(this.CurrentReaction));
        this._reply = false;
      }

      this.InnerUpdate();

      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Middle) {
        this.SendEnvironmentStates(this.ReactInEnvironments(this.CurrentReaction));
        this._reply = false;
      }
    }

    void LateUpdate() {
      this.PostUpdate();

      if (this._reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.After) {
        this.SendEnvironmentStates(this.ReactInEnvironments(this.CurrentReaction));
        this._reply = false;
      }
    }

    #endregion

    #region PrivateMethods

    protected virtual void InnerStart() { }
    protected virtual void InnerUpdate() { }

    protected void PostUpdate() {
      foreach (var environment in this._environments.Values)
        environment.PostUpdate();
      this.SetDefaultReaction();
    }

    protected Reaction SampleTestReaction() {
      var motions = new List<MotorMotion>();
      foreach (var environment in this._environments) {
        foreach (var actor in environment.Value.Actors) {
          foreach (var motor in actor.Value.Motors) {
            var strength = this._random_generator.Next(
                (int)motor.Value.ValidInput.MinValue,
                (int)(motor.Value.ValidInput.MaxValue + 1));
            motions.Add(new MotorMotion(actor.Key, motor.Key, strength));
          }
        }

        break;
      }

      var rp = new ReactionParameters(true, true) {IsExternal = false};
      return new Reaction(rp, motions.ToArray(), null, null);
    }

    protected void SendEnvironmentStates(EnvironmentState[] states) {
      this._message_server.SendEnvironmentStates(states);
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
        states[i++] = environment.React(reaction);
      return states;
    }

    public string GetStatus() { return this._message_server.ClientConnected ? "Connected" : "Not Connected"; }

    #endregion

    #region HasRegister implementation

    public void Register(LearningEnvironment environment) {
      if (this.Debugging) {
        Debug.Log(
            string.Format("Manager {0} has environment {1}", this.name, environment.EnvironmentIdentifier));
      }

      this._environments.Add(environment.EnvironmentIdentifier, environment);
    }

    public void Register(LearningEnvironment environment, string identifier) {
      if (this.Debugging)
        Debug.Log(string.Format("Manager {0} has environment {1}", this.name, identifier));
      this._environments.Add(identifier, environment);
    }

    #endregion

    #region MessageServerCallbacks

    void OnReceiveCallback(Reaction reaction) {
      if (this.Debugging)
        print("Received: " + reaction);
      this.CurrentReaction = reaction;
      this.CurrentReaction.Parameters.IsExternal = true;
      this._reply = true;
    }

    void OnDisconnectCallback() {
      if (this.Debugging)
        Debug.Log("Client disconnected.");
    }

    void OnDebugCallback(string error) {
      if (this.Debugging)
        Debug.Log("DebugCallback: " + error);
    }

    void OnConnectCallback() {
      if (this.Debugging)
        Debug.Log("Client connected.");
      this._message_server.StartReceiving(
          this.OnReceiveCallback,
          this.OnDisconnectCallback,
          this.OnDebugCallback);
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
