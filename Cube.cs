using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MathNet.Numerics.LinearAlgebra;

namespace _3DCube
{
	public class Cube {
		public Mesh Mesh = new Mesh();
		public void rotateCubeX(double degrees) {
			// Change the Pitch (X axis) of all Verticies in mesh by degrees.
			for (int i = 0; i < Render.Cube.Mesh.Vertex.Length; i++) {
				Render.Cube.Mesh.Vertex[i].Pitch(degrees);
			}
		}
		public void rotateCubeY(double degrees) {
			// Change the Yaw (Y axis) of all Verticies in mesh by degrees.
			for (int i = 0; i < 8; i++) {
				Render.Cube.Mesh.Vertex[i].Yaw(degrees);
			}
		}
		public void rotateCubeZ(double degrees) {
			// Change the Roll (Z axis) of all Verticies in mesh by degrees.
			for (int i = 0; i < 8; i++) {
				Render.Cube.Mesh.Vertex[i].Roll(degrees);
			}
		}
	}
}
