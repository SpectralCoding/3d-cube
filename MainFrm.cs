using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3DCube
{
	public partial class MainFrm : Form
	{
		
		private int prevTicks;
		private int rotateDirectionX, rotateDirectionY, rotateDirectionZ, rotationCount;
		public MainFrm()
		{
			InitializeComponent();
		}

		private void MainFrm_Load(object sender, EventArgs e)
		{
			chkDrawCorners.Checked = Cube.drawCorners;
			chkDrawWireframe.Checked = Cube.drawWireframe;
			chkDrawFaces.Checked = Cube.drawFaces;
			chkDrawLight.Checked = Cube.drawLight;
			combShading.SelectedIndex = Cube.drawShadowMode;
			chkDrawSurfaceNormals.Checked = Cube.drawSurfaceNormals;
			Cube.loadVerticies();
			Cube.loadFaces();
			ViewerX.Text = Cube.ViewerX.ToString();
			ViewerY.Text = Cube.ViewerY.ToString();
			ViewerZ.Text = Cube.ViewerZ.ToString();
			CameraX.Text = Cube.CameraX.ToString();
			CameraY.Text = Cube.CameraY.ToString();
			CameraZ.Text = Cube.CameraZ.ToString();
			ThetaX.Text = Cube.ThetaX.ToString();
			ThetaY.Text = Cube.ThetaY.ToString();
			ThetaZ.Text = Cube.ThetaZ.ToString();
			LightX.Text = Cube.LightX.ToString();
			LightY.Text = Cube.LightY.ToString();
			LightZ.Text = Cube.LightZ.ToString();
			combShading.SelectedIndex = 1;
			//StartStopCmd_Click(this, e);
		}

		private void printCoordsCmd_Click(object sender, EventArgs e)
		{
			Cube.printCoords();
		}

		private void VertexTimer_Tick(object sender, EventArgs e) {
			try {
				int ticks;
				double FPS;
				ticks = System.Environment.TickCount;
				FPS = 1000 / (ticks - prevTicks);
				prevTicks = ticks;
				FPSLbl.Text = Math.Round(FPS, 0) + " FPS";
			} catch (Exception) { }
			Cube.calc2DCoords();
			Cube.calcFaces();
			V13DCoord.Text = Cube.vertex[0].view3DCoords(); V12DCoord.Text = Cube.vertex[0].view2DCoords(); V1Distance.Text = Cube.vertex[0].viewDistanceFromViewer();
			V23DCoord.Text = Cube.vertex[1].view3DCoords(); V22DCoord.Text = Cube.vertex[1].view2DCoords(); V2Distance.Text = Cube.vertex[1].viewDistanceFromViewer();
			V33DCoord.Text = Cube.vertex[2].view3DCoords(); V32DCoord.Text = Cube.vertex[2].view2DCoords(); V3Distance.Text = Cube.vertex[2].viewDistanceFromViewer();
			V43DCoord.Text = Cube.vertex[3].view3DCoords(); V42DCoord.Text = Cube.vertex[3].view2DCoords(); V4Distance.Text = Cube.vertex[3].viewDistanceFromViewer();
			V53DCoord.Text = Cube.vertex[4].view3DCoords(); V52DCoord.Text = Cube.vertex[4].view2DCoords(); V5Distance.Text = Cube.vertex[4].viewDistanceFromViewer();
			V63DCoord.Text = Cube.vertex[5].view3DCoords(); V62DCoord.Text = Cube.vertex[5].view2DCoords(); V6Distance.Text = Cube.vertex[5].viewDistanceFromViewer();
			V73DCoord.Text = Cube.vertex[6].view3DCoords(); V72DCoord.Text = Cube.vertex[6].view2DCoords(); V7Distance.Text = Cube.vertex[6].viewDistanceFromViewer();
			V83DCoord.Text = Cube.vertex[7].view3DCoords(); V82DCoord.Text = Cube.vertex[7].view2DCoords(); V8Distance.Text = Cube.vertex[7].viewDistanceFromViewer();
			picCube.Image = Cube.drawCube();
			picCube.Refresh();
		}

		private void ApplyCmd_Click(object sender, EventArgs e)
		{
			Cube.ViewerX = Convert.ToDouble(ViewerX.Text);
			Cube.ViewerY = Convert.ToDouble(ViewerY.Text);
			Cube.ViewerZ = Convert.ToDouble(ViewerZ.Text);
			Cube.CameraX = Convert.ToDouble(CameraX.Text);
			Cube.CameraY = Convert.ToDouble(CameraY.Text);
			Cube.CameraZ = Convert.ToDouble(CameraZ.Text);
			Cube.ThetaX = Convert.ToDouble(ThetaX.Text);
			Cube.ThetaY = Convert.ToDouble(ThetaY.Text);
			Cube.ThetaZ = Convert.ToDouble(ThetaZ.Text);
			Cube.LightX = Convert.ToDouble(LightX.Text);
			Cube.LightY = Convert.ToDouble(LightY.Text);
			Cube.LightZ = Convert.ToDouble(LightZ.Text);
		}

		private void RotateCmd_Click(object sender, EventArgs e)
		{
			if (RotationX.Text == "") { RotationX.Text = "0"; }
			if (RotationY.Text == "") { RotationY.Text = "0"; }
			if (RotationZ.Text == "") { RotationZ.Text = "0"; }
			Cube.rotateCubeX(Convert.ToDouble(RotationX.Text));
			Cube.rotateCubeY(Convert.ToDouble(RotationY.Text));
			Cube.rotateCubeZ(Convert.ToDouble(RotationZ.Text));
		}

		private void RotateUpCmd_Click(object sender, EventArgs e)
		{
			Cube.rotateCubeX(0.05);
		}

		private void RotateLeftCmd_Click(object sender, EventArgs e)
		{
			Cube.rotateCubeY(-0.05);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
				case Keys.Left:
					Cube.rotateCubeY(-0.05);
					break;
				case Keys.Right:
					Cube.rotateCubeY(0.05);
					break;
				case Keys.Up:
					Cube.rotateCubeX(0.05);
					break;
				case Keys.Down:
					Cube.rotateCubeX(-0.05);
					break;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void StartStopCmd_Click(object sender, EventArgs e)
		{
			if (StartStopCmd.Text == "Start Rotating") {
				StartStopCmd.Text = "Stop Rotating";
				int tempRandom;
				Random random = new Random();
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionX = -1; } else if (tempRandom == 2) { rotateDirectionX = 0; } else if (tempRandom == 3) { rotateDirectionX = 1; }
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionY = -1; } else if (tempRandom == 2) { rotateDirectionY = 0; } else if (tempRandom == 3) { rotateDirectionY = 1; }
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionZ = -1; } else if (tempRandom == 2) { rotateDirectionZ = 0; } else if (tempRandom == 3) { rotateDirectionZ = 1; }
				RandomRotateTimer.Enabled = true;
			} else if (StartStopCmd.Text == "Stop Rotating") {
				StartStopCmd.Text = "Start Rotating";
				RandomRotateTimer.Enabled = false;
			}
		}

		private void chkDrawCorners_CheckedChanged(object sender, EventArgs e)
		{
			Cube.drawCorners = chkDrawCorners.Checked;
		}

		private void chkDrawWireframe_CheckedChanged(object sender, EventArgs e)
		{
			Cube.drawWireframe = chkDrawWireframe.Checked;
		}

		private void chkDrawFaces_CheckedChanged(object sender, EventArgs e)
		{
			Cube.drawFaces = chkDrawFaces.Checked;
		}

		private void RandomRotateTimer_Tick(object sender, EventArgs e)
		{
			Cube.rotateCubeX((0.025 * rotateDirectionX));
			Cube.rotateCubeY((0.025 * rotateDirectionY));
			Cube.rotateCubeZ((0.025 * rotateDirectionZ));
			rotationCount++;
			if (rotationCount > 100) {
				int tempRandom;
				Random random = new Random();
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionX = -1; } else if (tempRandom == 2) { rotateDirectionX = 0; } else if (tempRandom == 3) { rotateDirectionX = 1; }
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionY = -1; } else if (tempRandom == 2) { rotateDirectionY = 0; } else if (tempRandom == 3) { rotateDirectionY = 1; }
				tempRandom = random.Next(1, 4);
				if (tempRandom == 1) { rotateDirectionZ = -1; } else if (tempRandom == 2) { rotateDirectionZ = 0; } else if (tempRandom == 3) { rotateDirectionZ = 1; }
				rotationCount = 0;
			}
		}

		private void combShading_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (combShading.SelectedItem.ToString() == "Flat Shading") {
				chkDrawSurfaceNormals.Enabled = true;
			} else {
				chkDrawSurfaceNormals.Checked = false;
				chkDrawSurfaceNormals.Enabled = false;
			}
			Cube.drawShadowMode = combShading.SelectedIndex;
		}

		private void chkDrawLight_CheckedChanged(object sender, EventArgs e)
		{
			Cube.drawLight = chkDrawLight.Checked;
		}

		private void chkDrawSurfaceNormals_CheckedChanged(object sender, EventArgs e)
		{
			Cube.drawSurfaceNormals = chkDrawSurfaceNormals.Checked;
		}
		
	}
}
