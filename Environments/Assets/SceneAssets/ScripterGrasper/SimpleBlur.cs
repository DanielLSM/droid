using UnityEngine;

namespace SceneSpecificAssets.Grasping {
  [ExecuteInEditMode]
  public class SimpleBlur : MonoBehaviour {
    public Material background;

    private void OnRenderImage(RenderTexture src, RenderTexture dst) {
      Graphics.Blit(
                    src,
                    dst,
                    background);
    }
  }
}
