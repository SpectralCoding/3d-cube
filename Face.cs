using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _3DCube
{
	class Face
	{
		public int[] vertexIndicies = new int[4];
		public Point[][] polygonPoints = new Point[2][];
		public Vector3[] surfaceNormal = new Vector3[2];
		public double[][] surfaceCenterPoint = new double[2][];
		public Vector3[] lightingNormal = new Vector3[2];
		public Pen[] polygonPen = new Pen[2];
		public SolidBrush[] polygonBrush = new SolidBrush[2];
		public Pen polygonUnshadedPen;
		public SolidBrush polygonUnshadedBrush;

		public Face() {
			polygonPoints[0] = new Point[3];
			polygonPoints[1] = new Point[3];
			surfaceCenterPoint[0] = new double[3];
			surfaceCenterPoint[1] = new double[3];
		}
		
		public void setColor(Color inputColor) {
			polygonUnshadedPen = new Pen(inputColor, 1);
			polygonUnshadedBrush = new SolidBrush(inputColor);
			polygonBrush[0] = polygonUnshadedBrush;
			polygonBrush[1] = polygonUnshadedBrush;
			polygonPen[0] = polygonUnshadedPen;
			polygonPen[1] = polygonUnshadedPen;
		}
		
		public void setVerticies(int vertex1, int vertex2, int vertex3, int vertex4) {
			vertexIndicies[0] = vertex1;
			vertexIndicies[1] = vertex2;
			vertexIndicies[2] = vertex3;
			vertexIndicies[3] = vertex4;
		}
		
		public void setPolygons() {
			// 1, 2, 6, 5
			// 1, 2, 5 = 0, 1, 3
			// 5, 2, 6 = 3, 1, 2
			polygonPoints[0][0] = new Point(Cube.vertex[vertexIndicies[0]].X2, Cube.vertex[vertexIndicies[0]].Y2);
			polygonPoints[0][1] = new Point(Cube.vertex[vertexIndicies[1]].X2, Cube.vertex[vertexIndicies[1]].Y2);
			polygonPoints[0][2] = new Point(Cube.vertex[vertexIndicies[3]].X2, Cube.vertex[vertexIndicies[3]].Y2);
			polygonPoints[1][0] = new Point(Cube.vertex[vertexIndicies[3]].X2, Cube.vertex[vertexIndicies[3]].Y2);
			polygonPoints[1][1] = new Point(Cube.vertex[vertexIndicies[1]].X2, Cube.vertex[vertexIndicies[1]].Y2);
			polygonPoints[1][2] = new Point(Cube.vertex[vertexIndicies[2]].X2, Cube.vertex[vertexIndicies[2]].Y2);
			if (Cube.drawShadowMode == 0) {
				// No Shading
				Cube.drawSurfaceNormals = false;
				polygonBrush[0] = polygonUnshadedBrush;
				polygonBrush[1] = polygonUnshadedBrush;
				polygonPen[0] = polygonUnshadedPen;
				polygonPen[1] = polygonUnshadedPen;				
			} else if (Cube.drawShadowMode == 1) {
				calcSurfaceNormals();
				calcLightingNormals();
				fixPolygonNormals();
				double dotProduct;
				int iR, iG, iB;
				dotProduct = Vector3.DotProduct(lightingNormal[0], surfaceNormal[0]); dotProduct = 1 - dotProduct;
				iR = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.R, dotProduct, 0, 255));
				iG = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.G, dotProduct, 0, 255));
				iB = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.B, dotProduct, 0, 255));
				polygonBrush[0] = new SolidBrush(Color.FromArgb(iR, iG, iB));
				//Console.WriteLine("RGB(" + iR + "," + iG + "," + iB + ")*" + Math.Round(dotProduct, 3) + " -> RGB(" + polygonBrush[0].Color.R + "," + polygonBrush[0].Color.G + "," + polygonBrush[0].Color.B + ")");
				iR = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.R, dotProduct, 0, 255));
				iG = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.G, dotProduct, 0, 255));
				iB = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.B, dotProduct, 0, 255));
				polygonPen[0] = new Pen(Color.FromArgb(iR, iG, iB));
				dotProduct = Vector3.DotProduct(lightingNormal[1], surfaceNormal[1]); dotProduct = 1 - dotProduct;
				iR = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.R, dotProduct, 0, 255));
				iG = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.G, dotProduct, 0, 255));
				iB = (int)(Funct.ClippedMultiply(polygonUnshadedBrush.Color.B, dotProduct, 0, 255));
				polygonBrush[1] = new SolidBrush(Color.FromArgb(iR, iG, iB));
				iR = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.R, dotProduct, 0, 255));
				iG = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.G, dotProduct, 0, 255));
				iB = (int)(Funct.ClippedMultiply(polygonUnshadedPen.Color.B, dotProduct, 0, 255));
				polygonPen[1] = new Pen(Color.FromArgb(iR, iG, iB));
			}
		}
		
		public double polygonAvgDistance() {
			double tempAdder = 0;
			for (int i = 0; i < 4; i++) { tempAdder += Cube.vertex[vertexIndicies[i]].distanceFromViewer; }
			tempAdder = tempAdder / 4;
			return tempAdder;
		}
		
		private void calcLightingNormals() {
			double CentroidX, CentroidY, CentroidZ;
			CentroidX = (Cube.vertex[vertexIndicies[0]].X3 + Cube.vertex[vertexIndicies[1]].X3 + Cube.vertex[vertexIndicies[3]].X3) / 3;
			CentroidY = (Cube.vertex[vertexIndicies[0]].Y3 + Cube.vertex[vertexIndicies[1]].Y3 + Cube.vertex[vertexIndicies[3]].Y3) / 3;
			CentroidZ = (Cube.vertex[vertexIndicies[0]].Z3 + Cube.vertex[vertexIndicies[1]].Z3 + Cube.vertex[vertexIndicies[3]].Z3) / 3;
			surfaceCenterPoint[0][0] = CentroidX; surfaceCenterPoint[0][1] = CentroidY; surfaceCenterPoint[0][2] = CentroidZ; 
			lightingNormal[0] = new Vector3(
				(Cube.LightX - CentroidX), (Cube.LightY - CentroidY), (Cube.LightZ - CentroidZ) 
			);
			CentroidX = (Cube.vertex[vertexIndicies[3]].X3 + Cube.vertex[vertexIndicies[1]].X3 + Cube.vertex[vertexIndicies[2]].X3) / 3;
			CentroidY = (Cube.vertex[vertexIndicies[3]].Y3 + Cube.vertex[vertexIndicies[1]].Y3 + Cube.vertex[vertexIndicies[2]].Y3) / 3;
			CentroidZ = (Cube.vertex[vertexIndicies[3]].Z3 + Cube.vertex[vertexIndicies[1]].Z3 + Cube.vertex[vertexIndicies[2]].Z3) / 3;
			surfaceCenterPoint[1][0] = CentroidX; surfaceCenterPoint[1][1] = CentroidY; surfaceCenterPoint[1][2] = CentroidZ; 
			lightingNormal[1] = new Vector3(
				(Cube.LightX - CentroidX), (Cube.LightY - CentroidY), (Cube.LightZ - CentroidZ)
			);
		}
		
		private void calcSurfaceNormals() {
			surfaceNormal[0] = calcPolygonNormal(Cube.vertex[vertexIndicies[0]], Cube.vertex[vertexIndicies[1]], Cube.vertex[vertexIndicies[3]]);
			surfaceNormal[1] = calcPolygonNormal(Cube.vertex[vertexIndicies[3]], Cube.vertex[vertexIndicies[1]], Cube.vertex[vertexIndicies[2]]);
		}
		
		private Vector3 calcPolygonNormal(Vertex vertex1, Vertex vertex2, Vertex vertex3) {
			Vector3 vectorU = new Vector3(
				(vertex2.X3 - vertex1.X3), (vertex2.Y3 - vertex1.Y3), (vertex2.Z3 - vertex1.Z3)
			);
			Vector3 vectorV = new Vector3(
				(vertex3.X3 - vertex1.X3), (vertex3.Y3 - vertex1.Y3), (vertex3.Z3 - vertex1.Z3)
			);
			return (new Vector3(
				((vectorU.Y * vectorV.Z) - (vectorU.Z * vectorV.Y)),
				((vectorU.Z * vectorV.X) - (vectorU.X * vectorV.Z)),
				((vectorU.X * vectorV.Y) - (vectorU.Y * vectorV.X))
			));
		}
		
		private void fixPolygonNormals() {
			for (int i = 0; i < 2; i++) {
				lightingNormal[i].Normalize();
				Vector3 icVector = new Vector3(
					(0 - surfaceCenterPoint[i][0]), (0 - surfaceCenterPoint[i][1]), (0 - surfaceCenterPoint[i][2])
				);
				double dotProduct;
				dotProduct = Vector3.DotProduct(surfaceNormal[i], icVector);
				if (dotProduct > 0) {
					surfaceNormal[i] = surfaceNormal[i] * -1;
				}
			}
		}
		
	}
}
