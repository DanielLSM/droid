using Neodroid.Managers;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class DemonstrationWindow : EditorWindow {
    private SimulationManager _environment_manager;
    private Texture _icon;
    private int capturedFrame;

    private string fileName = "Demonstration/frame";
    private float lastFrameTime;
    private string recordButton = "Record";
    private bool recording;

    private string status = "Idle";

    [MenuItem("Neodroid/DemonstrationWindow")]
    public static void ShowWindow() {
      GetWindow(
                typeof(DemonstrationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    private void OnEnable() {
      _icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
                                                       "Assets/Neodroid/Icons/bullet_red.png",
                                                       typeof(Texture2D));
      titleContent = new GUIContent(
                                    "Neo:Rec",
                                    _icon,
                                    "Window for recording demonstrations");
    }

    public void OnInspectorUpdate() { Repaint(); }

    private void OnGUI() {
      fileName = EditorGUILayout.TextField(
                                           "File Name:",
                                           fileName);

      if (GUILayout.Button(recordButton))
        if (recording) {
          //recording
          status = "Idle...";
          recordButton = "Record";
          recording = false;
        } else {
          // idle
          capturedFrame = 0;
          recordButton = "Stop";
          recording = true;
        }

      EditorGUILayout.LabelField(
                                 "Status: ",
                                 status);
    }

    private void Update() {
      if (recording)
        if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
          RecordImages();
          Repaint();
        } else {
          status = "Waiting for Editor to Play";
        }
    }

    private void RecordImages() {
      if (lastFrameTime < Time.time + 1 / 24f) {
        // 24fps
        status = "Captured frame" + capturedFrame;
        ScreenCapture.CaptureScreenshot(fileName + capturedFrame + ".png");
        capturedFrame++;
        lastFrameTime = Time.time;
      }
    }
  }
  #endif
}
