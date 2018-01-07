using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawBoundingBoxOnCamera : MonoBehaviour {
    private List<Color> colors;
    public Color lColor = Color.green;
    public Material lineMaterial;
    private List<Vector3[,]> outlines;
    private List<Vector3[,]> triangles;

    private void Awake() {
      outlines = new List<Vector3[,]>();
      colors = new List<Color>();
      triangles = new List<Vector3[,]>();
    }

    private void Start() { }

    private void OnPostRender() {
      if (outlines == null)
        return;
      lineMaterial.SetPass(0);
      GL.Begin(GL.LINES);
      for (var j = 0; j < outlines.Count; j++) {
        GL.Color(colors[j]);
        for (var i = 0; i < outlines[j].GetLength(0); i++) {
          GL.Vertex(
                    outlines[j][i,
                                0]);
          GL.Vertex(
                    outlines[j][i,
                                1]);
        }
      }

      GL.End();

      GL.Begin(GL.TRIANGLES);

      for (var j = 0; j < triangles.Count; j++) {
        GL.Color(colors[j]);
        for (var i = 0; i < triangles[j].GetLength(0); i++) {
          GL.Vertex(
                    triangles[j][i,
                                 0]);
          GL.Vertex(
                    triangles[j][i,
                                 1]);
          GL.Vertex(
                    triangles[j][i,
                                 2]);
        }
      }

      GL.End();
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor) {
      if (newOutlines == null)
        return;
      if (outlines == null)
        return;
      if (newOutlines.GetLength(0) > 0) {
        outlines.Add(newOutlines);
        colors.Add(newcolor);
      }
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor, Vector3[,] newTriangles) {
      if (newOutlines == null)
        return;
      if (outlines == null)
        return;
      if (newOutlines.GetLength(0) > 0) {
        outlines.Add(newOutlines);
        colors.Add(newcolor);
        triangles.Add(newTriangles);
      }
    }

    private void Update() {
      outlines = new List<Vector3[,]>();
      colors = new List<Color>();
      triangles = new List<Vector3[,]>();
    }
  }
}
