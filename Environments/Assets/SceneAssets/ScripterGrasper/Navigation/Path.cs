
using System.Collections.Generic;
using Assets.SceneAssets.ScripterGrasper.Navigation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SceneSpecificAssets.Grasping.Navigation {
  [System.Serializable]
  internal class BezierCurvePath {
    private readonly BezierCurve _bezier_curve;
    private readonly List<Vector3> _path_list;
    private float _progress;

    public Vector3 StartPosition;
    public Vector3 TargetPosition;

    public BezierCurvePath(
      Vector3 start_position,
      Vector3 target_position,
      BezierCurve game_object,
      List<Vector3> path_list) {
      StartPosition = start_position;
      TargetPosition = target_position;
      _path_list = path_list;
      _bezier_curve = game_object;
      CurvifyPath();
    }

    private void CurvifyPath() {
      for (var i = 0; i < _bezier_curve.PointCount; i++) Object.Destroy(_bezier_curve[i].gameObject);
      _bezier_curve.ClearPoints();
      foreach (var t in _path_list)
        _bezier_curve.AddPointAt(t);

      SetHandlePosition(_bezier_curve);
    }

    private void SetHandlePosition(BezierCurve bc) {
      for (var i = 0; i < bc.PointCount; i++) {
        bc[i].handleStyle = BezierPoint.HandleStyle.Broken;

        if (i != 0 && i + 1 != bc.PointCount) {
          var curr_point = bc[i].position;
          var prev_point = bc[i - 1].position;
          var next_point = bc[i + 1].position;
          var direction_forward = (next_point - prev_point).normalized;
          var direction_back = (prev_point - next_point).normalized;
          var handle_scalar = 0.33f;
          var distance_previous = Vector3.Distance(
                                                   prev_point,
                                                   curr_point);
          var distance_next = Vector3.Distance(
                                               curr_point,
                                               next_point);

          bc[i].globalHandle1 += direction_back.normalized * distance_previous * handle_scalar;
          bc[i].globalHandle2 += direction_forward.normalized * distance_next * handle_scalar;

          //if (Debugging) Debug.DrawLine(bc[i].globalHandle1, bc[i].globalHandle2, Color.blue, 5);
        }
      }
    }

    public Vector3 Next(float step_size) {
      _progress += step_size;
      return _bezier_curve.GetPointAt(_progress);
    }
  }
}
