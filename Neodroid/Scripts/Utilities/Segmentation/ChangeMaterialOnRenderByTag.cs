using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Segmentation {
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : MonoBehaviour {
    Renderer[] _all_renders;

    MaterialPropertyBlock _block;
    public SegmentationColorByTag[] _colors_by_tag;
    LinkedList<Color>[] _original_colors;

    public bool _replace_untagged_color = true;

    Dictionary<string, Color> _tag_colors;
    public Color _untagged_color = Color.black;

    public SegmentationColorByTag[] SegmentationColorsByTag { get { return this._colors_by_tag; } }

    void Awake() { this.Setup(); }

    void Update() {
      this.Setup(); // renderes maybe be disable and enabled, that is why every update we find all renderers again
    }

    void Setup() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();

      this._tag_colors = new Dictionary<string, Color>();
      if (this._colors_by_tag.Length > 0)
        foreach (var tag_color in this._colors_by_tag)
          if (!this._tag_colors.ContainsKey(key : tag_color.Tag))
            this._tag_colors.Add(
                                 key : tag_color.Tag,
                                 value : tag_color.Col);
    }

    void Change() {
      this._original_colors = new LinkedList<Color>[this._all_renders.Length];
      for (var i = 0; i < this._original_colors.Length; i++)
        this._original_colors[i] = new LinkedList<Color>();

      for (var i = 0; i < this._all_renders.Length; i++)
        if (this._tag_colors != null && this._tag_colors.ContainsKey(key : this._all_renders[i].tag))
          foreach (var mat in this._all_renders[i].sharedMaterials) {
            if (mat != null) this._original_colors[i].AddFirst(value : mat.color);
            this._block.SetColor(
                                 name : "_Color",
                                 value : this._tag_colors[key : this._all_renders[i].tag]);
            this._all_renders[i].SetPropertyBlock(properties : this._block);
          }
        else if (this._replace_untagged_color)
          foreach (var mat in this._all_renders[i].sharedMaterials) {
            if (mat != null) this._original_colors[i].AddFirst(value : mat.color);
            this._block.SetColor(
                                 name : "_Color",
                                 value : this._untagged_color);
            this._all_renders[i].SetPropertyBlock(properties : this._block);
          }
    }

    void Restore() {
      for (var i = 0; i < this._all_renders.Length; i++)
        foreach (var mat in this._all_renders[i].sharedMaterials)
          if (mat != null) {
            this._block.SetColor(
                                 name : "_Color",
                                 value : this._original_colors[i].Last.Value);
            this._original_colors[i].RemoveLast();
            this._all_renders[i].SetPropertyBlock(properties : this._block);
          }
    }

    void OnPreCull() {
      // change
    }

    void OnPreRender() {
      // change
      this.Change();
    }

    void OnPostRender() {
      // change back
      this.Restore();
    }
  }
}
