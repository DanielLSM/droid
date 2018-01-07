using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Segmentation {
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : MonoBehaviour {
    private Renderer[] _all_renders;

    private MaterialPropertyBlock _block;
    public SegmentationColorByTag[] _colors_by_tag;
    private LinkedList<Color>[] _original_colors;

    public bool _replace_untagged_color = true;

    private Dictionary<string, Color> _tag_colors;
    public Color _untagged_color = Color.black;

    public SegmentationColorByTag[] SegmentationColorsByTag { get { return _colors_by_tag; } }

    private void Awake() { Setup(); }

    private void Update() {
      Setup(); // renderes maybe be disable and enabled, that is why every update we find all renderers again
    }

    private void Setup() {
      _all_renders = FindObjectsOfType<Renderer>();
      _block = new MaterialPropertyBlock();

      _tag_colors = new Dictionary<string, Color>();
      if (_colors_by_tag.Length > 0)
        foreach (var tag_color in _colors_by_tag)
          if (!_tag_colors.ContainsKey(tag_color.tag))
            _tag_colors.Add(
                            tag_color.tag,
                            tag_color.color);
    }

    private void Change() {
      _original_colors = new LinkedList<Color>[_all_renders.Length];
      for (var i = 0; i < _original_colors.Length; i++) _original_colors[i] = new LinkedList<Color>();

      for (var i = 0; i < _all_renders.Length; i++)
        if (_tag_colors != null && _tag_colors.ContainsKey(_all_renders[i].tag))
          foreach (var mat in _all_renders[i].sharedMaterials) {
            if (mat != null) _original_colors[i].AddFirst(mat.color);
            _block.SetColor(
                            "_Color",
                            _tag_colors[_all_renders[i].tag]);
            _all_renders[i].SetPropertyBlock(_block);
          }
        else if (_replace_untagged_color)
          foreach (var mat in _all_renders[i].sharedMaterials) {
            if (mat != null) _original_colors[i].AddFirst(mat.color);
            _block.SetColor(
                            "_Color",
                            _untagged_color);
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

    private void OnPreCull() {
      // change
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
