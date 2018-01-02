using Neodroid.Utilities;
using UnityEngine;
using Neodroid.Messaging.Messages;

namespace Neodroid.Configurables {

  public class QuaternionTransformConfigurable : ConfigurableGameObject {
    [Header ("Specfic", order = 102)]
    [SerializeField]
    Vector3 _position;
    [SerializeField]
    Quaternion _rotation;
    [SerializeField]
    string _X;
    [SerializeField]
    string _Y;
    [SerializeField]
    string _Z;

    public Quaternion Rotation {
      get {
        return _rotation;
      }
    }

    public Vector3 Position {
      get {
        return _position;
      }
    }

    public QuaternionTransformConfigurable () {


    }
  }
}

