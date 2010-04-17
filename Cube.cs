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
	static class Cube
	{
		public static Boolean drawWireframe = false;
		public static Boolean drawFaces = true;
		public static Boolean drawCorners = false;
		public static Boolean drawLight = true;
		public static Boolean drawSurfaceNormals = false;
		public static int drawShadowMode = 0;
		public static double CameraX = 0;
		public static double CameraY = 0;
		public static double CameraZ = 10;
		public static double ViewerX = 0;
		public static double ViewerY = 0;
		public static double ViewerZ = 1.83;
		public static double ThetaX = 0;
		public static double ThetaY = 0;
		public static double ThetaZ = 0;
		public static double LightX = 4;
		public static double LightY = 4;
		public static double LightZ = 9;

		public static Vertex[] vertex = new Vertex[8];
		
		public static Face[] face = new Face[6];
		
		public static void loadVerticies() {		
			for (int i = 0; i < 8; i++) {
				vertex[i] = new Vertex();
			}
			vertex[0].setCoords(-0.5, 0.5, 0.5);
			vertex[1].setCoords(0.5, 0.5, 0.5);
			vertex[2].setCoords(-0.5, 0.5, -0.5);
			vertex[3].setCoords(0.5, 0.5, -0.5);
			vertex[4].setCoords(-0.5, -0.5, 0.5);
			vertex[5].setCoords(0.5, -0.5, 0.5);
			vertex[6].setCoords(-0.5, -0.5, -0.5);
			vertex[7].setCoords(0.5, -0.5, -0.5);
		}
		
		public static void loadFaces() {
			for (int i = 0; i < 6; i++) {
				face[i] = new Face();
			}
			face[0].setColor(Color.FromArgb(255, 0, 0));
			face[0].setVerticies(0, 1, 5, 4);
			face[2].setColor(Color.FromArgb(255, 0, 0));
			face[2].setVerticies(3, 2, 6, 7);
			
			face[1].setColor(Color.FromArgb(0, 0, 255));
			face[1].setVerticies(1, 3, 7, 5);
			face[3].setColor(Color.FromArgb(0, 0, 255));
			face[3].setVerticies(2, 6, 4, 0);
			
			face[4].setColor(Color.FromArgb(255, 255, 0));
			face[4].setVerticies(0, 2, 3, 1);
			face[5].setColor(Color.FromArgb(255, 255, 0));
			face[5].setVerticies(4, 6, 7, 5);
		}
		
		public static void printCoords() {
			for (int i = 0; i < 8; i++) {
				Console.WriteLine("Vertex" + i + " (3D): X=" + vertex[i].X3 + " Y=" + vertex[i].Y3 + " Z=" + vertex[i].Z3);
				Console.WriteLine("Vertex" + i + " (2D): X=" + vertex[i].X2 + " Y=" + vertex[i].Y2);
			}
		}
		
		public static void calc2DCoords() {
			for (int i = 0; i < 8; i++) {
				vertex[i].calc2DCoords();
			}
		}
		
		public static Bitmap drawCube() {
			Bitmap bmpReturn = new Bitmap(500, 500);	
			int tempX, tempY;
			double[] distanceMatrix = new double[6];
			int[] faceMatrix = new int[6];
			Pen myPen = new Pen(System.Drawing.Color.Black, 1);
			// Create the Graphics Object that will be used to contain the data for the Cube.
			System.Drawing.Graphics graphicsObj = Graphics.FromImage(bmpReturn);
			
			// This is for drawing the actual colored faces on the sides of the cube.
			if (drawFaces == true) {
				for (int i = 0; i < 6; i++) {
					distanceMatrix[i] = face[i].polygonAvgDistance();
					faceMatrix[i] = i;
				}
				Array.Sort(distanceMatrix, faceMatrix);
				for (int i = 0; i < 6; i++) {
					// One line of code for each polygon that creates the face.
					graphicsObj.FillPolygon(face[faceMatrix[i]].polygonBrush[0], face[faceMatrix[i]].polygonPoints[0]);
					graphicsObj.FillPolygon(face[faceMatrix[i]].polygonBrush[1], face[faceMatrix[i]].polygonPoints[1]);
				}
			}

			// This is for drawing the black lines that represent the Surface Normals for each polygon
			if (drawSurfaceNormals == true) {
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

			// This is for drawing the black lines aroun the cube (wireframe)
			if (drawWireframe == true)
			{
				for (int i = 0; i < 6; i++)
				{
					graphicsObj.DrawPolygon(new Pen(new SolidBrush(Color.Black), 1), face[i].polygonPoints[0]);
					graphicsObj.DrawPolygon(new Pen(new SolidBrush(Color.Black), 1), face[i].polygonPoints[1]);
				}
			}

			
			// This draws the X's at each vertex.
			if (drawCorners == true) {
				for (int i = 0; i < 8; i++) {
					tempX = vertex[i].X2; tempY = vertex[i].Y2;
					try {
						bmpReturn.SetPixel(tempX - 1, tempY + 1, Color.Black); bmpReturn.SetPixel(tempX - 2, tempY + 2, Color.Black);
						bmpReturn.SetPixel(tempX - 1, tempY - 1, Color.Black); bmpReturn.SetPixel(tempX - 2, tempY - 2, Color.Black);
						bmpReturn.SetPixel(tempX, tempY, Color.Black);
						bmpReturn.SetPixel(tempX + 1, tempY + 1, Color.Black); bmpReturn.SetPixel(tempX + 2, tempY + 2, Color.Black);
						bmpReturn.SetPixel(tempX + 1, tempY - 1, Color.Black); bmpReturn.SetPixel(tempX + 2, tempY - 2, Color.Black);
					} catch (Exception) { }
				}
			}
			
			// Draw Light
			if (drawLight == true) {
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


			
			// Moves the graphics object to the bmpReturn Bitmap for display on the form.
			graphicsObj.DrawImage(bmpReturn, new Point(0,0));			
			return bmpReturn;
		}

		public static void rotateCubeX(double degrees) {
			for (int i = 0; i < 8; i++) {
				vertex[i].rotateVertexX(degrees);
			}
		}
		public static void rotateCubeY(double degrees) {
			for (int i = 0; i < 8; i++) {
				vertex[i].rotateVertexY(degrees);
			}
		}
		public static void rotateCubeZ(double degrees) {
			for (int i = 0; i < 8; i++) {
				vertex[i].rotateVertexZ(degrees);
			}
		}
		public static void calcFaces() {
			for (int i = 0; i < 6; i++) {
				//Console.WriteLine("Face " + i + ":");
				face[i].setPolygons();
			}
		}

		private static int[] convert3Dto2D(double PointX, double PointY, double PointZ) {
			// From http://en.wikipedia.org/wiki/3D_projection#Perspective_projection :
			//
			// Point(X|Y|X)			a{x,y,z}	The point in 3D space that is to be projected.
			// Cube.Camera(X|Y|X)	c{x,y,z}	The location of the camera.
			// Cube.Theta(X|Y|X)	0{x,y,z}	The rotation of the camera. When c{x,y,z}=<0,0,0>, and 0{x,y,z}=<0,0,0>, the 3D vector <1,2,0> is projected to the 2D vector <1,2>.
			// Cube.Viewer(X|Y|X)	e{x,y,z}	The viewer's position relative to the display surface.
			// Bsub(X|Y)			b{x,y}		The 2D projection of a.
			//

			double BsubX, BsubY;
			MathNet.Numerics.LinearAlgebra.Matrix convMat1, convMat2, convMat3, convMat4, convMat41, convMat42, DsubXYZ;
			double CosThetaX = Math.Cos(Cube.ThetaX); double SinThetaX = Math.Sin(Cube.ThetaX);
			double CosThetaY = Math.Cos(Cube.ThetaY); double SinThetaY = Math.Sin(Cube.ThetaY);
			double CosThetaZ = Math.Cos(Cube.ThetaZ); double SinThetaZ = Math.Sin(Cube.ThetaZ);

			convMat1 = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][] {
				new double[] {	1,	0,			0					},
				new double[] {	0,	CosThetaX,	((-1)*(SinThetaX))	},
				new double[] {	0,	SinThetaX,	CosThetaX			}
			});
			convMat2 = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][] {
				new double[] {	CosThetaY,				0,	SinThetaY	},
				new double[] {	0,						1,	0			},
				new double[] {	((-1)*(SinThetaY)),		0,	CosThetaY	}
			});
			convMat3 = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][] {
				new double[] {	CosThetaZ,	((-1)*(SinThetaZ)),		0	},
				new double[] {	SinThetaZ,	CosThetaZ,				0	},
				new double[] {	0,			0,						1	}
			});
			convMat41 = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][] {
				new double[] {	PointX	},
				new double[] {	PointY	},
				new double[] {	PointZ	}
			});
			convMat42 = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][] {
				new double[] {	Cube.CameraX	},
				new double[] {	Cube.CameraY	},
				new double[] {	Cube.CameraZ	}
			});
			convMat4 = convMat41.Clone();
			convMat4.Subtract(convMat42);
			DsubXYZ = ((convMat1.Multiply(convMat2)).Multiply(convMat3)).Multiply(convMat4);
			BsubX = (DsubXYZ[0, 0] - Cube.ViewerX) / (Cube.ViewerZ / DsubXYZ[2, 0]);
			BsubY = (DsubXYZ[1, 0] - Cube.ViewerY) / (Cube.ViewerZ / DsubXYZ[2, 0]);
			int[] returnValues = new int[2];
			returnValues[0] = (int)((BsubX * 50) + 250);
			returnValues[1] = (int)((BsubY * 50) + 250);
			return returnValues;
		}
		
	}
}
