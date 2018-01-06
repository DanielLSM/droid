using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using Neodroid.Utilities.BoundingBoxes;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {
    //BoundingBox _bounding_box;

    protected override void Start () {
      //_bounding_box = this.GetComponent<BoundingBox> ();
    }

    public override void UpdateData () {
      //Data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsJSON);
    }

    public override string ObserverIdentifier { get { return name + "BoundingBox"; } }
  }
}

