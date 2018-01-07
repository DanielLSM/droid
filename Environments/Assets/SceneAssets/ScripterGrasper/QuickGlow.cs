using UnityEngine;

namespace SceneSpecificAssets.Grasping {
  [ExecuteInEditMode]
  public class QuickGlow : MonoBehaviour {
    public Material AddMaterial;
    public Material BlurMaterial;

    [Range(
      0,
      4)]
    public int DownRes;

    [Range(
      0,
      3)]
    public float Intensity;

    [Range(
      0,
      10)]
    public int Iterations;

    [Range(
      0,
      10)]
    public float Size;

    private void OnValidate() {
      if (BlurMaterial != null)
        BlurMaterial.SetFloat(
                              "_Size",
                              Size);
      if (AddMaterial != null)
        AddMaterial.SetFloat(
                             "_Intensity",
                             Intensity);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst) {
      var composite = RenderTexture.GetTemporary(
                                                 src.width,
                                                 src.height);
      Graphics.Blit(
                    src,
                    composite);

      var width = src.width >> DownRes;
      var height = src.height >> DownRes;

      var rt = RenderTexture.GetTemporary(
                                          width,
                                          height);
      Graphics.Blit(
                    src,
                    rt);

      for (var i = 0; i < Iterations; i++) {
        var rt2 = RenderTexture.GetTemporary(
                                             width,
                                             height);
        Graphics.Blit(
                      rt,
                      rt2,
                      BlurMaterial);
        RenderTexture.ReleaseTemporary(rt);
        rt = rt2;
      }

      AddMaterial.SetTexture(
                             "_BlendTex",
                             rt);
      Graphics.Blit(
                    composite,
                    dst,
                    AddMaterial);

      RenderTexture.ReleaseTemporary(rt);
      RenderTexture.ReleaseTemporary(composite);
    }
  }
}
