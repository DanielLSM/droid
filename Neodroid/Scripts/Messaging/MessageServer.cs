
using System.Threading;
using AsyncIO;
using FlatBuffers;
using Neodroid.FBS.Reaction;
using Neodroid.Messaging.Messages;
using NetMQ;
using NetMQ.Sockets;

namespace Neodroid.Messaging {
  [System.Serializable]
  public class MessageServer {
    #region PublicMembers

    public bool ClientConnected;

    #endregion

    #region PrivateMembers

    private Thread _polling_thread;
    private Thread _wait_for_client_thread;
    private readonly object _thread_lock = new object ();
    private bool _stop_thread_;
    private bool _waiting_for_main_loop_to_send;
    private readonly bool _use_inter_process_communication;
    private bool _debugging;

    private readonly ResponseSocket _socket;

    //PairSocket _socket;
    private readonly string _ip_address;
    private readonly int _port;
    private byte[] _byte_buffer;

    #endregion

    #region PrivateMethods

    #region Threads

    private void WaitForClientToConnect (System.Action callback) {
      if (_use_inter_process_communication)
        _socket.Bind ("ipc:///tmp/neodroid/messages");
      else
        _socket.Bind ("tcp://" + _ip_address + ":" + _port);
      callback ();
      ClientConnected = true;
    }

    private void PollingThread (
      System.Action<Reaction> receive_callback,
      System.Action disconnect_callback,
      System.Action<string> debug_callback) {
      while (_stop_thread_ == false)
        if (!_waiting_for_main_loop_to_send)
          try {
            byte[] msg;
            _socket.TryReceiveFrameBytes (
              System.TimeSpan.FromSeconds (2),
              out msg);
            if (msg != null && msg.Length > 0) {
              var flat_reaction = FBSReaction.GetRootAsFBSReaction (new ByteBuffer (msg));
              if (Debugging)
                debug_callback (flat_reaction.ToString ());
              var reaction = FBSReactionUtilities.create_reaction (flat_reaction);
              receive_callback (reaction);
              _waiting_for_main_loop_to_send = true;
            }
          } catch (System.Exception err) {
            debug_callback (err.ToString ());
          }

      if (_use_inter_process_communication)
        _socket.Disconnect ("inproc://neodroid");
      else
        _socket.Disconnect ("tcp://" + _ip_address + ":" + _port);
      try {
        _socket.Dispose ();
        _socket.Close ();
      } finally {
        NetMQConfig.Cleanup (false);
      }
    }

    #endregion

    #endregion

    #region PublicMethods

    public void SendEnvironmentStates (EnvironmentState[] environment_states) {
      _byte_buffer = FBSStateUtilities.build_states (environment_states);
      _socket.SendFrame (_byte_buffer);
      _waiting_for_main_loop_to_send = false;
    }

    public void ListenForClientToConnect (System.Action callback) {
      _wait_for_client_thread = new Thread (unused_param => WaitForClientToConnect (callback)) {
        IsBackground =
                                                                                                 true
      };
      // Is terminated with foreground threads, when they terminate
      _wait_for_client_thread.Start ();
    }

    public void StartReceiving (
      System.Action<Reaction> cmd_callback,
      System.Action disconnect_callback,
      System.Action<string> debug_callback) {
      _polling_thread = new Thread (
        unused_param =>
                                     PollingThread (
          cmd_callback,
          disconnect_callback,
          debug_callback)) {
        IsBackground = true
      };
      // Is terminated with foreground threads, when they terminate
      _polling_thread.Start ();
    }

    #region Contstruction

    public MessageServer (
      string ip_address = "127.0.0.1",
      int port = 5555,
      bool use_inter_process_communication = false,
      bool debug = false) {
      Debugging = debug;
      _ip_address = ip_address;
      _port = port;
      _use_inter_process_communication = use_inter_process_communication;
      if (!_use_inter_process_communication)
        ForceDotNet.Force ();
      _socket = new ResponseSocket ();
      //_socket = new PairSocket ();
    }

    public MessageServer (bool debug = false) : this (
        "127.0.0.1",
        5555,
        false,
        debug) {
    }

    #endregion

    #region Getters

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    #endregion

    #endregion

    #region Deconstruction

    public void Destroy () {
      KillPollingAndListenerThread ();
    }

    public void KillPollingAndListenerThread () {
      try {
        lock (_thread_lock) {
          _stop_thread_ = true;
        }

        if (_use_inter_process_communication)
          _socket.Disconnect ("ipc:///tmp/neodroid/messages");
        else
          _socket.Disconnect ("tcp://" + _ip_address + ":" + _port);
        try {
          _socket.Dispose ();
          _socket.Close ();
        } finally {
          NetMQConfig.Cleanup (false);
        }

        if (_wait_for_client_thread != null)
          _wait_for_client_thread.Join ();
        if (_polling_thread != null)
          _polling_thread.Join ();
      } catch {
        System.Console.WriteLine ("Exception thrown while killing threads");
      }
    }

    #endregion
  }
}
