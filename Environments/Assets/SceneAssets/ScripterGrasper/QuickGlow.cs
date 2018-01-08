using UnityEngine;

namespace SceneAssets.ScripterGrasper {
  [ExecuteInEditMode]
  public class QuickGlow : MonoBehaviour {
    [SerializeField]  Material _add_material;
    [SerializeField]  Material _blur_material;

    [Range(
      min : 0,
      max : 4)]
    public int DownRes;

    [Range(
      min : 0,
      max : 3)]
    public float Intensity;

    [Range(
      min : 0,
      max : 10)]
    public int Iterations;

    [Range(
      min : 0,
      max : 10)]
    public float Size;

    void OnValidate() {
      if (this._blur_material != null)
        this._blur_material.SetFloat(
                                   name : "_Size",
                                   value : this.Size);
      if (this._add_material != null)
        this._add_material.SetFloat(
                                  name : "_Intensity",
                                  value : this.Intensity);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
      var composite = RenderTexture.GetTemporary(
                                                 width : src.width,
                                                 height : src.height);
      Graphics.Blit(
                    source : src,
                    dest : composite);

      var width = src.width >> this.DownRes;
      var height = src.height >> this.DownRes;

      var rt = RenderTexture.GetTemporary(
                                          width : width,
                                          height : height);
      Graphics.Blit(
                    source : src,
                    dest : rt);

      for (var i = 0; i < this.Iterations; i++) {
        var rt2 = RenderTexture.GetTemporary(
                                             width : width,
                                             height : height);
        Graphics.Blit(
                      source : rt,
                      dest : rt2,
                      mat : this._blur_material);
        RenderTexture.ReleaseTemporary(temp : rt);
        rt = rt2;
      }

      this._add_material.SetTexture(
                                  name : "_BlendTex",
                                  value : rt);
      Graphics.Blit(
                    source : composite,
                    dest : dst,
                    mat : this._add_material);

      RenderTexture.ReleaseTemporary(temp : rt);
      RenderTexture.ReleaseTemporary(temp : composite);
    }
  }
}
