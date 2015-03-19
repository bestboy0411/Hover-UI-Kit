﻿using System;
using System.Collections.Generic;
using Hover.Board.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiProjectionRenderer : MonoBehaviour {

		private ProjectionState vProjectionState;
		private GameObject vDotObj;
		private GameObject vBarObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ProjectionState pProjectionState) {
			vProjectionState = pProjectionState;

			////

			vDotObj = new GameObject("Dot");
			vDotObj.transform.SetParent(gameObject.transform, false);
			vDotObj.transform.localScale = Vector3.zero;

			MeshFilter dotMeshFilt = vDotObj.AddComponent<MeshFilter>();
			BuildCircleMesh(dotMeshFilt.mesh, 0.5f, 32);

			MeshRenderer dotMeshRend = vDotObj.AddComponent<MeshRenderer>();
			dotMeshRend.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			dotMeshRend.sharedMaterial.renderQueue += 100;

			////

			vBarObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vBarObj.name = "Bar";
			vBarObj.transform.SetParent(gameObject.transform, false);
			vBarObj.transform.localScale = Vector3.zero;
			vBarObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBarObj.renderer.sharedMaterial.renderQueue += 200;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float dist = vProjectionState.ProjectedPanelDistance;
			float prog = vProjectionState.ProjectedPanelProgress;
			float barThick = 0.01f*vProjectionState.CursorState.Size;
			float dotSize = (1-prog)*60 + 5;

			Vector3 dotScale = Vector3.one*barThick*dotSize;
			dotScale.y *= (vProjectionState.ProjectedFromFront ? -1 : 1);

			Color col = Color.white; //TODO: vSettings.ColorNorm;
			col.a *= (float)Math.Pow(prog, 2);

			vDotObj.renderer.sharedMaterial.color = col;
			vDotObj.transform.localScale = dotScale;

			vBarObj.renderer.sharedMaterial.color = col;
			vBarObj.transform.localScale = new Vector3(barThick, dist, barThick);
			vBarObj.transform.localPosition = new Vector3(0, vBarObj.transform.localScale.y/2f, 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Vector3 GetRingPoint(float pRadius, float pAngle) {
			float x = (float)Math.Sin(pAngle);
			float y = (float)Math.Cos(pAngle);
			return new Vector3(x*pRadius, 0, y*pRadius);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static void BuildCircleMesh(Mesh pMesh, float pRadius, int pSteps) {
			const float angleFull = (float)Math.PI*2;
			float angleInc = angleFull/pSteps;
			float angle = 0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			verts.Add(Vector3.zero);
			uvs.Add(new Vector2(0, 0));

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				int vi = verts.Count;
				float uvx = i/(float)pSteps;

				verts.Add(GetRingPoint(pRadius, angle));
				uvs.Add(new Vector2(uvx, 1));

				if ( i > 0 ) {
					tris.Add(0);
					tris.Add(vi-1);
					tris.Add(vi);
				}

				angle += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}

	}

}