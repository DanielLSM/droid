﻿using System;
using System.Collections.Generic;
using SceneAssets.ScripterGrasper.Navigation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SceneSpecificAssets.Grasping.Navigation {
  [Serializable]
  class BezierCurvePath {
    readonly BezierCurve _bezier_curve;
    readonly List<Vector3> _path_list;
    float _progress;

    public Vector3 StartPosition;
    public Vector3 TargetPosition;

    public BezierCurvePath(
        Vector3 start_position,
        Vector3 target_position,
        BezierCurve game_object,
        List<Vector3> path_list) {
      this.StartPosition = start_position;
      this.TargetPosition = target_position;
      this._path_list = path_list;
      this._bezier_curve = game_object;
      this.CurvifyPath();
    }

    void CurvifyPath() {
      for (var i = 0; i < this._bezier_curve.PointCount; i++)
        Object.Destroy(this._bezier_curve[i].gameObject);
      this._bezier_curve.ClearPoints();
      foreach (var t in this._path_list) this._bezier_curve.AddPointAt(t);

      this.SetHandlePosition(this._bezier_curve);
    }

    void SetHandlePosition(BezierCurve bc) {
      for (var i = 0; i < bc.PointCount; i++) {
        bc[i].Style = BezierPoint.HandleStyle.Broken;

        if (i != 0 && i + 1 != bc.PointCount) {
          var curr_point = bc[i].Position;
          var prev_point = bc[i - 1].Position;
          var next_point = bc[i + 1].Position;
          var direction_forward = (next_point - prev_point).normalized;
          var direction_back = (prev_point - next_point).normalized;
          var handle_scalar = 0.33f;
          var distance_previous = Vector3.Distance(prev_point, curr_point);
          var distance_next = Vector3.Distance(curr_point, next_point);

          bc[i].GlobalHandle1 += direction_back.normalized * distance_previous * handle_scalar;
          bc[i].GlobalHandle2 += direction_forward.normalized * distance_next * handle_scalar;

          //if (Debugging) Debug.DrawLine(bc[i].globalHandle1, bc[i].globalHandle2, Color.blue, 5);
        }
      }
    }

    public Vector3 Next(float step_size) {
      this._progress += step_size;
      return this._bezier_curve.GetPointAt(this._progress);
    }
  }
}
