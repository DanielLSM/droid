using Neodroid.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [RequireComponent(typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    public override string ObserverIdentifier { get { return name + "BoundingBox"; } }
    //BoundingBox _bounding_box;

    protected override void Start() {
      //_bounding_box = this.GetComponent<BoundingBox> ();
    }

    public override void UpdateData() {
      //Data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsJSON);
    }
  }
}
