using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FlowCamera : MonoBehaviour {
  public Color _background_color = Color.white;

  [SerializeField]
  [Range(
    0,
    1)]
  public float _blending = 0.5f;

  private Material _material;

  [SerializeField]
  [Range(
    0,
    100)]
  public float _overlay_amplitude = 60;

  public Shader _shader;

  private void Start() {
    GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination) {
    if (_material == null) {
      _material = new Material(_shader) {
                                          hideFlags = HideFlags.DontSave
                                        };
    }

    _material.SetColor(
                       "_BackgroundColor",
                       _background_color);
    _material.SetFloat(
                       "_Blending",
                       _blending);
    _material.SetFloat(
                       "_Amplitude",
                       _overlay_amplitude);
    Graphics.Blit(
                  source,
                  destination,
                  _material);
  }

  private void OnDestroy() {
    if (_material != null)
      if (Application.isPlaying)
        Destroy(_material);
      else
        DestroyImmediate(_material);
  }
}
