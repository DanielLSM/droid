using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeMaterialOnRenderByInstance : MonoBehaviour {
 
  public bool _use_shared_materials = false;

  Dictionary<GameObject, Color> _instance_colors;
  LinkedList<Color>[] _original_colors;
  Renderer[] _all_renders;
  MaterialPropertyBlock _block;

  public Dictionary<GameObject, Color> InstanceColorsDict {
    get{ return _instance_colors; }
  }

  public SegmentationColorByInstance[] InstanceColors {
    get {
      if (_instance_colors != null) {
        var instance_color_array = new SegmentationColorByInstance[_instance_colors.Keys.Count];
        int i = 0;
        foreach (var key in _instance_colors.Keys) {
          var seg = new SegmentationColorByInstance ();
          seg.game_object = key;
          seg.color = _instance_colors [key];
          instance_color_array [i] = seg;
          i++;
        }
        return instance_color_array;
      }
      return null;
    }

    set {
      foreach (SegmentationColorByInstance seg in value) {
        _instance_colors [seg.game_object] = seg.color;
      }
    }
  }

  // Use this for initialization
  void Start () {
    Setup ();
  }

  void Awake () {
    Setup ();
  }

  // Update is called once per frame
  void Update () {
    if (_instance_colors == null)
      Setup ();
    else if (_instance_colors.Keys.Count != FindObjectsOfType<Renderer> ().Length) {
      Setup ();
    }
  }

  void Setup () {
    _all_renders = FindObjectsOfType<Renderer> ();
    _block = new MaterialPropertyBlock ();

    _instance_colors = new Dictionary<GameObject, Color> (_all_renders.Length);
    foreach (Renderer renderer in _all_renders) {
      _instance_colors.Add (renderer.gameObject, Random.ColorHSV ());
    }


  }

  void Change () {
    _original_colors = new LinkedList<Color>[_all_renders.Length];

    for (int i = 0; i < _original_colors.Length; i++) {
      _original_colors [i] = new LinkedList<Color> ();
    }
      
    for (int i = 0; i < _all_renders.Length; i++) {
      foreach (var mat in _all_renders[i].sharedMaterials) {
        if (mat != null) {
          _original_colors [i].AddFirst (mat.color);
        }
        _block.SetColor ("_Color", _instance_colors [_all_renders [i].gameObject]);
        _all_renders [i].SetPropertyBlock (_block);
      }

    }
  }

  void Restore () {
    for (int i = 0; i < _all_renders.Length; i++) {
      foreach (var mat in _all_renders[i].sharedMaterials) {
        if (mat != null) {
          _block.SetColor ("_Color", _original_colors [i].Last.Value);
          _original_colors [i].RemoveLast ();
          _all_renders [i].SetPropertyBlock (_block);
        }
      }
    }
  }

  void OnPreRender () { // change
    Change ();
  }

  void OnPostRender () { // change back
    Restore ();
  }



}
