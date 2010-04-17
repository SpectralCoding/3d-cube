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
			chkDrawCorners.Checked = Render.drawCorners;
			chkDrawWireframe.Checked = Render.drawWireframe;
			chkDrawFaces.Checked = Render.drawFaces;
			chkDrawLight.Checked = Render.drawLight;
			combShading.SelectedIndex = Render.drawShadowMode;
			chkDrawSurfaceNormals.Checked = Render.drawSurfaceNormals;
			ViewerX.Text = Viewer.X.ToString();
			ViewerY.Text = Viewer.Y.ToString();
			ViewerZ.Text = Viewer.Z.ToString();
			CameraX.Text = Camera.X.ToString();
			CameraY.Text = Camera.Y.ToString();
			CameraZ.Text = Camera.Z.ToString();
			ThetaX.Text = Camera.RotationX.ToString();
			ThetaY.Text = Camera.RotationY.ToString();
			ThetaZ.Text = Camera.RotationZ.ToString();
			LightX.Text = Render.Light.X.ToString();
			LightY.Text = Render.Light.Y.ToString();
			LightZ.Text = Render.Light.Z.ToString();
			combShading.SelectedIndex = 1;
			//StartStopCmd_Click(this, e);
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
			V13DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(0); V12DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(0); V1Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(0);
			V23DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(1); V22DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(1); V2Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(1);
			V33DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(2); V32DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(2); V3Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(2);
			V43DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(3); V42DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(3); V4Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(3);
			V53DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(4); V52DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(4); V5Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(4);
			V63DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(5); V62DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(5); V6Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(5);
			V73DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(6); V72DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(6); V7Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(6);
			V83DCoord.Text = Render.Cube.Mesh.viewVertex3DCoords(7); V82DCoord.Text = Render.Cube.Mesh.viewVertex2DCoords(7); V8Distance.Text = Render.Cube.Mesh.viewVertexDistanceFromCameraSquared(7);
			picCube.Image = Render.drawObject();
			picCube.Refresh();
			VertexTimer.Enabled = false;
		}

		private void ApplyCmd_Click(object sender, EventArgs e)
		{
			if (ViewerX.Text == "") { ViewerX.Text = "0"; } if (ViewerY.Text == "") { ViewerY.Text = "0"; } if (ViewerZ.Text == "") { ViewerZ.Text = "0"; }
			if (CameraX.Text == "") { CameraX.Text = "0"; } if (CameraY.Text == "") { CameraY.Text = "0"; } if (CameraZ.Text == "") { CameraZ.Text = "0"; }
			if (ThetaX.Text == "") { ThetaX.Text = "0"; } if (ThetaY.Text == "") { ThetaY.Text = "0"; } if (ThetaZ.Text == "") { ThetaZ.Text = "0"; }
			if (LightX.Text == "") { LightX.Text = "0"; } if (LightY.Text == "") { LightY.Text = "0"; } if (LightZ.Text == "") { LightZ.Text = "0"; }
			Viewer.X = Convert.ToDouble(ViewerX.Text);
			Viewer.Y = Convert.ToDouble(ViewerY.Text);
			Viewer.Z = Convert.ToDouble(ViewerZ.Text);
			Camera.X = Convert.ToDouble(CameraX.Text);
			Camera.Y = Convert.ToDouble(CameraY.Text);
			Camera.Z = Convert.ToDouble(CameraZ.Text);
			Camera.RotationX = Convert.ToDouble(ThetaX.Text);
			Camera.RotationY = Convert.ToDouble(ThetaY.Text);
			Camera.RotationZ = Convert.ToDouble(ThetaZ.Text);
			Render.Light.X = Convert.ToDouble(LightX.Text);
			Render.Light.Y = Convert.ToDouble(LightY.Text);
			Render.Light.Z = Convert.ToDouble(LightZ.Text);
		}

		private void RotateCmd_Click(object sender, EventArgs e)
		{
			if (RotationX.Text == "") { RotationX.Text = "0"; } if (RotationY.Text == "") { RotationY.Text = "0"; } if (RotationZ.Text == "") { RotationZ.Text = "0"; }
			Render.Cube.rotateCubeX(Convert.ToDouble(RotationX.Text));
			Render.Cube.rotateCubeY(Convert.ToDouble(RotationY.Text));
			Render.Cube.rotateCubeZ(Convert.ToDouble(RotationZ.Text));
		}

		private void RotateUpCmd_Click(object sender, EventArgs e) {
			Render.Cube.rotateCubeX(0.05);
		}

		private void RotateLeftCmd_Click(object sender, EventArgs e) {
			Render.Cube.rotateCubeY(-0.05);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Left:
					Render.Cube.rotateCubeY(-0.05);
					break;
				case Keys.Right:
					Render.Cube.rotateCubeY(0.05);
					break;
				case Keys.Up:
					Render.Cube.rotateCubeX(0.05);
					break;
				case Keys.Down:
					Render.Cube.rotateCubeX(-0.05);
					break;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e) { }

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

		private void chkDrawCorners_CheckedChanged(object sender, EventArgs e) {
			Render.drawCorners = chkDrawCorners.Checked;
		}

		private void chkDrawWireframe_CheckedChanged(object sender, EventArgs e) {
			Render.drawWireframe = chkDrawWireframe.Checked;
		}

		private void chkDrawFaces_CheckedChanged(object sender, EventArgs e) {
			Render.drawFaces = chkDrawFaces.Checked;
		}

		private void RandomRotateTimer_Tick(object sender, EventArgs e) {
			Render.Cube.rotateCubeX((0.025 * rotateDirectionX));
			Render.Cube.rotateCubeY((0.025 * rotateDirectionY));
			Render.Cube.rotateCubeZ((0.025 * rotateDirectionZ));
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

		private void combShading_SelectedIndexChanged(object sender, EventArgs e) {
			if (combShading.SelectedItem.ToString() == "Flat Shading") {
				chkDrawSurfaceNormals.Enabled = true;
			} else {
				chkDrawSurfaceNormals.Checked = false;
				chkDrawSurfaceNormals.Enabled = false;
			}
			Render.drawShadowMode = combShading.SelectedIndex;
		}

		private void chkDrawLight_CheckedChanged(object sender, EventArgs e) {
			Render.drawLight = chkDrawLight.Checked;
		}

		private void chkDrawSurfaceNormals_CheckedChanged(object sender, EventArgs e)
		{
			Render.drawSurfaceNormals = chkDrawSurfaceNormals.Checked;
		}
		
	}
}
