using UnityEngine;

namespace Neodroid.Configurables {
  public class QuaternionTransformConfigurable : ConfigurableGameObject {
    [Header(
      "Specfic",
      order = 102)]
    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private Quaternion _rotation;

    [SerializeField]
    private string _X;

    [SerializeField]
    private string _Y;

    [SerializeField]
    private string _Z;

    public Quaternion Rotation { get { return _rotation; } }

    public Vector3 Position { get { return _position; } }
  }
}
