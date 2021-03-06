﻿using System;
using System.Threading;
using AsyncIO;
using FlatBuffers;
using Neodroid.FBS.Reaction;
using Neodroid.Scripts.Messaging.FBS;
using Neodroid.Scripts.Messaging.Messages;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

namespace Neodroid.Utilities.Messaging {
  [Serializable]
  public class MessageServer {
    #region PublicMembers

    public bool ClientConnected;

    #endregion

    #region PrivateMembers

    bool _use_threads = false;
    Thread _polling_thread;
    Thread _wait_for_client_thread;
    readonly object _thread_lock = new object();
    bool _stop_thread_;
    bool _waiting_for_main_loop_to_send;
    readonly bool _use_inter_process_communication;
    bool _debugging;

    readonly ResponseSocket _socket;

    //PairSocket _socket;
    readonly string _ip_address;
    readonly int _port;
    byte[] _byte_buffer;

    #endregion

    #region PrivateMethods

    #region Threads

    void WaitForClientToConnect(Action callback) {
      if (this._use_inter_process_communication)
        this._socket.Bind("ipc:///tmp/neodroid/messages");
      else
        this._socket.Bind("tcp://" + this._ip_address + ":" + this._port);
      if(callback!=null)
        callback();
      this.ClientConnected = true;
    }

    public Reaction Receive(TimeSpan wait_time) {
      Reaction reaction = null;
      try {
        byte[] msg;
        if(wait_time>TimeSpan.Zero)
          this._socket.TryReceiveFrameBytes(wait_time, out msg);
        else
          msg = this._socket.ReceiveFrameBytes();

        if (msg != null && msg.Length > 0) {
          var flat_reaction = FReaction.GetRootAsFReaction(new ByteBuffer(msg));
          reaction = FBSReactionUtilities.create_reaction(flat_reaction);
        }
      } catch (Exception err) {
        Debug.Log(err);
      }

      return reaction;
    }

    void PollingThread(
        Action<Reaction> receive_callback,
        Action disconnect_callback,
        Action<string> debug_callback) {
      while (this._stop_thread_ == false) {
        if (!this._waiting_for_main_loop_to_send) {
          var reaction = this.Receive(TimeSpan.FromSeconds(2));
          if (reaction != null) {
            receive_callback(reaction);
            this._waiting_for_main_loop_to_send = true;
          }
        }
      }

      disconnect_callback();
      if (this._use_inter_process_communication)
        this._socket.Disconnect("inproc://neodroid");
      else
        this._socket.Disconnect("tcp://" + this._ip_address + ":" + this._port);
      try {
        this._socket.Dispose();
        this._socket.Close();
      } finally {
        NetMQConfig.Cleanup(false);
      }
    }


    #endregion

    #endregion

    #region PublicMethods

    public void SendStates(EnvironmentState[] environment_states) {
      this._byte_buffer = FBSStateUtilities.build_states(environment_states);
      this._socket.SendFrame(this._byte_buffer);
      this._waiting_for_main_loop_to_send = false;
    }

    public void ListenForClientToConnect(Action callback) {
      if (this._use_threads){
        this._wait_for_client_thread =
            new Thread(unused_param => this.WaitForClientToConnect(callback)) {IsBackground = true};
      // Is terminated with foreground threads, when they terminate
      this._wait_for_client_thread.Start();
    }
    else

    {
        this.WaitForClientToConnect(null);
      }
    }

    public void StartReceiving(
        Action<Reaction> cmd_callback,
        Action disconnect_callback,
        Action<string> debug_callback) {
      this._polling_thread =
          new Thread(unused_param => this.PollingThread(cmd_callback, disconnect_callback, debug_callback)) {
              IsBackground = true
          };
      // Is terminated with foreground threads, when they terminate
      this._polling_thread.Start();
    }

    #region Contstruction

    public MessageServer(
        string ip_address = "127.0.0.1",
        int port = 5555,
        bool use_inter_process_communication = false,
        bool debug = false) {
      this.Debugging = debug;
      this._ip_address = ip_address;
      this._port = port;
      this._use_inter_process_communication = use_inter_process_communication;
      if (!this._use_inter_process_communication)
        ForceDotNet.Force();
      this._socket = new ResponseSocket();
    }

    public MessageServer(bool debug = false) : this("127.0.0.1", 5555, false, debug) { }

    #endregion

    #region Getters

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #endregion

    #region Deconstruction

    public void Destroy() { this.CleanUp(); }

    public void CleanUp() {
      try {
        lock (this._thread_lock) this._stop_thread_ = true;

        if (this._use_inter_process_communication)
          this._socket.Disconnect("ipc:///tmp/neodroid/messages");
        else
          this._socket.Disconnect("tcp://" + this._ip_address + ":" + this._port);
        try {
          this._socket.Dispose();
          this._socket.Close();
        } finally {
          NetMQConfig.Cleanup(false);
        }

        if (this._wait_for_client_thread != null) this._wait_for_client_thread.Join();
        if (this._polling_thread != null) this._polling_thread.Join();
      } catch {
        Console.WriteLine("Exception thrown while killing threads");
      }
    }

    #endregion
  }
}
