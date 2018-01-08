using UnityEngine;

namespace Neodroid.Scripts.Utilities.Testing {
  public class DrawSpaces : MonoBehaviour {
    void OnDrawGizmos() {
      var color = Color.green;
      // local up
      this.DrawHelperAtCenter(
                              direction : this.transform.up,
                              color : color,
                              scale : 2f);

      color.g -= 0.5f;
      // global up
      this.DrawHelperAtCenter(
                              direction : Vector3.up,
                              color : color,
                              scale : 1f);

      color = Color.blue;
      // local forward
      this.DrawHelperAtCenter(
                              direction : this.transform.forward,
                              color : color,
                              scale : 2f);

      color.b -= 0.5f;
      // global forward
      this.DrawHelperAtCenter(
                              direction : Vector3.forward,
                              color : color,
                              scale : 1f);

      color = Color.red;
      // local right
      this.DrawHelperAtCenter(
                              direction : this.transform.right,
                              color : color,
                              scale : 2f);

      color.r -= 0.5f;
      // global right
      this.DrawHelperAtCenter(
                              direction : Vector3.right,
                              color : color,
                              scale : 1f);
    }

    void DrawHelperAtCenter(
      Vector3 direction,
      Color color,
      float scale) {
      Gizmos.color = color;
      var destination = this.transform.position + direction * scale;
      Gizmos.DrawLine(
                      @from : this.transform.position,
                      to : destination);
    }
  }
}
