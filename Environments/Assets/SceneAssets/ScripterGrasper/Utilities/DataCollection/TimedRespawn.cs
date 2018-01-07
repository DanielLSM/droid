using System.Collections;
using Neodroid.Utilities;
using SceneSpecificAssets.Grasping.Grasps;
using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities.DataCollection {
  public class TimedRespawn : MonoBehaviour {
    private Grasp _grasp;
    public GraspableObject _graspable_object;
    public ScriptedGripper _gripper;
    private Vector3 _initial_position;
    private Quaternion _initial_rotation;
    private Rigidbody[] _rigid_bodies;
    private Rigidbody _rigid_body;

    public bool Debugging;

    // Use this for initialization
    private void Start() {
      if (!_graspable_object) _graspable_object = GetComponent<GraspableObject>();

      if (!_gripper) _gripper = FindObjectOfType<ScriptedGripper>();

      _grasp = _graspable_object.GetOptimalGrasp(_gripper).First;
      _rigid_body = _grasp.GetComponentInParent<Rigidbody>();
      _rigid_bodies = _graspable_object.GetComponentsInChildren<Rigidbody>();
      _initial_position = _rigid_body.transform.position;
      _initial_rotation = _rigid_body.transform.rotation;

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    this,
                                                                    transform,
                                                                    OnCollisionEnterChild,
                                                                    OnTriggerEnterChild,
                                                                    OnCollisionExitChild,
                                                                    OnTriggerExitChild,
                                                                    OnCollisionStayChild,
                                                                    OnTriggerStayChild,
                                                                    Debugging);
    }

    private void OnTriggerStayChild(GameObject child_game_object, Collider col) { }

    private void OnCollisionStayChild(GameObject child_game_object, Collision collision) { }

    private void OnCollisionEnterChild(GameObject child_game_object, Collision collision) {
      if (collision.gameObject.CompareTag("Floor")) {
        StopCoroutine("RespawnObject");
        StartCoroutine("RespawnObject");
      }
    }

    private IEnumerator RespawnObject() {
      yield return new WaitForSeconds(.5f);
      StopCoroutine("MakeObjectVisible");
      _graspable_object.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
      _rigid_body.transform.position = _initial_position;
      _rigid_body.transform.rotation = _initial_rotation;
      MakeRigidBodiesSleep();
      StartCoroutine("MakeObjectVisible");
    }

    private void MakeRigidBodiesSleep() {
      foreach (var body in _rigid_bodies) {
        body.useGravity = false;
        //body.isKinematic = true;
        body.Sleep();
      }

      //_rigid_body.isKinematic = true;
      //_rigid_body.useGravity = false;
      //_rigid_body.Sleep ();
    }

    private void WakeUpRigidBodies() {
      foreach (var body in _rigid_bodies) {
        //body.isKinematic = false;
        body.useGravity = true;
        body.WakeUp();
      }

      //_rigid_body.isKinematic = false;
      //_rigid_body.useGravity = true;
      //_rigid_body.WakeUp ();
    }

    private IEnumerator MakeObjectVisible() {
      yield return new WaitForSeconds(.5f);
      _rigid_body.transform.position = _initial_position;
      _rigid_body.transform.rotation = _initial_rotation;
      WakeUpRigidBodies();
      _graspable_object.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }

    private void OnTriggerEnterChild(GameObject child_game_object, Collider col) { }

    private void OnCollisionExitChild(GameObject child_game_object, Collision collision) {
      if (collision.gameObject.CompareTag("Floor")) StopCoroutine("RespawnObject");
    }

    private void OnTriggerExitChild(GameObject child_game_object, Collider col) { }

    // Update is called once per frame
    private void Update() { }
  }
}
