using UnityEngine;

namespace Neodroid.Utilities {
  public class DrawSpaces : MonoBehaviour {
    private void OnDrawGizmos() {
    var color = Color.green;
      // local up
      DrawHelperAtCenter(
                         transform.up,
                         color,
                         2f);

      color.g -= 0.5f;
      // global up
      DrawHelperAtCenter(
                         Vector3.up,
                         color,
                         1f);

      color = Color.blue;
      // local forward
      DrawHelperAtCenter(
                         transform.forward,
                         color,
                         2f);

      color.b -= 0.5f;
      // global forward
      DrawHelperAtCenter(
                         Vector3.forward,
                         color,
                         1f);

      color = Color.red;
      // local right
      DrawHelperAtCenter(
                         transform.right,
                         color,
                         2f);

      color.r -= 0.5f;
      // global right
      DrawHelperAtCenter(
                         Vector3.right,
                         color,
                         1f);
    }

    private void DrawHelperAtCenter(
      Vector3 direction,
      Color color,
      float scale) {
      Gizmos.color = color;
      var destination = transform.position + direction * scale;
      Gizmos.DrawLine(
                      transform.position,
                      destination);
    }
  }
}
