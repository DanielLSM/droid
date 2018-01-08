using UnityEngine;

namespace SceneAssets.Robolab.Scripts {
  public class EditorLikeCameraBehaviour : MonoBehaviour {
    //Maximum speed when holdin gshift
    public float CamSens = 0.25f;

    Vector3 _last_mouse = new Vector3(
                                    x : 255,
                                    y : 255,
                                    z : 255);

    public float MainSpeed = 100.0f;

    //multiplied by how long shift is held.  Basically running
    public float MaxShift = 1000.0f;

    public bool MovementStaysFlat = true;

    //How sensitive it with mouse
    public bool RotateOnlyIfMousedown = true;

    //regular speed
    public float ShiftAdd = 250.0f;

    //kind of in the middle of the screen, rather than at the top (play)
    float _total_run = 1.0f;

    void Awake() {
      Debug.Log(message : "FlyCamera Awake() - RESETTING CAMERA POSITION"); // nop?
      // nop:
      //transform.position.Set(0,8,-32);
      //transform.rotation.Set(15,0,0,1);
      this.transform.position = new Vector3(
                                            x : 0,
                                            y : 8,
                                            z : -32);
      this.transform.rotation = Quaternion.Euler(
                                                 x : 25,
                                                 y : 0,
                                                 z : 0);
    }

    void Update() {
      if (Input.GetMouseButtonDown(button : 1))
        this._last_mouse = Input.mousePosition; // $CTK reset when we begin

      if (!this.RotateOnlyIfMousedown || this.RotateOnlyIfMousedown && Input.GetMouseButton(button : 1)) {
        this._last_mouse = Input.mousePosition - this._last_mouse;
        this._last_mouse = new Vector3(
                                     x : -this._last_mouse.y * this.CamSens,
                                     y : this._last_mouse.x * this.CamSens,
                                     z : 0);
        this._last_mouse = new Vector3(
                                     x : this.transform.eulerAngles.x + this._last_mouse.x,
                                     y : this.transform.eulerAngles.y + this._last_mouse.y,
                                     z : 0);
        this.transform.eulerAngles = this._last_mouse;
        this._last_mouse = Input.mousePosition;
        //Mouse  camera angle done.  
      }

      //Keyboard commands
      var p = this.GetBaseInput();
      if (Input.GetKey(key : KeyCode.LeftShift)) {
        this._total_run += Time.deltaTime;
        p = p * this._total_run * this.ShiftAdd;
        p.x = Mathf.Clamp(
                          value : p.x,
                          min : -this.MaxShift,
                          max : this.MaxShift);
        p.y = Mathf.Clamp(
                          value : p.y,
                          min : -this.MaxShift,
                          max : this.MaxShift);
        p.z = Mathf.Clamp(
                          value : p.z,
                          min : -this.MaxShift,
                          max : this.MaxShift);
      } else {
        this._total_run = Mathf.Clamp(
                                    value : this._total_run * 0.5f,
                                    min : 1f,
                                    max : 1000f);
        p = p * this.MainSpeed;
      }

      p = p * Time.deltaTime;
      var new_position = this.transform.position;
      if (Input.GetKey(key : KeyCode.Space)
          || this.MovementStaysFlat && !(this.RotateOnlyIfMousedown && Input.GetMouseButton(button : 1))) {
        //If player wants to move on X and Z axis only
        this.transform.Translate(translation : p);
        new_position.x = this.transform.position.x;
        new_position.z = this.transform.position.z;
        this.transform.position = new_position;
      } else {
        this.transform.Translate(translation : p);
      }
    }

    Vector3 GetBaseInput() {
      //returns the basic values, if it's 0 than it's not active.
      var p_velocity = new Vector3();
      if (Input.GetKey(key : KeyCode.W))
        p_velocity += new Vector3(
                                  x : 0,
                                  y : 0,
                                  z : 1);
      if (Input.GetKey(key : KeyCode.S))
        p_velocity += new Vector3(
                                  x : 0,
                                  y : 0,
                                  z : -1);
      if (Input.GetKey(key : KeyCode.A))
        p_velocity += new Vector3(
                                  x : -1,
                                  y : 0,
                                  z : 0);
      if (Input.GetKey(key : KeyCode.D))
        p_velocity += new Vector3(
                                  x : 1,
                                  y : 0,
                                  z : 0);
      return p_velocity;
    }
  }
}
