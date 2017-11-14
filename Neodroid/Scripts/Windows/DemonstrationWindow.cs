using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor.AnimatedValues;
  using UnityEditor;

  public class DemonstrationWindow : EditorWindow {

    [MenuItem ("Neodroid/DemonstrationWindow")]
    public static void ShowWindow () {
  EditorWindow.GetWindow (typeof(DemonstrationWindow));      //Show existing window instance. If one doesn't exist, make one.
    }

    EnvironmentManager _environment_manager;
    Texture _icon;

    void OnEnable () {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Neodroid/Scripts/Windows/Icons/bullet_red.png", typeof(Texture2D));
      this.titleContent = new GUIContent ("Neo:Demo", _icon, "Window for recording demonstrations");
    }


    public void OnInspectorUpdate () {
      this.Repaint ();
    }

    string fileName = "FileName";

    string status = "Idle";
    string recordButton = "Record";
    bool recording = false;
    float lastFrameTime = 0.0f;
    int capturedFrame = 0;

    void OnGUI () {
      fileName = EditorGUILayout.TextField ("File Name:", fileName);

      if (GUILayout.Button (recordButton)) {
        if (recording) {  //recording
          status = "Idle...";
          recordButton = "Record";
          recording = false;
        } else {     // idle
          capturedFrame = 0;
          recordButton = "Stop";
          recording = true;
        }
      }
      EditorGUILayout.LabelField ("Status: ", status);
    }

    void Update () {
      if (recording) {
        if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
          RecordImages ();
          Repaint ();
        } else
          status = "Waiting for Editor to Play";
      }
    }

    void RecordImages () {
      if (lastFrameTime < Time.time + (1 / 24f)) { // 24fps
        status = "Captured frame " + capturedFrame;
        ScreenCapture.CaptureScreenshot (fileName + " " + capturedFrame + ".png");
        capturedFrame++;
        lastFrameTime = Time.time;
      }
    }
  }
  #endif
}
