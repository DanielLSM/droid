
using Neodroid.Messaging.Messages;
using NetMQ;
using NetMQ.Sockets;

using Neodroid.Messaging;
using Neodroid.Messaging.CustomFBS;
using Neodroid.FBS.Reaction;

namespace Neodroid.Messaging {
  [System.Serializable]
  class MessageServer {
    #region PublicMembers

    public bool _client_connected = false;

    #endregion

    #region PrivateMembers

    System.Threading.Thread _polling_thread;
    System.Threading.Thread _wait_for_client_thread;
    System.Object _thread_lock = new System.Object ();
    bool _stop_thread_ = false;
    bool _waiting_for_main_loop_to_send = false;
    bool _use_inter_process_communication = false;
    bool _debugging = false;

    ResponseSocket _socket;
    string _ip_address;
    int _port;
    byte[] byte_buffer;

    #endregion

    #region PrivateMethods

    #region Threads

    void WaitForClientToConnect (System.Action callback) {
      if (_use_inter_process_communication) {
        _socket.Bind ("ipc:///tmp/neodroid/messages");
        //_socket.Bind ("inproc://neodroid");
        //_socket.Bind ("epgm://" + _ip_address + ":" + _port.ToString ()); // for pub/sub sockets
      } else {
        _socket.Bind ("tcp://" + _ip_address + ":" + _port.ToString ());
      }
      callback ();
      _client_connected = true;
    }

    void PollingThread (System.Action<Reaction> receive_callback, System.Action disconnect_callback, System.Action<System.String> debug_callback) {
      byte[] msg;
      while (_stop_thread_ == false) {
        if (!_waiting_for_main_loop_to_send) {
          try {
            _socket.TryReceiveFrameBytes (System.TimeSpan.FromSeconds (2), out msg);
            if (msg != null && msg.Length > 0) {
              var flat_reaction = FBSReaction.GetRootAsFBSReaction (new FlatBuffers.ByteBuffer (msg));
              if (Debugging) {
                debug_callback (flat_reaction.ToString ());
              }
              var reaction = FBSReactionUtilities.create_reaction (flat_reaction);
              receive_callback (reaction);
              _waiting_for_main_loop_to_send = true;
            }
          } catch (System.Exception err) {
            debug_callback (err.ToString ());
          }
        }
      }

      if (_use_inter_process_communication) {
        _socket.Disconnect (("inproc://neodroid"));
      } else {
        _socket.Disconnect (("tcp://" + _ip_address + ":" + _port.ToString ()));
      }
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
      byte_buffer = FBSStateUtilities.build_states (environment_states);
      _socket.SendFrame (byte_buffer);
      _waiting_for_main_loop_to_send = false;
    }

    public void ListenForClientToConnect (System.Action callback) {
      _wait_for_client_thread = new System.Threading.Thread (unused_param => WaitForClientToConnect (callback));
      _wait_for_client_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _wait_for_client_thread.Start ();
    }

    public void StartReceiving (System.Action<Reaction> cmd_callback, System.Action disconnect_callback, System.Action<System.String> debug_callback) {
      _polling_thread = new System.Threading.Thread (unused_param => PollingThread (cmd_callback, disconnect_callback, debug_callback));
      _polling_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _polling_thread.Start ();
    }


    #region Contstruction

    public MessageServer (string ip_address = "127.0.0.1", int port = 5555, bool use_inter_process_communication = false, bool debug = false) {
      Debugging = debug;
      _ip_address = ip_address;
      _port = port;
      _use_inter_process_communication = use_inter_process_communication;
      if (!_use_inter_process_communication) {
        AsyncIO.ForceDotNet.Force ();
      }
      _socket = new ResponseSocket ();
    }

    public MessageServer (bool debug = false) {
      Debugging = debug;
      _ip_address = "127.0.0.1";
      _port = 5555;
      _use_inter_process_communication = false;
      if (!_use_inter_process_communication) {
        AsyncIO.ForceDotNet.Force ();
      }
      _socket = new ResponseSocket ();
    }



    #endregion

    #region Getters

    public bool Debugging {
      get {
        return _debugging;
      }
      set {
        _debugging = value;
      }
    }


    #endregion

    #endregion

    #region Deconstruction

    public void Destroy () {
      KillPollingAndListenerThread ();
    }

    public void KillPollingAndListenerThread () {
      try {
        lock (_thread_lock)
          _stop_thread_ = true;
        if (_use_inter_process_communication) {
          _socket.Disconnect (("ipc:///tmp/neodroid/messages"));
        } else {
          _socket.Disconnect (("tcp://" + _ip_address + ":" + _port.ToString ()));
        }
        try {
          _socket.Dispose ();
          _socket.Close ();
        } finally {
          NetMQConfig.Cleanup (false);
        }
        if (_wait_for_client_thread != null) {
          _wait_for_client_thread.Join ();
        }
        if (_polling_thread != null) {
          //  _polling_thread.Abort ();
          _polling_thread.Join ();
        }
      } catch {
        System.Console.WriteLine ("Exception thrown while killing threads");
      }
    }

    #endregion
  }
}
