using UnityEngine;

namespace Neodroid.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  public class ReplacementShaderEffect : MonoBehaviour {
    public Color _color;
    public string _replace_rendertype = "";
    public Shader _replacement_shader;

    private void OnValidate() {
      Shader.SetGlobalColor(
                            "_OverDrawColor",
                            _color);
      Shader.SetGlobalColor(
                            "_SegmentationColor",
                            _color);
      //Shader.SetGlobalColor ("_Color", _color);
    }

    private void OnEnable() {
      if (_replacement_shader != null)
        GetComponent<Camera>().SetReplacementShader(
                                                    _replacement_shader,
                                                    _replace_rendertype);
    }

    private void OnDisable() { GetComponent<Camera>().ResetReplacementShader(); }

    private void OnPreRender() { }
  }
}
