using System;
using System.Threading;

using Neodroid.Messaging.Messages;
using NetMQ;
using NetMQ.Sockets;

using Neodroid.Messaging;
using Neodroid.Messaging.CustomFBS;
using Neodroid.Messaging.Models.Reaction;

namespace Neodroid.Messaging {
  class MessageServer {
    #region PrivateMembers

    Thread _polling_thread;
    Thread _wait_for_client_thread;
    Object _thread_lock = new Object ();
    bool _stop_thread_ = false;
    bool _waiting_for_main_loop_to_send = false;
    bool _use_inter_process_communication = false;

    ResponseSocket _socket;
    string _ip_address;
    int _port;
    byte[] byte_buffer;

    #endregion

    #region Contstruction

    public MessageServer (string ip_address = "127.0.0.1", int port = 5555, bool use_inter_process_communication = false) {
      _ip_address = ip_address;
      _port = port;
      _use_inter_process_communication = use_inter_process_communication;
      if (!_use_inter_process_communication) {
        AsyncIO.ForceDotNet.Force ();
      }
      _socket = new ResponseSocket ();
    }

    public void ListenForClientToConnect (Action callback) {
      _wait_for_client_thread = new Thread (unused_param => WaitForClientToConnect (callback));
      _wait_for_client_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _wait_for_client_thread.Start ();
    }

    public void StartReceiving (Action<Reaction> cmd_callback, Action disconnect_callback, Action<String> error_callback) {
      _polling_thread = new Thread (unused_param => PollingThread (cmd_callback, disconnect_callback, error_callback));
      _polling_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _polling_thread.Start ();
    }

    #endregion

    #region Threads

    void WaitForClientToConnect (Action callback) {
      if (_use_inter_process_communication) {
        //_socket.Bind ("inproc://neodroid");
        _socket.Bind ("ipc:///tmp/neodroid/messages0");
        //_socket.Bind ("epgm://" + _ip_address + ":" + _port.ToString ()); // for pub/sub sockets
      } else {
        _socket.Bind ("tcp://" + _ip_address + ":" + _port.ToString ());
      }
      callback ();
    }

    void PollingThread (Action<Reaction> receive_callback, Action disconnect_callback, Action<String> error_callback) {
      byte[] msg;
      while (_stop_thread_ == false) {
        if (!_waiting_for_main_loop_to_send) {
          try {
            //msg = _socket.TryReceiveFrameBytes ();
            _socket.TryReceiveFrameBytes (TimeSpan.FromSeconds (2), out msg);
            if (msg != null && msg.Length > 0) {
              var flat_reaction = FBSReaction.GetRootAsFBSReaction (new FlatBuffers.ByteBuffer (msg));
              var reaction = FBSUtilities.create_reaction (flat_reaction);
              receive_callback (reaction);
              _waiting_for_main_loop_to_send = true;
            }
          } catch (Exception err) {
            error_callback (err.ToString ());
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

    #region PublicMethods

    public void SendEnvironmentState (EnvironmentState environment_state) {
      byte_buffer = FBSUtilities.build_state (environment_state);
      _socket.SendFrame (byte_buffer);
      _waiting_for_main_loop_to_send = false; 
    }

    public void SendEnvironmentStates (EnvironmentState[] environment_states) {
      byte_buffer = FBSUtilities.build_states (environment_states);
      _socket.SendFrame (byte_buffer);
      _waiting_for_main_loop_to_send = false;
    }

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
          _socket.Disconnect (("ipc:///tmp/neodroid/messages0"));
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
