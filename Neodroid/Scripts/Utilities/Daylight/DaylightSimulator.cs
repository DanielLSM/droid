using Neodroid.Environments;
using Neodroid.Utilities;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(ParticleSystem))]
public class DaylightSimulator : Resetable {
  public float _day_atmosphere_thickness = 0.88f;
  public AnimationCurve _fog_density_curve = NeodroidUtilities.DefaultAnimationCurve();
  public Gradient _fog_gradient = NeodroidUtilities.DefaultGradient();
  private Light _light;

  public Gradient _light_gradient = NeodroidUtilities.DefaultGradient();

  public float _max_ambient = 1f;

  public float _max_intensity = 1.34f;
  public float _min_ambient = 0.01f;
  public float _min_ambient_point = -0.2f;
  public float _min_intensity = 0.02f;
  public float _min_point = -0.2f;
  public float _night_atmosphere_thickness = 1.03f;

  private ParticleSystem _particle_system;
  private ParticleSystem.Particle[] _particles;

  public Vector3 _rotation = new Vector3(
                                         1f,
                                         0f,
                                         1f);

  private Material _sky_mat;
  public float _speed_multiplier = 1f;

  public Quaternion _start_rotation = Quaternion.identity;
  public float fogScale = 0.2f;

  public override string ResetableIdentifier { get { return name; } }

  private void Awake() {
    Setup();

    _light = GetComponent<Light>();
    _sky_mat = RenderSettings.skybox;
    transform.rotation = _start_rotation;
  }

  private void Setup() {
    if (!_particle_system) _particle_system = GetComponent<ParticleSystem>();
    _particle_system.Pause();
    if (_particles == null || _particles.Length < _particle_system.main.maxParticles)
      _particles = new ParticleSystem.Particle[_particle_system.main.maxParticles];
  }

  private void Update() {
    Setup();

    var a = 1 - _min_point;
    var dot = Mathf.Clamp01(
                            (Vector3.Dot(
                                         _light.transform.forward,
                                         Vector3.down)
                             - _min_point)
                            / a);
    var intensity = (_max_intensity - _min_intensity) * dot + _min_intensity;

    _light.intensity = intensity;

    var _stars_intensity = _min_intensity / intensity;
    var particle_color = new Color(
                                   1f,
                                   1f,
                                   1f,
                                   _stars_intensity);

    var num_alive_particles = _particle_system.GetParticles(_particles);
    for (var i = 0; i < num_alive_particles; i++) _particles[i].startColor = particle_color;
    _particle_system.SetParticles(
                                  _particles,
                                  num_alive_particles);

    a = 1 - _min_ambient_point;
    dot = Mathf.Clamp01(
                        (Vector3.Dot(
                                     _light.transform.forward,
                                     Vector3.down)
                         - _min_ambient_point)
                        / a);
    var ambient_intensity = (_max_ambient - _min_ambient) * dot + _min_ambient;
    RenderSettings.ambientIntensity = ambient_intensity;

    _light.color = _light_gradient.Evaluate(dot);
    RenderSettings.ambientLight = _light.color;

    RenderSettings.fogColor = _fog_gradient.Evaluate(dot);
    RenderSettings.fogDensity = _fog_density_curve.Evaluate(dot) * fogScale;

    var atmosphere_thickness = (_day_atmosphere_thickness - _night_atmosphere_thickness) * dot
                               + _night_atmosphere_thickness;
    _sky_mat.SetFloat(
                      "_AtmosphereThickness",
                      atmosphere_thickness);

    transform.Rotate(_rotation * Time.deltaTime * _speed_multiplier);
  }

  public override void Reset() { }
}
