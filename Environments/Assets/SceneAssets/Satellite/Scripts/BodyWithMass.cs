using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BodyWithMass : MonoBehaviour {

  const float GRAVITATIONAL_CONSTANT = 667.4f;

  static BodyWithMass[] _attractors;

	Rigidbody _rigidbody;

  void Start(){
    if (!_rigidbody) {
      _rigidbody = GetComponent<Rigidbody> ();
    }
    if (_attractors == null) {
      _attractors = FindObjectsOfType<BodyWithMass> ();
    }
  }

  void Update(){
    if (!_rigidbody) {
      _rigidbody = GetComponent<Rigidbody> ();
    }
    if (_attractors == null) {
      _attractors = FindObjectsOfType<BodyWithMass> ();
    }
  }

	void FixedUpdate ()	{
		foreach (BodyWithMass attractor in _attractors){
			if (attractor != this)
				Attract(attractor);
		}
	}

  public float Mass{
    get{ return _rigidbody.mass; }
  }

  void Attract (BodyWithMass other_body)	{

    Vector3 direction = this.transform.position - other_body.transform.position;
    //float distance = direction.sqrMagnitude;
    float distance = direction.magnitude;

    if (Mathf.Approximately (distance, 0)) {
      return;
    }
      
    var nom = (this.Mass * other_body.Mass);
    var denom = distance * distance;

    float force_magnitude =  nom / denom;
    force_magnitude *= GRAVITATIONAL_CONSTANT;
		Vector3 force = direction.normalized * force_magnitude;

    other_body.ApplyForce(force);
	}

  public void ApplyForce(Vector3 force){
    _rigidbody.AddForce (force);
  }

}