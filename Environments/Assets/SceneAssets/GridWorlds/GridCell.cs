using UnityEngine;
using Object = System.Object;

namespace SceneAssets.GridWorlds {
  
  public abstract class GridCell: MonoBehaviour {
    public IntVector3 GridCoordinates { get; set; }

    protected Collider _col;
    protected Renderer _rend;

    public abstract void Setup (string name, Material mat);
  }

  public class EmptyCell : GridCell {


    public override void Setup (string name, Material mat) {
      _rend = this.GetComponent<Renderer> ();
      _col = this.GetComponent<Collider> ();
      this.name = name;
      _col.isTrigger = true;
      _rend.enabled = false;


      //Destroy (this.GetComponent<Renderer> ());
      //this.GetComponent<Renderer>().material = mat;
    }

    public void SetAsGoal (string name, Material mat) {
      this.name = name;
      _rend.enabled = true;
      _rend.material = mat;
      this.tag = "Goal";
    }
  }

  public class FilledCell : GridCell {
    public override void Setup (string name, Material mat) {
      this.name = name;
      this.GetComponent<Collider> ().isTrigger = false;
      this.GetComponent<Renderer> ().material = mat;
      this.tag = "Obstruction";
    }
  }

  public class GoalCell : EmptyCell {
    public override void Setup (string name, Material mat) {
      this.name = name;
      this.GetComponent<Collider> ().isTrigger = true;
      this.GetComponent<Renderer> ().material = mat;
      this.tag = "Goal";
    }
  }
}
