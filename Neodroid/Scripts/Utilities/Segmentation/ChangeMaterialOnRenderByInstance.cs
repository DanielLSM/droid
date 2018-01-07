using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Segmentation {
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByInstance : MonoBehaviour {
    private Renderer[] _all_renders;
    private MaterialPropertyBlock _block;

    private LinkedList<Color>[] _original_colors;

    public Dictionary<GameObject, Color> InstanceColorsDict { get; private set; }

    public SegmentationColorByInstance[] InstanceColors {
      get {
        if (InstanceColorsDict != null) {
          var instance_color_array = new SegmentationColorByInstance[InstanceColorsDict.Keys.Count];
          var i = 0;
          foreach (var key in InstanceColorsDict.Keys) {
            var seg = new SegmentationColorByInstance {
                                                        game_object = key,
                                                        color = InstanceColorsDict[key]
                                                      };
            instance_color_array[i] = seg;
            i++;
          }

          return instance_color_array;
        }

        return null;
      }
      set {
        foreach (var seg in value) InstanceColorsDict[seg.game_object] = seg.color;
      }
    }

    // Use this for initialization
    private void Start() { Setup(); }

    private void Awake() { Setup(); }

    // Update is called once per frame
    private void Update() {
      if (InstanceColorsDict == null)
        Setup();
      else if (InstanceColorsDict.Keys.Count != FindObjectsOfType<Renderer>().Length)
        Setup();
    }

    private void Setup() {
      _all_renders = FindObjectsOfType<Renderer>();
      _block = new MaterialPropertyBlock();

      InstanceColorsDict = new Dictionary<GameObject, Color>(_all_renders.Length);
      foreach (var rend in _all_renders)
        InstanceColorsDict.Add(
                               rend.gameObject,
                               Random.ColorHSV());
    }

    private void Change() {
      _original_colors = new LinkedList<Color>[_all_renders.Length];

      for (var i = 0; i < _original_colors.Length; i++) _original_colors[i] = new LinkedList<Color>();

      for (var i = 0; i < _all_renders.Length; i++)
        foreach (var mat in _all_renders[i].sharedMaterials) {
          if (mat != null) _original_colors[i].AddFirst(mat.color);
          _block.SetColor(
                          "_Color",
                          InstanceColorsDict[_all_renders[i].gameObject]);
          _all_renders[i].SetPropertyBlock(_block);
        }
    }

    private void Restore() {
      for (var i = 0; i < _all_renders.Length; i++)
        foreach (var mat in _all_renders[i].sharedMaterials)
          if (mat != null) {
            _block.SetColor(
                            "_Color",
                            _original_colors[i].Last.Value);
            _original_colors[i].RemoveLast();
            _all_renders[i].SetPropertyBlock(_block);
          }
    }

    private void OnPreRender() {
      // change
      Change();
    }

    private void OnPostRender() {
      // change back
      Restore();
    }
  }
}
