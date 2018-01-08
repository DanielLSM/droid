﻿using UnityEngine;

namespace SceneAssets.ScripterGrasper.Utilities.DataCollection {
  [ExecuteInEditMode]
  public class BlitToMaterial : MonoBehaviour {
    [SerializeField]  Material _material;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
      Graphics.Blit(
                    source : source,
                    dest : destination,
                    mat : this._material);
    }
  }
}
