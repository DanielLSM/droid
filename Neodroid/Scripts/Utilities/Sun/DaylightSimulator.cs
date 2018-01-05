using System.Collections;
using UnityEngine;
using System.Diagnostics;
using Neodroid.Environments;
using Neodroid.Utilities;

[ExecuteInEditMode]
[RequireComponent (typeof(Light))]
public class DaylightSimulator : Resetable {

  public float _max_intensity = 1.34f;
  public float _min_intensity = 0.02f;
  public float _min_point = -0.2f;

  public float _max_ambient = 1f;
  public float _min_ambient = 0.01f;
  public float _min_ambient_point = -0.2f;

  public Gradient _light_gradient = NeodroidUtilities.DefaultGradient ();
  public Gradient _fog_gradient = NeodroidUtilities.DefaultGradient ();
  public AnimationCurve _fog_density_curve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));
  public float fogScale = 0.2f;

  public float _day_atmosphere_thickness = 0.88f;
  public float _night_atmosphere_thickness = 1.03f;

  public Quaternion _start_rotation = Quaternion.identity;
  public Vector3 _rotation = new Vector3 (1f, 0f, 1f);
  public float _speed_multiplier = 1f;

  Light _light;
  Material _sky_mat;

  void Start () {

    _light = GetComponent<Light> ();
    _sky_mat = RenderSettings.skybox;
    this.transform.rotation = _start_rotation;
  }

  void Update () {

    float tRange = 1 - _min_point;
    float dot = Mathf.Clamp01 ((Vector3.Dot (_light.transform.forward, Vector3.down) - _min_point) / tRange);
    float i = ((_max_intensity - _min_intensity) * dot) + _min_intensity;

    _light.intensity = i;

    tRange = 1 - _min_ambient_point;
    dot = Mathf.Clamp01 ((Vector3.Dot (_light.transform.forward, Vector3.down) - _min_ambient_point) / tRange);
    i = ((_max_ambient - _min_ambient) * dot) + _min_ambient;
    RenderSettings.ambientIntensity = i;

    _light.color = _light_gradient.Evaluate (dot);
    RenderSettings.ambientLight = _light.color;

    RenderSettings.fogColor = _fog_gradient.Evaluate (dot);
    RenderSettings.fogDensity = _fog_density_curve.Evaluate (dot) * fogScale;

    i = ((_day_atmosphere_thickness - _night_atmosphere_thickness) * dot) + _night_atmosphere_thickness;
    _sky_mat.SetFloat ("_AtmosphereThickness", i);

    transform.Rotate (_rotation * Time.deltaTime * _speed_multiplier);
  }

  public override void Reset () {

  }

  public override string ResetableIdentifier{ get { return this.name; } }
}
