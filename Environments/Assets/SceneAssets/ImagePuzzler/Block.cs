using System.Collections;
using UnityEngine;

namespace SceneAssets.ImagePuzzler {
  public class Block : MonoBehaviour {
    [SerializeField]
    Vector2Int _coord;
    [SerializeField]
    Vector2Int _starting_coord;

    public event System.Action<Block> OnBlockPressed;
    public event System.Action OnFinishedMoving;

    public Vector2Int Coord { get { return this._coord; } set { this._coord = value; } }



    public void Init (Vector2Int starting_coord, Texture2D image) {
      this._starting_coord = starting_coord;
      this._coord = starting_coord;

      this.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Unlit/Texture");
      this.GetComponent<MeshRenderer> ().material.mainTexture = image;
    }

    void OnMouseDown () {
      if (this.OnBlockPressed != null)
        this.OnBlockPressed (this);
    }

    public void MoveToPosition (Vector2 target, float duration) {
      this.StartCoroutine (this.AnimateMove (target, duration));
    }

    IEnumerator AnimateMove (Vector2 target, float duration) {
      Vector2 initial_pos = this.transform.position;
      float percent = 0;

      while (percent < 1) {
        percent += Time.deltaTime / duration;
        this.transform.position = Vector2.Lerp (initial_pos, target, percent);
        yield return null;
      }

      if (this.OnFinishedMoving != null) {
        this.OnFinishedMoving ();
      }
    }

    public bool IsAtStartingCoord () {
      return this.Coord == this._starting_coord;
    }
  }
}
