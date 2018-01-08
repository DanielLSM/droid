using System.Collections.Generic;
using SceneSpecificAssets.Grasping.Utilities;
using UnityEngine;

namespace SceneAssets.ScripterGrasper.Scripts {
  //[ExecuteInEditMode]
  public class ObstacleSpawner : MonoBehaviour {
    [SerializeField] GameObject _cube;
    [SerializeField]  Material _material_cube;
    [SerializeField]  Material _material_sphere;

    [SerializeField] GameObject[] _obstacles;
    [SerializeField] GameObject _sphere;
    //[SerializeField]  float _cube_size = 0.2f;

    [Range (
      min : 1,
      max : 20)]
    [SerializeField]  int _number_of_cubes = 1;

    [Range (
      min : 1,
      max : 20)]
    [SerializeField]  int _number_of_spheres = 1;

    [Header (header : "Spawn random number of objects?")]
    [SerializeField]  bool _random_obj_num;

    [Space]
    [Header (header : "Random scaling of objects (0 = uniform scale)")]
    [Range (
      min : 0.000f,
      max : 0.300f)]
    [SerializeField]  float _scaling_factor;

    [Header (header : "Cube")] [SerializeField]  bool _spawn_cubes = true;

    [Header (header : "Sphere")] [SerializeField]  bool _spawn_spheres = true;

    [SerializeField]  float _sphere_size = 0.2f;

    [Header (header : "Show obstacle spawn box?")]
    [SerializeField]  bool _visualize_grid = true;

    [Space]
    [Header (header : "Bounderies")]
    [Range (
      min : 0.10f,
      max : 5.00f)]
    [SerializeField]  float _x_size = 1.4f;

    [Range (
      min : 0.00f,
      max : 3.00f)]
    [SerializeField] float _y_center_point = 1.4f;

    [Range (
      min : 0.10f,
      max : 5.00f)]
    [SerializeField]  float _y_size = 1.2f;

    [Range (
      min : 0.10f,
      max : 5.00f)]
    [SerializeField]  float _z_size = 1.4f;

    void Awake () {
      this.Setup ();
    }

    void TearDown () {
      if (this._cube)
        DestroyImmediate (obj : this._cube);
      if (this._sphere)
        DestroyImmediate (obj : this._sphere);
      if (this._obstacles != null && this._obstacles.Length > 0)
        this.RemoveObstacles ();
    }

    public void Setup () {
      this.TearDown ();
      this._y_center_point = this.transform.position.y;
      this._obstacles = new GameObject[this._number_of_cubes + this._number_of_spheres];
      this._cube = GameObject.CreatePrimitive (type : PrimitiveType.Cube);
      this._cube.SetActive (value : false);
      //_cube.AddComponent<Obstruction>();
      this._cube.GetComponent<MeshRenderer> ().material = this._material_cube;
      this._sphere = GameObject.CreatePrimitive (type : PrimitiveType.Sphere);
      this._sphere.SetActive (value : false);
      //_sphere.AddComponent<Obstruction>();
      this._sphere.GetComponent<MeshRenderer> ().material = this._material_sphere;
      if (this._scaling_factor > 0.3f || this._scaling_factor < -0.3f)
        this._scaling_factor = 0.3f;

      if (!this._spawn_cubes && !this._spawn_spheres)
        this._spawn_cubes = true;

      if (this._random_obj_num)
        this.SpawnObstacles (
          cube_num : Random.Range (
            min : 1,
            max : this._number_of_cubes),
          sphere_num : Random.Range (
            min : 1,
            max : this._number_of_spheres));
      else
        this.SpawnObstacles (
          cube_num : this._number_of_cubes,
          sphere_num : this._number_of_spheres);
    }

    void Update () {
      if (this._visualize_grid)
        GraspingUtilities.DrawRect (
          x_size : this._x_size,
          y_size : this._y_size,
          z_size : this._z_size,
          pos : this.transform.position,
          color : Color.red);
    }

    public void SpawnObstacles (float cube_num = 1, float sphere_num = 1) {
      this.RemoveObstacles ();
      var temp_list = new List<GameObject> ();
      if (this._spawn_cubes)
        for (var i = 0; i < cube_num; i++) {
          var temp = Random.Range (
                       min : -this._scaling_factor,
                       max : this._scaling_factor);
          //spawn_pos = new Vector3(Random.Range(x_min, x_max), Random.Range(y_min, y_max), Random.Range(z_min, z_max));
          var spawn_pos = new Vector3 (
                            x : Random.Range (
                              min : -this._x_size / 2,
                              max : this._x_size / 2),
                            y : Random.Range (
                              min : -this._y_size / 2 + this._y_center_point,
                              max : this._y_size / 2 + this._y_center_point),
                            z : Random.Range (
                              min : -this._z_size / 2,
                              max : this._z_size / 2));
          var cube_clone = Instantiate (
                             original : this._cube,
                             position : spawn_pos,
                             rotation : Quaternion.identity,
                             parent : this.transform);
          cube_clone.transform.localScale = new Vector3 (
            x : this._sphere_size + temp,
            y : this._sphere_size + temp,
            z : this._sphere_size + temp);
          cube_clone.SetActive (value : true);
          cube_clone.tag = "Obstruction";

          temp_list.Add (item : cube_clone);
          /*if (Vector3.Distance(cube_clone.transform.position, GameObject.Find("EscapePos").transform.position) < 0.5f) {
                    Destroy(cube_clone);
                  }*/
        }

      if (this._spawn_spheres) {
        Vector3 spawn_pos;
        for (var i = 0; i < sphere_num; i++) {
          var temp = Random.Range (
                       min : -this._scaling_factor,
                       max : this._scaling_factor);
          //spawn_pos = new Vector3(Random.Range(x_min, x_max), Random.Range(y_min, y_max), Random.Range(z_min, z_max));
          spawn_pos = new Vector3 (
            x : Random.Range (
              min : -this._x_size / 2,
              max : this._x_size / 2),
            y : Random.Range (
              min : -this._y_size / 2 + this._y_center_point,
              max : this._y_size / 2 + this._y_center_point),
            z : Random.Range (
              min : -this._z_size / 2,
              max : this._z_size / 2));
          var sphere_clone = Instantiate (
                               original : this._sphere,
                               position : spawn_pos,
                               rotation : Quaternion.identity,
                               parent : this.transform);
          sphere_clone.transform.localScale = new Vector3 (
            x : this._sphere_size + temp,
            y : this._sphere_size + temp,
            z : this._sphere_size + temp);
          sphere_clone.SetActive (value : true);
          sphere_clone.tag = "Obstruction";

          temp_list.Add (item : sphere_clone);
          /*if (Vector3.Distance(sphere_clone.transform.position, GameObject.Find("EscapePos").transform.position) < 0.2f) {
                    Destroy(sphere_clone);
                  }*/
        }
      }

      temp_list.CopyTo (array : this._obstacles);
    }

    void RemoveObstacles () {
      foreach (var obstacle in this._obstacles)
        DestroyImmediate (obj : obstacle);
    }

    void OnDestroy () {
      this.TearDown ();
    }
  }
}
