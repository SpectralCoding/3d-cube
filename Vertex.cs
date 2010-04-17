using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace _3DCube
{
	class Vertex
	{
		
		public double X3, Y3, Z3;
		public int X2, Y2;
		public double distanceFromViewer;
		
		public Vertex() {
			X3 = 0; Y3 = 0; Z3 = 0;
		}

		public void setCoords(double inX, double inY, double inZ) { X3 = inX; Y3 = inY; Z3 = inZ; }
		public string view3DCoords() { return "(" + Math.Round(X3, 2) + "," + Math.Round(Y3, 2) + "," + Math.Round(Z3,2) + ")"; }
		public string view2DCoords() { return "(" + X2 + "," + Y2 + ")"; }
		public string viewDistanceFromViewer() { return Math.Round(distanceFromViewer, 5).ToString(); }
		
		public void calc2DCoords() {
			// From http://en.wikipedia.org/wiki/3D_projection#Perspective_projection :
			//
			// Point(X|Y|X)			a{x,y,z}	The point in 3D space that is to be projected.
			// Cube.Camera(X|Y|X)	c{x,y,z}	The location of the camera.
			// Cube.Theta(X|Y|X)	0{x,y,z}	The rotation of the camera. When c{x,y,z}=<0,0,0>, and 0{x,y,z}=<0,0,0>, the 3D vector <1,2,0> is projected to the 2D vector <1,2>.
			// Cube.Viewer(X|Y|X)	e{x,y,z}	The viewer's position relative to the display surface.
			// Bsub(X|Y)			b{x,y}		The 2D projection of a.
			//
			
			double BsubX, BsubY;
			Matrix convMat1, convMat2, convMat3, convMat4, convMat41, convMat42, DsubXYZ;
			double CosThetaX = Math.Cos(Cube.ThetaX); double SinThetaX = Math.Sin(Cube.ThetaX);
			double CosThetaY = Math.Cos(Cube.ThetaY); double SinThetaY = Math.Sin(Cube.ThetaY);
			double CosThetaZ = Math.Cos(Cube.ThetaZ); double SinThetaZ = Math.Sin(Cube.ThetaZ);
			
			convMat1 = new Matrix(new double[][] {
				new double[] {	1,	0,			0					},
				new double[] {	0,	CosThetaX,	((-1)*(SinThetaX))	},
				new double[] {	0,	SinThetaX,	CosThetaX			}
			});
			convMat2 = new Matrix(new double[][] {
				new double[] {	CosThetaY,				0,	SinThetaY	},
				new double[] {	0,						1,	0			},
				new double[] {	((-1)*(SinThetaY)),		0,	CosThetaY	}
			});
			convMat3 = new Matrix(new double[][] {
				new double[] {	CosThetaZ,	((-1)*(SinThetaZ)),		0	},
				new double[] {	SinThetaZ,	CosThetaZ,				0	},
				new double[] {	0,			0,						1	}
			});
			convMat41 = new Matrix(new double[][] {
				new double[] {	X3	},
				new double[] {	Y3	},
				new double[] {	Z3	}
			});
			convMat42 = new Matrix(new double[][] {
				new double[] {	Cube.CameraX	},
				new double[] {	Cube.CameraY	},
				new double[] {	Cube.CameraZ	}
			});
			convMat4 = convMat41.Clone();
			convMat4.Subtract(convMat42);
			DsubXYZ = ((convMat1.Multiply(convMat2)).Multiply(convMat3)).Multiply(convMat4);
			BsubX = (DsubXYZ[0, 0] - Cube.ViewerX) / (Cube.ViewerZ / DsubXYZ[2, 0]);
			BsubY = (DsubXYZ[1, 0] - Cube.ViewerY) / (Cube.ViewerZ / DsubXYZ[2, 0]);
			X2 = (int)((BsubX * 50) + 250);
			Y2 = (int)((BsubY * 50) + 250);
			// Can include Math.Sqrt() here if needed. Math.Sqrt() is slow so its omitted. Doesn't change anything.
			distanceFromViewer = Math.Pow((X3 - Cube.ViewerX), 2) + Math.Pow((Y3 - Cube.ViewerY), 2) + Math.Pow((Z3 - Cube.ViewerZ), 2);
		}
		
		public void rotateVertexX(double degrees) {
			double Yold = Y3;
			Y3 = Y3 * Math.Cos(degrees) - Z3 * Math.Sin(degrees);
			Z3 = Yold * Math.Sin(degrees) + Z3 * Math.Cos(degrees);
		}
		public void rotateVertexY(double degrees) {
			double Xold = X3;
			X3 = X3 * Math.Cos(degrees) + Z3 * Math.Sin(degrees);
			Z3 = Xold * ((-1) * Math.Sin(degrees)) + Z3 * Math.Cos(degrees);
		}
		public void rotateVertexZ(double degrees) {
			double Xold = X3;
			X3 = X3 * Math.Cos(degrees) - Y3 * Math.Sin(degrees);
			Y3 = Xold * Math.Sin(degrees) + Y3 * Math.Cos(degrees);
		}
		
	}
}
