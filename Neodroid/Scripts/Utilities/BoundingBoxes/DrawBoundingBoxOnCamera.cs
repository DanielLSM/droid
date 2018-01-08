using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [RequireComponent( typeof(Camera))]
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
      this.lineMaterial.SetPass(pass : 0);
      GL.Begin(mode : GL.LINES);
      for (var j = 0; j < this.outlines.Count; j++) {
        GL.Color(c : this.colors[index : j]);
        for (var i = 0; i < this.outlines[index : j].GetLength(dimension : 0); i++) {
          GL.Vertex(
                    v : this.outlines[index : j][i,
                                                 0]);
          GL.Vertex(
                    v : this.outlines[index : j][i,
                                                 1]);
        }
      }

      GL.End();

      GL.Begin(mode : GL.TRIANGLES);

      for (var j = 0; j < this.triangles.Count; j++) {
        GL.Color(c : this.colors[index : j]);
        for (var i = 0; i < this.triangles[index : j].GetLength(dimension : 0); i++) {
          GL.Vertex(
                    v : this.triangles[index : j][i,
                                                  0]);
          GL.Vertex(
                    v : this.triangles[index : j][i,
                                                  1]);
          GL.Vertex(
                    v : this.triangles[index : j][i,
                                                  2]);
        }
      }

      GL.End();
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor) {
      if (newOutlines == null)
        return;
      if (this.outlines == null)
        return;
      if (newOutlines.GetLength(dimension : 0) > 0) {
        this.outlines.Add(item : newOutlines);
        this.colors.Add(item : newcolor);
      }
    }

    public void setOutlines(Vector3[,] newOutlines, Color newcolor, Vector3[,] newTriangles) {
      if (newOutlines == null)
        return;
      if (this.outlines == null)
        return;
      if (newOutlines.GetLength(dimension : 0) > 0) {
        this.outlines.Add(item : newOutlines);
        this.colors.Add(item : newcolor);
        this.triangles.Add(item : newTriangles);
      }
    }

    void Update() {
      this.outlines = new List<Vector3[,]>();
      this.colors = new List<Color>();
      this.triangles = new List<Vector3[,]>();
    }
  }
}
