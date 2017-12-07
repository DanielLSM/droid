using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Neodroid.Utilities.BoundingBoxes;

namespace Neodroid.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(BoundingBox))]
  public class BoundingBoxObserver : Observer {

    BoundingBox _bounding_box;

    protected override void Start () {
      AddToEnvironment ();
      _bounding_box = this.GetComponent<BoundingBox> ();
    }

    public override byte[] GetData () {
      _data = Encoding.ASCII.GetBytes (_bounding_box.BoundingBoxCoordinatesAsString);
      return _data;
    }

    public override string GetObserverIdentifier () {
      return name + "BoundingBox";
    }
  }
}

