using System.IO;
using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities.DataCollection.NotUsed {
  public class ImageRecorder : MonoBehaviour {
    [SerializeField]
    Camera _camera;
    private readonly string _file_path = @"training_data/shadow/";

    private int _i;

    private void Start() {
      if (!_camera)
        _camera = GetComponent<Camera>();
    }

    private void Update() {
      SaveRenderTextureToImage(
                               _i,
                               _camera,
                               _file_path);

      _i++;
    }

    public void SaveRenderTextureToImage(int id, Camera cam, string file_name_dd) {
      var texture2d = RenderTextureImage(cam);
      var data = texture2d.EncodeToPNG();
      var file_name = file_name_dd + id + ".png";
      File.WriteAllBytes(
                         file_name,
                         data);
    }

    public static Texture2D RenderTextureImage(Camera camera) {
      // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      var current_render_texture = RenderTexture.active;
      RenderTexture.active = camera.targetTexture;
      camera.Render();
      var image = new Texture2D(
                                camera.targetTexture.width,
                                camera.targetTexture.height);
      image.ReadPixels(
                       new Rect(
                                0,
                                0,
                                camera.targetTexture.width,
                                camera.targetTexture.height),
                       0,
                       0);
      image.Apply();
      RenderTexture.active = current_render_texture;
      return image;
    }
  }
}
