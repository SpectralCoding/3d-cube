using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace _3DCube
{
	public static class Render {
		
		public static Light Light = new Light();
		public static Boolean drawWireframe = false;
		public static Boolean drawFaces = true;
		public static Boolean drawCorners = false;
		public static Boolean drawLight = true;
		public static Boolean drawSurfaceNormals = false;
		public static int drawShadowMode = 0;
		public static Cube Cube = new Cube();


		public static int[] convert3Dto2D(double PointX, double PointY, double PointZ) {
			// From http://en.wikipedia.org/wiki/3D_projection#Perspective_projection :
			//
			// Point(X|Y|X)			a{x,y,z}	The point in 3D space that is to be projected.
			// Cube.Camera(X|Y|X)	c{x,y,z}	The location of the camera.
			// Cube.Theta(X|Y|X)	θ{x,y,z}	The rotation of the camera. When c{x,y,z}=<0,0,0>, and 0{x,y,z}=<0,0,0>, the 3D vector <1,2,0> is projected to the 2D vector <1,2>.
			// Cube.Viewer(X|Y|X)	e{x,y,z}	The viewer's position relative to the display surface.
			// Bsub(X|Y)			b{x,y}		The 2D projection of a.
			//
			// "First, we define a point DsubXYZ as a translation of point a{x,y,z} into a coordinate system defined by
			// c{x,y,z}. This is achieved by subtracting c{x,y,z} from a{x,y,z} and then applying a vector rotation matrix
			// using -θ{x,y,z} to the result. This transformation is often called a camera transform (note that these
			// calculations assume a left-handed system of axes). This transformed point can then be projected onto the 2D
			// plane using the formula below for B{x,y}."
			double BsubX, BsubY;
			int[] returnVals = new int[2];
			double[] point = Camera.cameraTransform(PointX, PointY, PointZ);
			BsubX = (point[0] - Viewer.X) / (Viewer.Z / point[2]);
			BsubY = (point[1] - Viewer.Y) / (Viewer.Z / point[2]);
			returnVals[0] = (int)((BsubX * 50) + 250);
			returnVals[1] = (int)((BsubY * 50) + 250);
			return returnVals;
		}



		public static Bitmap drawObject() {
			// This is currently in the process of being rewritten with the Rasterizer instead of using
			// Windows GDI stuff.
			Bitmap bmpReturn = new Bitmap(500, 500);
			Pen myPen = new Pen(System.Drawing.Color.Black, 1);
			double[] tempDistanceArray = new double[3];
			double[] distanceArray = new double[Render.Cube.Mesh.Triangle.Length];
			int[] triangleArray = new int[Render.Cube.Mesh.Triangle.Length];
			int[][] points = new int[3][];
			// Create the Graphics Object that will be used to contain the data for the Cube.
			System.Drawing.Graphics graphicsObj = Graphics.FromImage(bmpReturn);
			
			points[0] = new int[2]; points[1] = new int[2]; points[2] = new int[2];
			
			// Enter the distance for each triangle into the distanceArray and its corresponding
			// Triangle Index into the triangleArray. Sort the triangleArray by the distanceArray.
			for (int i = 0; i < Render.Cube.Mesh.Triangle.Length; i++) {
				tempDistanceArray = Render.Cube.Mesh.Triangle[i].VertexDistanceToCameraSquared;
				distanceArray[i] = tempDistanceArray[0] + tempDistanceArray[1] + tempDistanceArray[2];
				triangleArray[i] = i;
			}
			Array.Sort(distanceArray, triangleArray);

			
			// This is for drawing the actual colored faces on the sides of the cube.
			if (Render.drawFaces == true) {
				for (int i = 0; i < Render.Cube.Mesh.Triangle.Length; i++) {
					// Convert the each point on the polygon from 3D to 2D.
					points[0] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].Z);
					points[1] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].Z);
					points[2] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].Z);
					Point[] pointArr = new Point[3];
					pointArr[0] = new Point(points[0][0], points[0][1]);
					pointArr[1] = new Point(points[1][0], points[1][1]);
					pointArr[2] = new Point(points[2][0], points[2][1]);
					// Draw the filled Polygon on the graphicsObj using the three points which describe its verticies.
					graphicsObj.FillPolygon(
						new SolidBrush(Render.Cube.Mesh.Triangle[triangleArray[i]].Color),
						pointArr
					);
				}
			}
			// This is for drawing the black lines aroun the cube (wireframe)
			if (Render.drawWireframe == true) {
				for (int i = 0; i < Render.Cube.Mesh.Triangle.Length; i++) {
					// Convert the each point on the polygon from 3D to 2D.
					points[0] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex1].Z);
					points[1] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex2].Z);
					points[2] = convert3Dto2D(Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].X, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].Y, Render.Cube.Mesh.Vertex[Render.Cube.Mesh.Triangle[triangleArray[i]].VertexIndex3].Z);
					Point[] pointArr = new Point[3];
					pointArr[0] = new Point(points[0][0], points[0][1]);
					pointArr[1] = new Point(points[1][0], points[1][1]);
					pointArr[2] = new Point(points[2][0], points[2][1]);
					// Draw the Polygon outline on the graphicsObj using the three points which describe its verticies.
					graphicsObj.DrawPolygon(
						new Pen(new SolidBrush(Color.Black), 1),
						pointArr
					);
				}
			}
			/*
			// This is for drawing the black lines that represent the Surface Normals for each polygon
			if (Render.drawSurfaceNormals == true) {
				for (int i = 0; i < 6; i++) {
					int[] startPoint2D = new int[2];
					double[] endPoint3D = new double[3];
					int[] endPoint2D = new int[2];
					// Draw Surface Normal for first polygon on Face
					startPoint2D = convert3Dto2D(face[i].surfaceCenterPoint[0][0], face[i].surfaceCenterPoint[0][1], face[i].surfaceCenterPoint[0][2]);
					endPoint3D[0] = face[i].surfaceCenterPoint[0][0] + (face[i].surfaceNormal[0].X / 5);
					endPoint3D[1] = face[i].surfaceCenterPoint[0][1] + (face[i].surfaceNormal[0].Y / 5);
					endPoint3D[2] = face[i].surfaceCenterPoint[0][2] + (face[i].surfaceNormal[0].Z / 5);
					endPoint2D = convert3Dto2D(endPoint3D[0], endPoint3D[1], endPoint3D[2]);
					graphicsObj.DrawLine(myPen, startPoint2D[0], startPoint2D[1], endPoint2D[0], endPoint2D[1]);
					// Draw Surface Normal for second polygon on Face
					startPoint2D = convert3Dto2D(face[i].surfaceCenterPoint[1][0], face[i].surfaceCenterPoint[1][1], face[i].surfaceCenterPoint[1][2]);
					endPoint3D[0] = face[i].surfaceCenterPoint[1][0] + (face[i].surfaceNormal[0].X / 5);
					endPoint3D[1] = face[i].surfaceCenterPoint[1][1] + (face[i].surfaceNormal[0].Y / 5);
					endPoint3D[2] = face[i].surfaceCenterPoint[1][2] + (face[i].surfaceNormal[0].Z / 5);
					endPoint2D = convert3Dto2D(endPoint3D[0], endPoint3D[1], endPoint3D[2]);
					graphicsObj.DrawLine(myPen, startPoint2D[0], startPoint2D[1], endPoint2D[0], endPoint2D[1]);
				}
			}
			
			*/
			// This draws the X's at each vertex.
			if (Render.drawCorners == true) {
				for (int i = 0; i < Render.Cube.Mesh.Vertex.Length; i++) {
					int[] vertPoint = new int[2];
					// Convert the vertex point from 3D to 2D space.
					vertPoint = convert3Dto2D(Render.Cube.Mesh.Vertex[i].X, Render.Cube.Mesh.Vertex[i].Y, Render.Cube.Mesh.Vertex[i].Z);
					try {
						// Draw an X on each 2D vertex point.
						bmpReturn.SetPixel(vertPoint[0] - 1, vertPoint[1] + 1, Color.Black); bmpReturn.SetPixel(vertPoint[0] - 2, vertPoint[1] + 2, Color.Black);
						bmpReturn.SetPixel(vertPoint[0] - 1, vertPoint[1] - 1, Color.Black); bmpReturn.SetPixel(vertPoint[0] - 2, vertPoint[1] - 2, Color.Black);
						bmpReturn.SetPixel(vertPoint[0], vertPoint[1], Color.Black);
						bmpReturn.SetPixel(vertPoint[0] + 1, vertPoint[1] + 1, Color.Black); bmpReturn.SetPixel(vertPoint[0] + 2, vertPoint[1] + 2, Color.Black);
						bmpReturn.SetPixel(vertPoint[0] + 1, vertPoint[1] - 1, Color.Black); bmpReturn.SetPixel(vertPoint[0] + 2, vertPoint[1] - 2, Color.Black);
					} catch (Exception) { }
				}
			}
			
			/*
			// Draw Light
			if (Render.drawLight == true) {
				int[] tempXY = new int[2];
				tempXY = convert3Dto2D(LightX, LightY, LightZ);
				//Console.WriteLine("(" + LightX + "," + LightY + "," + LightZ + ") -> (" + tempXY[0] + "," + tempXY[1] + ")");
				try
				{
					graphicsObj.DrawLine(myPen, tempXY[0] - 5, tempXY[1] - 5, tempXY[0] + 5, tempXY[1] + 5);
					graphicsObj.DrawLine(myPen, tempXY[0] + 5, tempXY[1] - 5, tempXY[0] - 5, tempXY[1] + 5);
					graphicsObj.DrawLine(myPen, tempXY[0], tempXY[1] - 7, tempXY[0], tempXY[1] + 7);
					graphicsObj.DrawLine(myPen, tempXY[0] - 7, tempXY[1], tempXY[0] + 7, tempXY[1]);
				}
				catch (Exception) { }
			}


			*/
			// Moves the graphics object to the bmpReturn Bitmap for display on the form.
			graphicsObj.DrawImage(Rasterizer.Rasterize(bmpReturn), new Point(0, 0));
			//graphicsObj.DrawImage(bmpReturn, new Point(0,0));
			return bmpReturn;
		}

	}
}
