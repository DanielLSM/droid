using UnityEngine;

namespace Neodroid.Scripts.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    [SerializeField]
    Color _color;
    [SerializeField] string _replace_rendertype = "";
    [SerializeField] Shader _replacement_shader;

    void OnValidate() {
      Shader.SetGlobalColor(
                            name : "_OverDrawColor",
                            value : this._color);
      Shader.SetGlobalColor(
                            name : "_SegmentationColor",
                            value : this._color);
      //Shader.SetGlobalColor ("_Color", _color);
    }

    void OnEnable() {
      if (this._replacement_shader != null)
        this.GetComponent<Camera>().SetReplacementShader(
                                                         shader : this._replacement_shader,
                                                         replacementTag : this._replace_rendertype);
    }

    void OnDisable() { this.GetComponent<Camera>().ResetReplacementShader(); }

    void OnPreRender() { }
  }
}
