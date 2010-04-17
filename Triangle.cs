using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _3DCube
{
	public struct Triangle {
		
		public int VertexIndex1;
		public int VertexIndex2;
		public int VertexIndex3;
		public Color Color;

		public Triangle(int index1, int index2, int index3, Color inputColor)
		{
			VertexIndex1 = index1;
			VertexIndex2 = index2;
			VertexIndex3 = index3;
			Color = inputColor;
		}

		
		public Vector3 SurfaceNormal {
			get {
				// Calculates the Surface Normal of the triangle using the 3 Vector3's associated with it.
				//		From: http://en.wikipedia.org/wiki/Surface_normal
				//            http://www.opengl.org/wiki/Calculating_a_Surface_Normal
				Vector3 vectorU = new Vector3(
					(Render.Cube.Mesh.Vertex[VertexIndex2].X - Render.Cube.Mesh.Vertex[VertexIndex1].X),
					(Render.Cube.Mesh.Vertex[VertexIndex2].Y - Render.Cube.Mesh.Vertex[VertexIndex1].Y),
					(Render.Cube.Mesh.Vertex[VertexIndex2].Z - Render.Cube.Mesh.Vertex[VertexIndex1].Z)
				);
				Vector3 vectorV = new Vector3(
					(Render.Cube.Mesh.Vertex[VertexIndex3].X - Render.Cube.Mesh.Vertex[VertexIndex1].X),
					(Render.Cube.Mesh.Vertex[VertexIndex3].Y - Render.Cube.Mesh.Vertex[VertexIndex1].Y),
					(Render.Cube.Mesh.Vertex[VertexIndex3].Z - Render.Cube.Mesh.Vertex[VertexIndex1].Z)
				);
				return (new Vector3(
					((vectorU.Y * vectorV.Z) - (vectorU.Z * vectorV.Y)),
					((vectorU.Z * vectorV.X) - (vectorU.X * vectorV.Z)),
					((vectorU.X * vectorV.Y) - (vectorU.Y * vectorV.X))
				));
			}
		}
		
		public Vector3 Centroid {
			get {
				// Center of the triangle using the Centroid technique.
				//		From: http://en.wikipedia.org/wiki/Centroid#Of_triangle_and_tetrahedron
				double CentroidX, CentroidY, CentroidZ;
				CentroidX = (Render.Cube.Mesh.Vertex[VertexIndex1].X + Render.Cube.Mesh.Vertex[VertexIndex2].X + Render.Cube.Mesh.Vertex[VertexIndex3].X) / 3;
				CentroidY = (Render.Cube.Mesh.Vertex[VertexIndex1].Y + Render.Cube.Mesh.Vertex[VertexIndex2].Y + Render.Cube.Mesh.Vertex[VertexIndex3].Y) / 3;
				CentroidZ = (Render.Cube.Mesh.Vertex[VertexIndex1].Z + Render.Cube.Mesh.Vertex[VertexIndex2].Z + Render.Cube.Mesh.Vertex[VertexIndex3].Z) / 3;
				return (new Vector3(CentroidX, CentroidY, CentroidZ));
			}
		}

		public double DistanceToCameraSquared {
			get {
				// Distance to Camera squared (distance formula without sqrt) is faster and provides the same
				// functionality. Sqrt can be added to another function if needed.
				return (
					Math.Pow((Camera.X - this.Centroid.X), 2) + 
					Math.Pow((Camera.Y - this.Centroid.Y), 2) + 
					Math.Pow((Camera.Z - this.Centroid.Z), 2)
				);
			}
		}

		public double[] VertexDistanceToCameraSquared {
			get {
				// Closest Distance to Camera squared (distance formula without sqrt) is faster and provides the same
				// functionality. Sqrt can be added to another function if needed. It returns the distance from the
				// closest vertex to the camera.
				double[] vertexDistance = new double[3];				
				vertexDistance[0] = Math.Pow((Camera.X - Render.Cube.Mesh.Vertex[0].X), 2) +
					Math.Pow((Camera.Y - Render.Cube.Mesh.Vertex[0].Y), 2) +
					Math.Pow((Camera.Z - Render.Cube.Mesh.Vertex[0].Z), 2);
				vertexDistance[1] = Math.Pow((Camera.X - Render.Cube.Mesh.Vertex[1].X), 2) +
					Math.Pow((Camera.Y - Render.Cube.Mesh.Vertex[1].Y), 2) +
					Math.Pow((Camera.Z - Render.Cube.Mesh.Vertex[1].Z), 2);
				vertexDistance[2] = Math.Pow((Camera.X - Render.Cube.Mesh.Vertex[2].X), 2) +
					Math.Pow((Camera.Y - Render.Cube.Mesh.Vertex[2].Y), 2) +
					Math.Pow((Camera.Z - Render.Cube.Mesh.Vertex[2].Z), 2);
				Array.Sort(vertexDistance);
				return vertexDistance;
			}
		}		


		override public string ToString() {
			// Returns a string representing the Triangle.
			//		Format: V1(V1x, V1y, V1z) -> V2(V2x, V2y, V2z) -> V3(V3x, V3y, V3z) | Centroid=(X,Y,Z) | Distance=#
			return
				VertexIndex1 + "(" + Render.Cube.Mesh.Vertex[VertexIndex1].X + "," + Render.Cube.Mesh.Vertex[VertexIndex1].Y + "," + Render.Cube.Mesh.Vertex[VertexIndex1].Z + ") -> " +
				VertexIndex2 + "(" + Render.Cube.Mesh.Vertex[VertexIndex2].X + "," + Render.Cube.Mesh.Vertex[VertexIndex2].Y + "," + Render.Cube.Mesh.Vertex[VertexIndex2].Z + ") -> " +
				VertexIndex3 + "(" + Render.Cube.Mesh.Vertex[VertexIndex3].X + "," + Render.Cube.Mesh.Vertex[VertexIndex3].Y + "," + Render.Cube.Mesh.Vertex[VertexIndex3].Z + ") | Centroid=(" + 
				this.Centroid.X + "," + this.Centroid.Y + "," + this.Centroid.Y + ") | Distance=" + Math.Round(this.DistanceToCameraSquared, 5);
		}
		
	}
}
