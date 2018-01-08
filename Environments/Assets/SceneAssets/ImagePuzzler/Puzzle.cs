using UnityEngine;

public class Puzzle : MonoBehaviour {
  [SerializeField] Block _empty_block;
  [SerializeField] int _horisontal_divisions = 6;

  [SerializeField] Texture2D _image;
  [SerializeField] int _vertical_divisions = 6;

  void Start () {
    this.CreatePuzzle ();
  }

  void CreatePuzzle () {
    var image_slices = new Texture2D[this._horisontal_divisions, this._vertical_divisions];
    if (this._image) {
      image_slices = ImageSlicer.GetSlices (this._image, this._horisontal_divisions, this._vertical_divisions);
    }
    var dominant_division = Mathf.Max (this._vertical_divisions, this._horisontal_divisions);
    //var lesser_division = Mathf.Min (this._vertical_divisions, this._horisontal_divisions);

    for (var y = 0; y < this._vertical_divisions; y++) {
      for (var x = 0; x < this._horisontal_divisions; x++) {
        var block_object = GameObject.CreatePrimitive (PrimitiveType.Quad);
        block_object.transform.position = -Vector2.one * (dominant_division - 1) * .5f + new Vector2 (x, y);
        block_object.transform.parent = this.transform;

        var block = block_object.AddComponent<Block> ();
        block.OnBlockPressed += this.PlayerMoveBlockInput;
        block.Init (new Vector2Int (x, y), image_slices [x, y]);

        if (y == 0 && x == this._horisontal_divisions - 1) {
          block_object.SetActive (false);
          this._empty_block = block;
        }
      }
    }

    Camera.main.orthographicSize = dominant_division * .55f;
  }

  void PlayerMoveBlockInput (Block block_to_move) {
    if ((block_to_move.Coord - this._empty_block.Coord).sqrMagnitude == 1) {
      var target_coord = this._empty_block.Coord;
      this._empty_block.Coord = block_to_move.Coord;
      block_to_move.Coord = target_coord;

      Vector2 target_position = this._empty_block.transform.position;
      this._empty_block.transform.position = block_to_move.transform.position;
      block_to_move.transform.position = target_position;
    }
  }
}
