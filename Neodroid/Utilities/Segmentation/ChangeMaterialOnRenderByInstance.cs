using System.Collections.Generic;
using Neodroid.Scripts.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Segmentation {
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByInstance : MonoBehaviour {
    Renderer[] _all_renders;
    MaterialPropertyBlock _block;

    LinkedList<Color>[] _original_colors;

    public Dictionary<GameObject, Color> InstanceColorsDict { get; private set; }

    public ColorByInstance[] InstanceColors {
      get {
        if (this.InstanceColorsDict != null) {
          var instance_color_array = new ColorByInstance[this.InstanceColorsDict.Keys.Count];
          var i = 0;
          foreach (var key in this.InstanceColorsDict.Keys) {
            var seg = new ColorByInstance {Obj = key, Col = this.InstanceColorsDict[key]};
            instance_color_array[i] = seg;
            i++;
          }

          return instance_color_array;
        }

        return null;
      }
      set {
        foreach (var seg in value)
          this.InstanceColorsDict[seg.Obj] = seg.Col;
      }
    }

    // Use this for initialization
    void Start() { this.Setup(); }

    void Awake() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();
      this.Setup();
    }

    // Update is called once per frame
    void Update() {
      if (this.InstanceColorsDict == null)
        this.Setup();
      else if (this.InstanceColorsDict.Keys.Count != FindObjectsOfType<Renderer>().Length)
        this.Setup();
    }

    void Setup() {
      if (this._block == null)
        this._block = new MaterialPropertyBlock();

      this.InstanceColorsDict = new Dictionary<GameObject, Color>(this._all_renders.Length);
      foreach (var rend in this._all_renders)
        this.InstanceColorsDict.Add(rend.gameObject, Random.ColorHSV());
    }

    void Change() {
      this._original_colors = new LinkedList<Color>[this._all_renders.Length];

      for (var i = 0; i < this._original_colors.Length; i++)
        this._original_colors[i] = new LinkedList<Color>();

      for (var i = 0; i < this._all_renders.Length; i++) {
        foreach (var mat in this._all_renders[i].sharedMaterials) {
          if (mat != null && mat.HasProperty("_Color"))
            this._original_colors[i].AddFirst(mat.color);
          this._block.SetColor("_Color", this.InstanceColorsDict[this._all_renders[i].gameObject]);
          this._all_renders[i].SetPropertyBlock(this._block);
        }
      }
    }

    void Restore() {
      for (var i = 0; i < this._all_renders.Length; i++) {
        foreach (var mat in this._all_renders[i].sharedMaterials) {
          if (mat != null
              && mat.HasProperty("_Color")
              && this._original_colors != null
              && i < this._original_colors.Length) {
            this._block.SetColor("_Color", this._original_colors[i].Last.Value);
            this._original_colors[i].RemoveLast();
            this._all_renders[i].SetPropertyBlock(this._block);
          }
        }
      }
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
