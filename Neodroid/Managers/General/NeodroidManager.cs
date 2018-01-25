using System;
using System.Collections;
using System.Collections.Generic;
using Neodroid.Environments.General;
using Neodroid.Scripts.Messaging;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Interfaces;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using Neodroid.Utilities.Messaging;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers.General {


  public class NeodroidManager : MonoBehaviour,
                                 IHasRegister<NeodroidEnvironment> {
    [Header ("General", order = 101)]
    [SerializeField]
    protected bool _awaiting_reply;

    [SerializeField] protected SimulatorConfiguration _configuration;

    [Header ("Development", order = 99)]
    [SerializeField]
    bool _debugging;

    [Header ("Connection", order = 100)]
    [SerializeField]
    string _ip_address = "localhost";

    [SerializeField] int _port = 5555;

    [Header ("Simulation", order = 101)]

    // When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    [SerializeField]
    bool _update_fixed_time_scale;

    [SerializeField] bool _testing_motors;
    [SerializeField] float _last_reply_time;
    [SerializeField] int _skip_frame_i;

    public SimulatorConfiguration Configuration {
      get {
        if (this._configuration == null)
          this.Configuration = new SimulatorConfiguration ();

        return this._configuration;
      }
      set { this._configuration = value; }
    }

    public event Action EarlyFixedUpdateEvent;
    public event Action FixedUpdateEvent;
    public event Action LateFixedUpdateEvent;

    public event Action EarlyUpdateEvent;
    public event Action UpdateEvent;
    public event Action LateUpdateEvent;
    public event Action OnPostRenderEvent;
    public event Action OnRenderImageEvent;
    public event Action OnEndOfFrameEvent;



    public event Action OnReceiveEvent;

    void FetchCommmandLineArguments () {
      var arguments = Environment.GetCommandLineArgs ();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments [i] == "-ip")
          this.IPAddress = arguments [i + 1];
        if (arguments [i] == "-port")
          this.Port = int.Parse (arguments [i + 1]);
      }
    }

    void StartMessagingServer () {
      if (this.IPAddress != "" || this.Port != 0)
        this._message_server = new MessageServer (this.IPAddress, this.Port, false, this.Debugging);
      else
        this._message_server = new MessageServer (this.Debugging);
      this._message_server.ListenForClientToConnect (this.OnConnectCallback);
    }

    #region Getter Setters

    public Reaction CurrentReaction {
      get { return this._current_reaction; }
      set { this._current_reaction = value; }
    }

    public bool TestMotors { get { return this._testing_motors; } set { this._testing_motors = value; } }

    public string IPAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    public int Port { get { return this._port; } set { this._port = value; } }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #region PrivateMembers

    protected Dictionary<string, NeodroidEnvironment> _environments =
      new Dictionary<string, NeodroidEnvironment> ();

    protected MessageServer _message_server;

    protected Reaction _current_reaction = new Reaction ();

    #endregion

    #region UnityCallbacks

    protected void Start () {
      if (this.Configuration.SimulationType != SimulationType.FrameDependent) {
        this.EarlyFixedUpdateEvent += this.PreStep;
        this.FixedUpdateEvent += this.Step;
        this.LateFixedUpdateEvent += this.PostStep;
        this.StartCoroutine(this.LateFixedUpdate());
      } else {
        this.EarlyUpdateEvent += this.PreStep;
        this.UpdateEvent += this.Step;
        switch (this._configuration.FrameFinishes) {
          case FrameFinishes.LateUpdate:
            this.LateUpdateEvent += this.PostStep;
            break;
          case FrameFinishes.OnPostRender:
            this.OnPostRenderEvent += this.PostStep;
            break;
          case FrameFinishes.OnRenderImage:
            this.OnRenderImageEvent += this.PostStep;
            break;
          case FrameFinishes.EndOfFrame:
            this.StartCoroutine(this.EndOfFrame());
            this.OnEndOfFrameEvent += this.PostStep;
            break;
          default: throw new ArgumentOutOfRangeException();
        }

      }

      if (this.Configuration == null)
        this.Configuration = new SimulatorConfiguration ();

      this.ApplyConfiguration ();
      this.FetchCommmandLineArguments ();

      this.StartMessagingServer ();
    }

    public void ApplyConfiguration () {
      QualitySettings.SetQualityLevel (this._configuration.QualityLevel, true);
      this.SimulationTime = this._configuration.TimeScale;
      Application.targetFrameRate = this._configuration.TargetFrameRate;
      QualitySettings.vSyncCount = 0;

      #if !UNITY_EDITOR
      Screen.SetResolution (
        width : this._configuration.Width,
        height : this._configuration.Height,
        fullscreen : this._configuration.FullScreen);
      #endif
    }

    public float SimulationTime {
      get { return Time.timeScale; }
      set {
        Time.timeScale = value;
        if (this._update_fixed_time_scale)
          Time.fixedDeltaTime = 0.02F * Time.timeScale;
      }
    }

    void OnPostRender() {
      if (this.OnPostRenderEvent != null) this.OnPostRenderEvent();
    }

    /*void OnRenderImage(RenderTexture src, RenderTexture dest) {
      if (this.OnRenderImageEvent != null) {
        this.OnRenderImageEvent();
      }
    }*/

    protected void FixedUpdate () {
      if (this.EarlyFixedUpdateEvent != null)
        this.EarlyFixedUpdateEvent ();

      if (this.FixedUpdateEvent != null)
        this.FixedUpdateEvent ();
    }


    protected IEnumerator LateFixedUpdate() {
      while (true) {
        yield return new WaitForFixedUpdate();
        if(this.Debugging) print("LateFixedUpdate");
        if (this.LateFixedUpdateEvent != null)
          this.LateFixedUpdateEvent ();
      }
      // ReSharper disable once IteratorNeverReturns
    }

    protected IEnumerator EndOfFrame() {
      while (true) {
        yield return new WaitForEndOfFrame();

        if (this.OnEndOfFrameEvent != null)
          this.OnEndOfFrameEvent ();
      }
      // ReSharper disable once IteratorNeverReturns
    }

    protected void Update () {
      if (this.EarlyUpdateEvent != null)
        this.EarlyUpdateEvent ();

      if (this.UpdateEvent != null)
        this.UpdateEvent ();
    }

    protected void LateUpdate () {
      if (this.LateUpdateEvent != null)
        this.LateUpdateEvent ();
    }

    #endregion

    #region PrivateMethods

    protected void PreStep () {
      if (this._awaiting_reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Before)
        this.ReactReply ();
    }

    protected void Step () {
      if (this.TestMotors)
        this.React (this.SampleRandomReaction ());

      if (this._awaiting_reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.Middle)
        this.ReactReply ();
    }

    protected void ReactReply () {
      var state = this.React (this.CurrentReaction);

      if (this._skip_frame_i >= this.Configuration.FrameSkips) {
        //&&this._last_reply_time + this._configuration.MaxReplyInterval > Time.time) {
        this.Reply (state);
        this._awaiting_reply = false;
        this._skip_frame_i = 0;
        //this._last_reply_time = Time.time;
      } else {
        this._skip_frame_i += 1;
        if (this.Debugging)
          print ("Skipping frame");
      }
    }

    protected void PostStep () {
      if (this._awaiting_reply && this.CurrentReaction.Parameters.Phase == ExecutionPhase.After)
        this.ReactReply ();

      foreach (var environment in this._environments.Values)
        environment.PostStep ();

      //if (!this._awaiting_reply)
      this.ResetReaction ();
    }

    protected Reaction SampleRandomReaction () {
      var reaction = new Reaction ();
      foreach (var environment in this._environments.Values)
        reaction = environment.SampleReaction ();

      return reaction;
    }

    //TODO: EnvironmentState[][] states for aggregation of frame skip states
    protected void Reply (EnvironmentState[] states) {
      this._message_server.SendStates (states);
    }

    void ResetReaction () {
      this.CurrentReaction = new Reaction ();
      this.CurrentReaction.Parameters.IsExternal = false;
    }

    #endregion

    #region PublicMethods

    public EnvironmentState[] React (Reaction reaction) {
      var states = new EnvironmentState[this._environments.Values.Count];
      var i = 0;
      foreach (var environment in this._environments.Values)
        states [i++] = environment.React (reaction);
      return states;
    }

    public string GetStatus () {
      return this._message_server.ClientConnected ? "Connected" : "Not Connected";
    }

    #endregion

    #region Registration

    public void Register (NeodroidEnvironment environment) {
      if (this.Debugging)
        Debug.Log (string.Format ("Manager {0} has environment {1}", this.name, environment.Identifier));
      this._environments.Add (environment.Identifier, environment);
    }

    public void Register (NeodroidEnvironment environment, string identifier) {
      if (this.Debugging)
        Debug.Log (string.Format ("Manager {0} has environment {1}", this.name, identifier));
      this._environments.Add (identifier, environment);
    }

    #endregion

    #region MessageServerCallbacks

    void OnReceiveCallback (Reaction reaction) {
      if (this.Debugging)
        print ("Received: " + reaction);
      this.SetReactionFromExternalSource(reaction);

      if (this.OnReceiveEvent != null) {
        this.OnReceiveEvent();
      }
    }

    protected void SetReactionFromExternalSource(Reaction reaction) {
      this.CurrentReaction = reaction;
      this.CurrentReaction.Parameters.IsExternal = true;
      this._awaiting_reply = true;
    }

    void OnDisconnectCallback () {
      if (this.Debugging)
        Debug.Log ("Client disconnected.");
    }

    void OnDebugCallback (string error) {
      if (this.Debugging)
        Debug.Log ("DebugCallback: " + error);
    }

    void OnConnectCallback () {
      if (this.Debugging)
        Debug.Log ("Ready to connect client");
      this._message_server.StartReceiving (
        this.OnReceiveCallback,
        this.OnDisconnectCallback,
        this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    void OnApplicationQuit () {
      this._message_server.CleanUp ();
    }

    void OnDestroy () {
      this._message_server.Destroy ();
    }

    #endregion
  }
}
