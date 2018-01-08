using Neodroid.Models.Configurables.General;
using UnityEngine;

namespace Neodroid.Models.Configurables {
  public class QuaternionTransformConfigurable : ConfigurableGameObject {
    [Header("Specfic", order = 102)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _x;

    [SerializeField] string _y;

    [SerializeField] string _z;

    public Quaternion Rotation { get { return this._rotation; } }

    public Vector3 Position { get { return this._position; } }
  }
}
