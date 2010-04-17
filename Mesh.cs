using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _3DCube
{
	public class Mesh
	{
		public Vector3[] Vertex;
		public Triangle[] Triangle;
		
		public Mesh() {
			setCubeVerticies();
			generateCubeTriangles();
		}
		
		public void setCubeVerticies() {
			// Set the verticies for a cube object
			Vertex = new Vector3[8];
			Vertex[0] = new Vector3(-0.5, 0.5, 0.5);
			Vertex[1] = new Vector3(0.5, 0.5, 0.5);
			Vertex[2] = new Vector3(-0.5, 0.5, -0.5);
			Vertex[3] = new Vector3(0.5, 0.5, -0.5);
			Vertex[4] = new Vector3(-0.5, -0.5, 0.5);
			Vertex[5] = new Vector3(0.5, -0.5, 0.5);
			Vertex[6] = new Vector3(-0.5, -0.5, -0.5);
			Vertex[7] = new Vector3(0.5, -0.5, -0.5);
		}
		
		public void generateCubeTriangles() {
			// Setup the verticies relationship with their triangles.
			Triangle = new Triangle[12];
			Triangle[0] = new Triangle(0, 4, 1, Color.FromArgb(255, 0, 0));		// Face 0,4,5,1
			Triangle[1] = new Triangle(1, 4, 5, Color.FromArgb(255, 0, 0));		// Face 0,4,5,1
			Triangle[2] = new Triangle(1, 5, 3, Color.FromArgb(0, 0, 255));		// Face 1,5,7,3
			Triangle[3] = new Triangle(3, 5, 7, Color.FromArgb(0, 0, 255));		// Face 1,5,7,3
			Triangle[4] = new Triangle(3, 7, 2, Color.FromArgb(255, 0, 0));		// Face 3,7,6,2
			Triangle[5] = new Triangle(2, 7, 6, Color.FromArgb(255, 0, 0));		// Face 3,7,6,2
			Triangle[6] = new Triangle(2, 6, 0, Color.FromArgb(0, 0, 255));		// Face 2,6,4,0
			Triangle[7] = new Triangle(0, 6, 4, Color.FromArgb(0, 0, 255));		// Face 2,6,4,0
			Triangle[8] = new Triangle(0, 1, 2, Color.FromArgb(255, 255, 0));	// Face 0,1,3,2
			Triangle[9] = new Triangle(2, 1, 3, Color.FromArgb(255, 255, 0));	// Face 0,1,3,2
			Triangle[10] = new Triangle(5, 4, 7, Color.FromArgb(255, 255, 0));	// Face 5,4,6,7
			Triangle[11] = new Triangle(7, 4, 6, Color.FromArgb(255, 255, 0));	// Face 5,4,6,7
		}

		public string viewVertex3DCoords(int vertexIndex) {
			return "(" + Math.Round(Vertex[vertexIndex].X, 2) + "," + Math.Round(Vertex[vertexIndex].Y, 2) + "," + Math.Round(Vertex[vertexIndex].Z, 2) + ")";
		}
		public string viewVertex2DCoords(int vertexIndex) {
			int[] tempInt = new int[2];
			tempInt = Render.convert3Dto2D(Vertex[vertexIndex].X, Vertex[vertexIndex].Y, Vertex[vertexIndex].Z);
			return "(" + tempInt[0] + "," + tempInt[1] + ")";
		}
		public string viewVertexDistanceFromCameraSquared(int vertexIndex) {
			return (Math.Round(Math.Pow((Camera.X - Vertex[vertexIndex].X), 2) + Math.Pow((Camera.Y - Vertex[vertexIndex].Y), 2) + Math.Pow((Camera.Z - Vertex[vertexIndex].Z), 2), 5)).ToString();
		}
	}
}
