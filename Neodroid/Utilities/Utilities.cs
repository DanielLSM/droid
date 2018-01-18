using System.Collections.Generic;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Neodroid.Scripts.Utilities {
  public static class NeodroidUtilities {
    public static float KineticEnergy(Rigidbody rb) {
      return
          0.5f
          * rb.mass
          * Mathf.Pow(
              rb.velocity.magnitude,
              2); // mass in kg, velocity in meters per second, result is joules
    }

    public static Vector3 Vector3Clamp(ref Vector3 vec, Vector3 min_point, Vector3 max_point) {
      vec.x = Mathf.Clamp(vec.x, min_point.x, max_point.x);
      vec.y = Mathf.Clamp(vec.y, min_point.y, max_point.y);
      vec.z = Mathf.Clamp(vec.z, min_point.z, max_point.z);
      return vec;
    }

    public static void DrawLine(Vector3 p1, Vector3 p2, float width) {
      var count = Mathf.CeilToInt(width); // how many lines are needed.
      if (count == 1)
        Gizmos.DrawLine(p1, p2);
      else {
        var c = Camera.current;
        if (c == null) {
          Debug.LogError("Camera.current is null");
          return;
        }

        var v1 = (p2 - p1).normalized; // line direction
        var v2 = (c.transform.position - p1).normalized; // direction to camera
        var n = Vector3.Cross(v1, v2); // normal vector
        for (var i = 0; i < count; i++) {
          //Vector3 o = n * width ((float)i / (count - 1) - 0.5f);
          var o = n * width * ((float)i / (count - 1) - 0.5f);
          Gizmos.DrawLine(p1 + o, p2 + o);
        }
      }
    }

    public static Vector3[] ExtractCorners(
        Vector3 v3Center,
        Vector3 v3Extents,
        Transform reference_transform = null) {
      var v3FrontTopLeft = new Vector3(
          v3Center.x - v3Extents.x,
          v3Center.y + v3Extents.y,
          v3Center.z - v3Extents.z); // Front top left corner
      var v3FrontTopRight = new Vector3(
          v3Center.x + v3Extents.x,
          v3Center.y + v3Extents.y,
          v3Center.z - v3Extents.z); // Front top right corner
      var v3FrontBottomLeft = new Vector3(
          v3Center.x - v3Extents.x,
          v3Center.y - v3Extents.y,
          v3Center.z - v3Extents.z); // Front bottom left corner
      var v3FrontBottomRight = new Vector3(
          v3Center.x + v3Extents.x,
          v3Center.y - v3Extents.y,
          v3Center.z - v3Extents.z); // Front bottom right corner
      var v3BackTopLeft = new Vector3(
          v3Center.x - v3Extents.x,
          v3Center.y + v3Extents.y,
          v3Center.z + v3Extents.z); // Back top left corner
      var v3BackTopRight = new Vector3(
          v3Center.x + v3Extents.x,
          v3Center.y + v3Extents.y,
          v3Center.z + v3Extents.z); // Back top right corner
      var v3BackBottomLeft = new Vector3(
          v3Center.x - v3Extents.x,
          v3Center.y - v3Extents.y,
          v3Center.z + v3Extents.z); // Back bottom left corner
      var v3BackBottomRight = new Vector3(
          v3Center.x + v3Extents.x,
          v3Center.y - v3Extents.y,
          v3Center.z + v3Extents.z); // Back bottom right corner
      if (reference_transform) {
        v3FrontTopLeft = reference_transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = reference_transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = reference_transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = reference_transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = reference_transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = reference_transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = reference_transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = reference_transform.TransformPoint(v3BackBottomRight);
      }

      return new[] {
          v3FrontTopLeft,
          v3FrontTopRight,
          v3FrontBottomLeft,
          v3FrontBottomRight,
          v3BackTopLeft,
          v3BackTopRight,
          v3BackBottomLeft,
          v3BackBottomRight
      };
    }

    public static void DrawBox(
        Vector3 v3FrontTopLeft,
        Vector3 v3FrontTopRight,
        Vector3 v3FrontBottomLeft,
        Vector3 v3FrontBottomRight,
        Vector3 v3BackTopLeft,
        Vector3 v3BackTopRight,
        Vector3 v3BackBottomLeft,
        Vector3 v3BackBottomRight,
        Color color) {
      Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, color);
      Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, color);
      Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, color);
      Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, color);

      Debug.DrawLine(v3BackTopLeft, v3BackTopRight, color);
      Debug.DrawLine(v3BackTopRight, v3BackBottomRight, color);
      Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, color);
      Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, color);

      Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, color);
      Debug.DrawLine(v3FrontTopRight, v3BackTopRight, color);
      Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, color);
      Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, color);
    }

    public static AnimationCurve DefaultAnimationCurve() {
      return new AnimationCurve(new Keyframe(1, 1), new Keyframe(0, 0));
    }

    public static Gradient DefaultGradient() {
      var gradient = new Gradient {
          // The number of keys must be specified in this array initialiser
          colorKeys = new[] {
              // Add your colour and specify the stop point
              new GradientColorKey(new Color(1, 1, 1), 0),
              new GradientColorKey(new Color(1, 1, 1), 1f),
              new GradientColorKey(new Color(1, 1, 1), 0)
          },
          // This sets the alpha to 1 at both ends of the gradient
          alphaKeys = new[] {
              new GradientAlphaKey(1, 0),
              new GradientAlphaKey(1, 1),
              new GradientAlphaKey(1, 0)
          }
      };

      return gradient;
    }

    public static Texture2D RenderTextureImage(Camera camera) {
      // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      var current_render_texture = RenderTexture.active;
      RenderTexture.active = camera.targetTexture;
      camera.Render();
      var texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
      texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
      texture.Apply();
      RenderTexture.active = current_render_texture;
      return texture;
    }

    public static void RegisterCollisionTriggerCallbacksOnChildren(
        Component caller,
        Transform parent,
        ChildSensor.OnChildCollisionEnterDelegate on_collision_enter_child,
        ChildSensor.OnChildTriggerEnterDelegate on_trigger_enter_child = null,
        ChildSensor.OnChildCollisionExitDelegate on_collision_exit_child = null,
        ChildSensor.OnChildTriggerExitDelegate on_trigger_exit_child = null,
        ChildSensor.OnChildCollisionStayDelegate on_collision_stay_child = null,
        ChildSensor.OnChildTriggerStayDelegate on_trigger_stay_child = null,
        bool debug = false) {
      var children_with_colliders = parent.GetComponentsInChildren<Collider>();

      foreach (var child in children_with_colliders) {
        var child_sensors = child.GetComponents<ChildSensor>();
        ChildSensor sensor = null;
        foreach (var child_sensor in child_sensors) {
          if (child_sensor.Caller != null && child_sensor.Caller == caller) {
            sensor = child_sensor;
            break;
          }

          if (child_sensor.Caller == null) {
            child_sensor.Caller = caller;
            sensor = child_sensor;
            break;
          }
        }

        if (sensor == null) {
          sensor = child.gameObject.AddComponent<ChildSensor>();
          sensor.Caller = caller;
        }

        if (on_collision_enter_child != null)
          sensor.OnCollisionEnterDelegate = on_collision_enter_child;
        if (on_trigger_enter_child != null)
          sensor.OnTriggerEnterDelegate = on_trigger_enter_child;
        if (on_collision_exit_child != null)
          sensor.OnCollisionExitDelegate = on_collision_exit_child;
        if (on_trigger_exit_child != null)
          sensor.OnTriggerExitDelegate = on_trigger_exit_child;
        if (on_trigger_stay_child != null)
          sensor.OnTriggerStayDelegate = on_trigger_stay_child;
        if (on_collision_stay_child != null)
          sensor.OnCollisionStayDelegate = on_collision_stay_child;
        if (debug) {
          Debug.Log(
              caller.name
              + " has created "
              + sensor.name
              + " on "
              + child.name
              + " under parent "
              + parent.name);
        }
      }
    }

    public static string ColorArrayToString(IEnumerable<Color> colors) {
      var s = "";
      foreach (var color in colors)
        s += color.ToString();
      return s;
    }

    public static TRecipient MaybeRegisterComponent<TRecipient, TCaller>(TRecipient r, TCaller c)
        where TRecipient : Object, IHasRegister<TCaller> where TCaller : Component {
      TRecipient component;
      if (r != null)
        component = r; //.GetComponent<Recipient>();
      else if (c.GetComponentInParent<TRecipient>() != null)
        component = c.GetComponentInParent<TRecipient>();
      else
        component = Object.FindObjectOfType<TRecipient>();
      if (component != null)
        component.Register(c);
      else
        Debug.Log(string.Format("Could not find a {0} recipient during registeration", typeof(TRecipient)));
      return component;
    }

    public static TRecipient MaybeRegisterNamedComponent<TRecipient, TCaller>(
        TRecipient r,
        TCaller c,
        string identifier) where TRecipient : Object, IHasRegister<TCaller> where TCaller : Component {
      TRecipient component;
      if (r != null)
        component = r;
      else if (c.GetComponentInParent<TRecipient>() != null)
        component = c.GetComponentInParent<TRecipient>();
      else
        component = Object.FindObjectOfType<TRecipient>();
      if (component != null)
        component.Register(c, identifier);
      else
        Debug.Log(string.Format("Could not find a {0} recipient during registeration", typeof(TRecipient)));
      return component;
    }

    /// Use this method to get all loaded objects of some type, including inactive objects.
    /// This is an alternative to Resources.FindObjectsOfTypeAll (returns project assets, including prefabs), and GameObject.FindObjectsOfTypeAll (deprecated).
    public static T[] FindAllObjectsOfTypeInScene<T>() {
      //(Scene scene) {
      var results = new List<T>();
      for (var i = 0; i < SceneManager.sceneCount; i++) {
        var s = SceneManager.GetSceneAt(i); // maybe EditorSceneManager
        if (!s.isLoaded)
          continue;
        var all_game_objects = s.GetRootGameObjects();
        foreach (var go in all_game_objects)
          results.AddRange(go.GetComponentsInChildren<T>(true));
      }

      return results.ToArray();
    }

    public static GameObject[] FindAllGameObjectsExceptLayer(int layer) {
      var goa = Object.FindObjectsOfType<GameObject>();
      var gol = new List<GameObject>();
      foreach (var go in goa) {
        if (go.layer != layer)
          gol.Add(go);
      }

      return gol.Count == 0 ? null : gol.ToArray();
    }

    public static GameObject[] RecursiveChildGameObjectsExceptLayer(Transform parent, int layer) {
      var gol = new List<GameObject>();
      foreach (Transform go in parent) {
        if (go) {
          if (go.gameObject.layer != layer) {
            gol.Add(go.gameObject);
            var children = RecursiveChildGameObjectsExceptLayer(go, layer);
            if (children != null && children.Length > 0)
              gol.AddRange(children);
          }
        }
      }

      return gol.Count == 0 ? null : gol.ToArray();
    }
    #if UNITY_EDITOR
    public static void DrawString(
        string text,
        Vector3 world_pos,
        Color? color = null,
        float o_x = 0,
        float o_y = 0) {
      Handles.BeginGUI();

      var restore_color = GUI.color;

      if (color.HasValue)
        GUI.color = color.Value;
      var view = SceneView.currentDrawingSceneView;
      var screen_pos = view.camera.WorldToScreenPoint(world_pos);

      if (screen_pos.y < 0
          || screen_pos.y > Screen.height
          || screen_pos.x < 0
          || screen_pos.x > Screen.width
          || screen_pos.z < 0) {
        GUI.color = restore_color;
        Handles.EndGUI();
        return;
      }

      Handles.Label(TransformByPixel(world_pos, o_x, o_y), text);

      GUI.color = restore_color;
      Handles.EndGUI();
    }

    public static Vector3 TransformByPixel(Vector3 position, float x, float y) {
      return TransformByPixel(position, new Vector3(x, y));
    }

    public static Vector3 TransformByPixel(Vector3 position, Vector3 translate_by) {
      var cam = SceneView.currentDrawingSceneView.camera;
      return cam ? cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translate_by) : position;
    }
    #endif

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
