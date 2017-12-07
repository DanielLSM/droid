using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neodroid.Segmentation {

  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : MonoBehaviour {

    public bool _replace_untagged_color = true;
    public Color _untagged_color = Color.black;
    public SegmentationColorByTag[] _colors_by_tag;

    MaterialPropertyBlock _block;

    Dictionary<string, Color> _tag_colors;
    LinkedList<Color>[] _original_colors;
    Renderer[] _all_renders;

    public SegmentationColorByTag[] SegmentationColorsByTag {
      get{ return _colors_by_tag; }
    }

    void Update () {
      Setup (); // renderes maybe be disable and enabled, that is why every update we find all renderers again
    }

    void Setup () {
      _all_renders = FindObjectsOfType<Renderer> ();
      _block = new MaterialPropertyBlock ();

      _tag_colors = new Dictionary<string, Color> ();
      if (_colors_by_tag.Length > 0) {
        foreach (var tag_color in _colors_by_tag) {
          if (!_tag_colors.ContainsKey (tag_color.tag)) {
            _tag_colors.Add (tag_color.tag, tag_color.color);
          }
        }
      }
    }

    void Change () {
      _original_colors = new LinkedList<Color>[_all_renders.Length];
      for (int i = 0; i < _original_colors.Length; i++) {
        _original_colors [i] = new LinkedList<Color> ();
      }

      for (int i = 0; i < _all_renders.Length; i++) {
        if (_tag_colors.ContainsKey (_all_renders [i].tag)) {
          foreach (var mat in _all_renders[i].sharedMaterials) {
            if (mat != null) {
              _original_colors [i].AddFirst (mat.color);
            }
            _block.SetColor ("_Color", _tag_colors [_all_renders [i].tag]);
            _all_renders [i].SetPropertyBlock (_block);
          }

        } else if (_replace_untagged_color) {
          foreach (var mat in _all_renders[i].sharedMaterials) {
            if (mat != null) {
              _original_colors [i].AddFirst (mat.color);
            }
            _block.SetColor ("_Color", _untagged_color);
            _all_renders [i].SetPropertyBlock (_block);
          }
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


    void OnPreCull () { // change
    }

    void OnPreRender () { // change
      Change ();
    }

    void OnPostRender () { // change back
      Restore ();
    }

  }
}