using UnityEngine;

namespace SceneSpecificAssets.Grasping.Utilities.DataCollection {
  [ExecuteInEditMode]
  public class BlitToMaterial : MonoBehaviour {
    public Material _material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
      Graphics.Blit(
                    source,
                    destination,
                    _material);
    }
  }
}
