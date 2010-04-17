using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace _3DCube
{
	public static class Rasterizer {
		public static Boolean PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c) {
			// Determines if a point is inside of a triangle in the 2D Coordinate plane.
			// From:
			//		http://www.blackpawn.com/texts/pointinpoly/default.html
			if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b)) {
				return true;
			}
			return false;
		}

		private static Boolean SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b) {
			// Related to PointInTriangle() function.
			// From:
			//		http://www.blackpawn.com/texts/pointinpoly/default.html
			Vector3 CrossProduct1 = Vector3.CrossProduct((b - a), (p1 - a));
			Vector3 CrossProduct2 = Vector3.CrossProduct((b - a), (p2 - a));
			if (Vector3.DotProduct(CrossProduct1, CrossProduct2) >= 0) {
				return true;
			}
			return false;
		}
		
		public static Bitmap Rasterize(Bitmap graphicsObject) {
			// This function is _REALLY_ fucking slow. chaos95 says its closer to raycasting than Scanline fill.
			Vector3 vertex1, vertex2, vertex3;
			int[] tempCoords = new int[2];
			for (int y = 0; y < graphicsObject.Height; y++) {
				for (int x = 0; x < graphicsObject.Width; x++) {
					foreach (Triangle tempTriangle in Render.Cube.Mesh.Triangle) {
						tempCoords = Render.convert3Dto2D(Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex1].X, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex1].Y, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex1].Z);
						vertex1 = new Vector3(tempCoords[0], tempCoords[1], 0);
						tempCoords = Render.convert3Dto2D(Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex2].X, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex2].Y, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex2].Z);
						vertex2 = new Vector3(tempCoords[0], tempCoords[1], 0);
						tempCoords = Render.convert3Dto2D(Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex3].X, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex3].Y, Render.Cube.Mesh.Vertex[tempTriangle.VertexIndex3].Z);
						vertex3 = new Vector3(tempCoords[0], tempCoords[1], 0);
						if (PointInTriangle((new Vector3(x, y, 0)), vertex1, vertex2, vertex3)) {
							graphicsObject.SetPixel(x, y, tempTriangle.Color);	
						}
					}
				}
				// So I can keep track of y/500 lines are done.
				Console.WriteLine(y);
			}
			return graphicsObject;
		}
		
	}
}
