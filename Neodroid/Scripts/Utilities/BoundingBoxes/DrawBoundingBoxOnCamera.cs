using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class DrawBoundingBoxOnCamera : MonoBehaviour {
    List<Color> colors;
    public Color lColor = Color.green;
    public Material lineMaterial;
    List<Vector3[,]> outlines;
    List<Vector3[,]> triangles;

    void Awake() {
      this.outlines = new List<Vector3[,]>();
      this.colors = new List<Color>();
      this.triangles = new List<Vector3[,]>();
    }

    void Start() { }

    void OnPostRender() {
      if (this.outlines == null)
        return;
      this.lineMaterial.SetPass(0);
      GL.Begin(GL.LINES);
      for (var j = 0; j < this.outlines.Count; j++) {
        GL.Color(this.colors[j]);
        for (var i = 0; i < this.outlines[j].GetLength(0); i++) {
          GL.Vertex(this.outlines[j][i, 0]);
          GL.Vertex(this.outlines[j][i, 1]);
        }
      }

      GL.End();

      GL.Begin(GL.TRIANGLES);

      for (var j = 0; j < this.triangles.Count; j++) {
        GL.Color(this.colors[j]);
        for (var i = 0; i < this.triangles[j].GetLength(0); i++) {
          GL.Vertex(this.triangles[j][i, 0]);
          GL.Vertex(this.triangles[j][i, 1]);
          GL.Vertex(this.triangles[j][i, 2]);
        }
      }

      GL.End();
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor) {
      if (newOutlines == null)
        return;
      if (this.outlines == null)
        return;
      if (newOutlines.GetLength(0) > 0) {
        this.outlines.Add(newOutlines);
        this.colors.Add(newcolor);
      }
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor, Vector3[,] newTriangles) {
      if (newOutlines == null)
        return;
      if (this.outlines == null)
        return;
      if (newOutlines.GetLength(0) > 0) {
        this.outlines.Add(newOutlines);
        this.colors.Add(newcolor);
        this.triangles.Add(newTriangles);
      }
    }

    void Update() {
      this.outlines = new List<Vector3[,]>();
      this.colors = new List<Color>();
      this.triangles = new List<Vector3[,]>();
    }
  }
}
