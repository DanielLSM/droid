﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Neodroid.Utilities {
  public static class NeodroidUtilities {
    #if UNITY_EDITOR
    public static void DrawString (string text, Vector3 worldPos, Color? color = null, float oX = 0, float oY = 0) {

      UnityEditor.Handles.BeginGUI ();

      var restoreColor = GUI.color;

      if (color.HasValue)
        GUI.color = color.Value;
      var view = UnityEditor.SceneView.currentDrawingSceneView;
      Vector3 screenPos = view.camera.WorldToScreenPoint (worldPos);

      if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI ();
        return;
      }

      UnityEditor.Handles.Label (TransformByPixel (worldPos, oX, oY), text);

      GUI.color = restoreColor;
      UnityEditor.Handles.EndGUI ();
    }

    public static Vector3 TransformByPixel (Vector3 position, float x, float y) {
      return TransformByPixel (position, new Vector3 (x, y));
    }

    public static Vector3 TransformByPixel (Vector3 position, Vector3 translateBy) {
      Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
      if (cam)
        return cam.ScreenToWorldPoint (cam.WorldToScreenPoint (position) + translateBy);
      else
        return position;
    }
    #endif

    public static Texture2D RenderTextureImage (Camera camera) { // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      RenderTexture current_render_texture = RenderTexture.active;
      RenderTexture.active = camera.targetTexture;
      camera.Render ();
      Texture2D texture = new Texture2D (camera.targetTexture.width, camera.targetTexture.height);
      texture.ReadPixels (new Rect (0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
      texture.Apply ();
      RenderTexture.active = current_render_texture;
      return texture;
    }

    public static void RegisterCollisionTriggerCallbacksOnChildren (Transform transform,
                                                                    ChildSensor.OnChildCollisionEnterDelegate OnCollisionEnterChild,
                                                                    ChildSensor.OnChildTriggerEnterDelegate OnTriggerEnterChild,
                                                                    ChildSensor.OnChildCollisionExitDelegate OnCollisionExitChild,
                                                                    ChildSensor.OnChildTriggerExitDelegate OnTriggerExitChild,
                                                                    ChildSensor.OnChildCollisionStayDelegate OnCollisionStayChild,
                                                                    ChildSensor.OnChildTriggerStayDelegate OnTriggerStayChild,
                                                                    bool debug = false) {
      var childrenWithColliders = transform.GetComponentsInChildren<Collider> (transform.gameObject);

      foreach (Collider child in childrenWithColliders) {
        ChildSensor child_sensor = child.gameObject.AddComponent<ChildSensor> ();
        child_sensor.OnCollisionEnterDelegate = OnCollisionEnterChild;
        child_sensor.OnTriggerEnterDelegate = OnTriggerEnterChild;
        child_sensor.OnCollisionExitDelegate = OnCollisionExitChild;
        child_sensor.OnTriggerExitDelegate = OnTriggerExitChild;
        child_sensor.OnTriggerStayDelegate = OnTriggerStayChild;
        child_sensor.OnCollisionStayDelegate = OnCollisionStayChild;
        if (debug)
          Debug.Log (transform.name + " has " + child_sensor.name + " registered");
      }
    }

    public static string ColorArrayToString (Color[] colors) {
      string s = "";
      foreach (Color color in colors) {
        s += color.ToString ();
      }
      return s;
    }

    public static Recipient MaybeRegisterComponent<Recipient, Caller> (Recipient r, Caller c) where Recipient : Object, HasRegister<Caller> where Caller : Component {
      Recipient component;
      if (r != null) {
        component = r;  //.GetComponent<Recipient>();
      } else if (c.GetComponentInParent<Recipient> () != null) {
        component = c.GetComponentInParent<Recipient> ();
      } else {
        component = Object.FindObjectOfType<Recipient> ();
      }
      if (component != null) {
        component.Register (c);
      } else {
        Debug.Log (System.String.Format ("Could not find a {0} recipient during registeration", typeof(Recipient).ToString ()));
      }
      return component;
    }

    public static Recipient MaybeRegisterNamedComponent<Recipient, Caller> (Recipient r, Caller c, string identifier) where Recipient : Object, HasRegister<Caller> where Caller : Component {
      Recipient component;
      if (r != null) {
        component = r;
      } else if (c.GetComponentInParent<Recipient> () != null) {
        component = c.GetComponentInParent<Recipient> ();
      } else {
        component = Object.FindObjectOfType<Recipient> ();
      }
      if (component != null) {
        component.Register (c, identifier);
      } else {
        Debug.Log (System.String.Format ("Could not find a {0} recipient during registeration", typeof(Recipient).ToString ()));
      }
      return component;
    }


    /// Use this method to get all loaded objects of some type, including inactive objects. 
    /// This is an alternative to Resources.FindObjectsOfTypeAll (returns project assets, including prefabs), and GameObject.FindObjectsOfTypeAll (deprecated).
    public static T[] FindAllObjectsOfTypeInScene<T> () {//(Scene scene) {
      List<T> results = new List<T> ();
      for (int i = 0; i < SceneManager.sceneCount; i++) {
        var s = SceneManager.GetSceneAt (i); // maybe EditorSceneManager
        if (s.isLoaded) {
          var allGameObjects = s.GetRootGameObjects ();
          for (int j = 0; j < allGameObjects.Length; j++) {
            var go = allGameObjects [j];
            results.AddRange (go.GetComponentsInChildren<T> (true));
          }
        }
      }
      return results.ToArray ();
    }

    public static GameObject[] FindAllGameObjectsExceptLayer (int layer) {
      var goa = GameObject.FindObjectsOfType<GameObject> ();
      var gol = new List<GameObject> ();
      foreach (var go in goa) {
        if (go.layer != layer) {
          gol.Add (go);
        }
      }
      if (gol.Count == 0) {
        return null;
      }
      return gol.ToArray ();
    }

    public static GameObject[] ChildGameObjectsExceptLayer (Transform parent, int layer) {
      var gol = new List<GameObject> ();
      foreach (Transform go in parent) {
        if (go.gameObject.layer != layer) {
          gol.Add (go.gameObject);
        }
      }
      if (gol.Count == 0) {
        return null;
      }
      return gol.ToArray ();
    }

    /** Contains logic for coverting a camera component into a Texture2D. */
    /*public Texture2D ObservationToTex(Camera camera, int width, int height)
        {
          Camera cam = camera;
          Rect oldRec = camera.rect;
          cam.rect = new Rect(0f, 0f, 1f, 1f);
          bool supportsAntialiasing = false;
          bool needsRescale = false;
          var depth = 24;
          var format = RenderTextureFormat.Default;
          var readWrite = RenderTextureReadWrite.Default;
          var antiAliasing = (supportsAntialiasing) ? Mathf.Max(1, QualitySettings.antiAliasing) : 1;

          var finalRT =
            RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
          var renderRT = (!needsRescale) ? finalRT :
            RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
          var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

          var prevActiveRT = RenderTexture.active;
          var prevCameraRT = cam.targetTexture;

          // render to offscreen texture (readonly from CPU side)
          RenderTexture.active = renderRT;
          cam.targetTexture = renderRT;

          cam.Render();

          if (needsRescale)
          {
            RenderTexture.active = finalRT;
            Graphics.Blit(renderRT, finalRT);
            RenderTexture.ReleaseTemporary(renderRT);
          }

          tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
          tex.Apply();
          cam.targetTexture = prevCameraRT;
          cam.rect = oldRec;
          RenderTexture.active = prevActiveRT;
          RenderTexture.ReleaseTemporary(finalRT);
          return tex;
        }

        /// Contains logic to convert the agent's cameras into observation list
        ///  (as list of float arrays)
        public List<float[,,,]> GetObservationMatrixList(List<int> agent_keys)
        {
          List<float[,,,]> observation_matrix_list = new List<float[,,,]>();
          Dictionary<int, List<Camera>> observations = CollectObservations();
          for (int obs_number = 0; obs_number < brainParameters.cameraResolutions.Length; obs_number++)
          {
            int width = brainParameters.cameraResolutions[obs_number].width;
            int height = brainParameters.cameraResolutions[obs_number].height;
            bool bw = brainParameters.cameraResolutions[obs_number].blackAndWhite;
            int pixels = 0;
            if (bw)
              pixels = 1;
            else
              pixels = 3;
            float[,,,] observation_matrix = new float[agent_keys.Count
              , height
              , width
              , pixels];
            int i = 0;
            foreach (int k in agent_keys)
            {
              Camera agent_obs = observations[k][obs_number];
              Texture2D tex = ObservationToTex(agent_obs, width, height);
              for (int w = 0; w < width; w++)
              {
                for (int h = 0; h < height; h++)
                {
                  Color c = tex.GetPixel(w, h);
                  if (!bw)
                  {
                    observation_matrix[i, tex.height - h - 1, w, 0] = c.r;
                    observation_matrix[i, tex.height - h - 1, w, 1] = c.g;
                    observation_matrix[i, tex.height - h - 1, w, 2] = c.b;
                  }
                  else
                  {
                    observation_matrix[i, tex.height - h - 1, w, 0] = (c.r + c.g + c.b) / 3;
                  }
                }
              }
              UnityEngine.Object.DestroyImmediate(tex);
              Resources.UnloadUnusedAssets();
              i++;
            }
            observation_matrix_list.Add(observation_matrix);
          }
          return observation_matrix_list;
        }*/
  }

}
