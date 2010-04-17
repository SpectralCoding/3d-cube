using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace _3DCube
{
	public static class Camera	{
		public static double X = 0;
		public static double Y = 0;
		public static double Z = 10;
		public static double RotationX = 0;
		public static double RotationY = 0;
		public static double RotationZ = 0;
		
		public static double[] cameraTransform(double PointX, double PointY, double PointZ) {
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
			// calculations assume a left-handed system of axes)."
			MathNet.Numerics.LinearAlgebra.Matrix convMat1, convMat2, convMat3, convMat4, convMat41, convMat42, DsubXYZ;
			double CosThetaX = Math.Cos(Camera.RotationX); double SinThetaX = Math.Sin(Camera.RotationX);
			double CosThetaY = Math.Cos(Camera.RotationY); double SinThetaY = Math.Sin(Camera.RotationY);
			double CosThetaZ = Math.Cos(Camera.RotationZ); double SinThetaZ = Math.Sin(Camera.RotationZ);
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
				new double[] {	Camera.X	},
				new double[] {	Camera.Y	},
				new double[] {	Camera.Z	}
			});
			convMat4 = convMat41.Clone();
			convMat4.Subtract(convMat42);
			DsubXYZ = ((convMat1.Multiply(convMat2)).Multiply(convMat3)).Multiply(convMat4);
			double[] returnVals = new double[3];
			returnVals[0] = DsubXYZ[0, 0]; returnVals[1] = DsubXYZ[1, 0]; returnVals[2] = DsubXYZ[2, 0];
			return returnVals;
		}
		
	}
}
