using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FlowCamera : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    public float _blending = 0.5f;
    public Shader _shader;
    [SerializeField, Range(0, 100)]
    public float _overlay_amplitude = 60;
    public Color _background_color = Color.white;
    Material _material;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        _material.SetColor("_BackgroundColor", _background_color);
        _material.SetFloat("_Blending", _blending);
        _material.SetFloat("_Amplitude", _overlay_amplitude);
        Graphics.Blit(source, destination, _material);
    }

    void OnDestroy()
    {
        if (_material != null)
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
    }

}
