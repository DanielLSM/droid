using System;
using UnityEngine;

public class Block : MonoBehaviour {
  [SerializeField]
  Vector2Int _coord;

  public event Action<Block> OnBlockPressed;

  public Vector2Int Coord { get { return this._coord; } set { this._coord = value; } }

  public void Init(Vector2Int starting_coord, Texture2D image) {
    this._coord = starting_coord;

    this.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
    this.GetComponent<MeshRenderer>().material.mainTexture = image;
  }

  void OnMouseDown() {
    if (this.OnBlockPressed != null) this.OnBlockPressed(this);
  }
}
