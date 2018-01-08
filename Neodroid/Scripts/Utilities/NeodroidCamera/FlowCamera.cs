using UnityEngine;

namespace Neodroid.Scripts.Utilities.NeodroidCamera {
  [ExecuteInEditMode]
  [RequireComponent( typeof(Camera))]
  public class FlowCamera : MonoBehaviour {
    [SerializeField]
    Color _background_color = Color.white;

    [SerializeField]
    [Range(
      min : 0,
      max : 1)]
    float _blending = 0.5f;

    Material _material;

    [SerializeField]
    [Range(
      min : 0,
      max : 100)]
    float _overlay_amplitude = 60;

    [SerializeField]
    Shader _shader;

    void Start() {
      this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
      if (this._material == null)
        this._material = new Material(shader : this._shader) {
                                                               hideFlags = HideFlags.DontSave
                                                             };

      this._material.SetColor(
                              name : "_BackgroundColor",
                              value : this._background_color);
      this._material.SetFloat(
                              name : "_Blending",
                              value : this._blending);
      this._material.SetFloat(
                              name : "_Amplitude",
                              value : this._overlay_amplitude);
      Graphics.Blit(
                    source : source,
                    dest : destination,
                    mat : this._material);
    }

    void OnDestroy() {
      if (this._material != null)
        if (Application.isPlaying)
          Destroy(obj : this._material);
        else
          DestroyImmediate(obj : this._material);
    }
  }
}
