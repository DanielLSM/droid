using Neodroid.Managers.General;
using Neodroid.Models.Observers.General;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Models.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class CameraObserver : Observer,
                                IHasByteArray {
    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    [Header("Specific", order = 102)]
    [SerializeField]
    Camera _camera;
    NeodroidManager _manager;
    bool _grab = true;
    [SerializeField] Texture2D _texture;

    public override string ObserverIdentifier { get { return this.name + "Camera"; } }

    public byte[] Bytes { get { return this._bytes; } private set { this._bytes = value; } }

    protected override void Awake() {
      base.Awake();
      this._manager = FindObjectOfType<NeodroidManager>();
      this._camera = this.GetComponent<Camera>();
      this._texture = new Texture2D(this._camera.targetTexture.width, this._camera.targetTexture.height);
    }

    protected virtual void OnPostRender() { this.UpdateBytes(); }

    protected virtual void UpdateBytes() {
      if (!this._grab)
        return;
      this._grab = false;

      var current_render_texture = RenderTexture.active;
      RenderTexture.active = this._camera.targetTexture;

      this._texture.ReadPixels(
          new Rect(0, 0, this._camera.targetTexture.width, this._camera.targetTexture.height),
          0,
          0);
      this._texture.Apply();
      this._bytes = this._texture.EncodeToPNG();

      RenderTexture.active = current_render_texture;
    }

    public override void UpdateObservation() {
      if (this._manager.Configuration.SimulationType != SimulationType.FrameDependent) {
        print("WARNING! Camera Observations may be out of sync other data");
      }
      this._grab = true;
    }
  }
}
