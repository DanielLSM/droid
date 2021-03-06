﻿using System.IO;
using System.Text;
using SceneAssets.ScripterGrasper.Grasps;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace SceneAssets.ScripterGrasper.Utilities.DataCollection {
  public class DataCollector : MonoBehaviour {
    // Sampling rate
    readonly string _file_path = @"training_data/";
    readonly string _file_path_gripper = @"gripper_position_rotation.csv";
    readonly string _file_path_target = @"target_position_rotation.csv";
    [SerializeField] Camera[] _cameras;
    [SerializeField] int _current_episode_progress;

    [SerializeField] bool _delete_file_content;

    [SerializeField] int _episode_length = 100;
    [SerializeField] ScriptedGripper _gripper;

    int _i;
    [SerializeField] Grasp _target;

    void Start() {
      if (!this._gripper) this._gripper = FindObjectOfType<ScriptedGripper>();
      if (!this._target) this._target = FindObjectOfType<Grasp>();

      //print ("GPU supports depth format: " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth));
      //print ("GPU supports shadowmap format: " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Shadowmap));

      File.WriteAllText(this._file_path + this._file_path_gripper, "frame, x, y, z, rot_x, rot_y, rot_z\n");
      File.WriteAllText(this._file_path + this._file_path_target, "frame, x, y, z, rot_x, rot_y, rot_z\n");

      /*if (!File.Exists(_file_path + _file_path_pos_rot)) {
      print("Created file/path: " + _file_path + _file_path_pos_rot);
      //File.Create(_file_path + _file_path_pos_rot);
    }
    if (_deleteFileContent) {
      File.WriteAllText(_file_path + _file_path_pos_rot, "");
      _deleteFileContent = false;
    }*/
    }

    void LateUpdate() {
      if (this._current_episode_progress == this._episode_length - 1) {
        //Vector3 screenPoint = _depth_camera.WorldToViewportPoint (_target.transform.position);
        //if (screenPoint.z > 0 && screenPoint.x > 0.1 && screenPoint.x < 0.9 && screenPoint.y > 0.1 && screenPoint.y < 0.9) {
        var gripper_position_relative_to_camera =
            this.transform.InverseTransformPoint(this._gripper.transform.position);
        var gripper_direction_relative_to_camera =
            this.transform.InverseTransformDirection(this._gripper.transform.eulerAngles);
        var gripper_transform_output = this.GetTransformOutput(
            this._i,
            gripper_position_relative_to_camera,
            gripper_direction_relative_to_camera);
        this.SaveToCSV(this._file_path + this._file_path_gripper, gripper_transform_output);

        var target_position_relative_to_camera =
            this.transform.InverseTransformPoint(this._target.transform.position);
        var target_direction_relative_to_camera =
            this.transform.InverseTransformDirection(this._target.transform.eulerAngles);
        var target_transform_output = this.GetTransformOutput(
            this._i,
            target_position_relative_to_camera,
            target_direction_relative_to_camera);
        this.SaveToCSV(this._file_path + this._file_path_target, target_transform_output);

        foreach (var input_camera in this._cameras)
          this.SaveRenderTextureToImage(this._i, input_camera, input_camera.name + "/");
        this._i++;
        //}
        this._current_episode_progress = 0;
      }

      this._current_episode_progress++;
    }

    string[] GetTransformOutput(int id, Vector3 pos, Vector3 dir) {
      return new[] {
          id.ToString(),
          pos.x.ToString("0.000000"),
          pos.y.ToString("0.000000"),
          pos.z.ToString("0.000000"),
          dir.x.ToString("0.000000"),
          dir.y.ToString("0.000000"),
          dir.z.ToString("0.000000")
      };
    }

    void SaveBytesToFile(byte[] bytes, string filename) { File.WriteAllBytes(filename, bytes); }

    //Writes to file in the format: pos x, pos y, pos z, rot x, rot y, rot z
    void SaveToCSV(string filePath, string[] output) {
      var delimiter = ", ";

      var sb = new StringBuilder();

      sb.AppendLine(string.Join(delimiter, output));

      File.AppendAllText(filePath, sb.ToString());

      //using (StreamWriter sw = File.AppendText(filePath)) {
      //  sw.WriteLine(sb.ToString());
      //}
    }

    public void SaveRenderTextureToImage(int id, Camera input_camera, string file_name_dd) {
      var texture2d = RenderTextureImage(input_camera);
      var data = texture2d.EncodeToPNG();
      var file_name = this._file_path + file_name_dd + id;
      //SaveTextureAsArray (camera, texture2d, file_name+".ssv");
      this.SaveBytesToFile(data, file_name + ".png");
      //return data;
    }

    public static Texture2D RenderTextureImage(Camera input_camera) {
      // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      var current_render_texture = RenderTexture.active;
      RenderTexture.active = input_camera.targetTexture;
      input_camera.Render();
      var image = new Texture2D(input_camera.targetTexture.width, input_camera.targetTexture.height);
      image.ReadPixels(
          new Rect(0, 0, input_camera.targetTexture.width, input_camera.targetTexture.height),
          0,
          0);
      image.Apply();
      RenderTexture.active = current_render_texture;
      return image;
    }

    void SaveTextureAsArray(Camera input_camera, Texture2D image, string path) {
      var str_array = new string[image.height];
      var colors = image.GetPixels32();
      for (var iss = 0; iss < image.height; iss++) {
        var str = "";
        for (var jss = 0; jss < image.width; jss++) {
          str = str
                + (input_camera.nearClipPlane + colors[iss * image.width + jss].r * input_camera.farClipPlane)
                + " ";
        }

        str_array[iss] = str;
      }

      File.WriteAllLines(path, str_array);
    }
  }
}
