﻿using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO
{


	public class FourDshape1 : WiiGlobalReceiver, IGlobalWiiMoteBHandler, IGlobalWiiMoteAHandler
	{
		public Transform box;
		public Trackball trackball;
		public FourDshape cell;
		public Vector3 A_;
		public Vector3 B_;
		public Vector4 A, B;
		bool isbutton;
		public GameObject Trackball;
		float radius;
		public Vector3 movement;


		void UpdateRotation (FourDshape cell, Trackball trackball, Vector4 A_, Vector4 B_)
		{

			float[] A = new float[4]{ A_.x, A_.y, A_.z, A_.w };
			float[] B = new float[4]{ B_.x, B_.y, B_.z, B_.w };

			trackball.rotate (A, B);

			for (int i = 0; i < 8; i++) {

				float[] src = new float[4];
				src [0] = cell.srcVertices [i].x;
				src [1] = cell.srcVertices [i].y;
				src [2] = cell.srcVertices [i].z;
				src [3] = cell.srcVertices [i].w;
				float[] dst = new float[4];

				trackball.transform (src, dst);

				cell.updatepoint4 (dst, i);
				cell.update_edges ();
			}
		}

		// Use this for initialization
		void Start ()
		{
			box = this.GetComponent<Transform> ();
			trackball = new Trackball (4);
			cell = new FourDshape (box);
			box.position = new Vector3 (0f, 1.5f, -0.9f);
			A_ = new Vector3 ();
			B_ = new Vector3 ();
			isbutton = false;
			A = new Vector4 ();
			B = new Vector4 ();
			//radius = Trackball.GetComponent<SphereCollider> ().radius;
			radius = 1.0f;

		}

		// Update is called once per frame
		void Update ()
		{
			if (isbutton) {
				Vector3 relapos = new Vector3 ();
				relapos = (B_ - box.position)*8f/3f;
				float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
				if (r < radius) {					
					B = new Vector4 (relapos.x, relapos.y, relapos.z, (float)Math.Sqrt (radius*radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
				}
				else{
					//float length = relapos.magnitude;
					Vector3 Q = (radius / r) * relapos;
					relapos = Q + box.position;
					B = new Vector4 (Q.x, Q.y, Q.z, 0f);
				}
				UpdateRotation (cell, trackball, A, B);
				A = B;
			}
		}


		public void OnGlobalBPress (WiiMoteEventData eventData)
		{
			isbutton = true;
			B_ = eventData.module.transform.position;
			Debug.Log (eventData.module.transform.position);
		}

		public void OnGlobalBPressDown (WiiMoteEventData eventData)
		{
			isbutton = true;
			B_ = eventData.module.transform.position;
			Vector3 relapos = new Vector3 ();
			relapos = (B_ - box.position)*8f/3f;
			float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
			if (r < radius) {					
				B = new Vector4 (relapos.x, relapos.y, relapos.z, (float)Math.Sqrt (radius*radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
			}
			else{
				//float length = relapos.magnitude;
				Vector3 Q = (radius / r) * relapos;
				//relapos = Q + box.position;
				B = new Vector4 (Q.x, Q.y, Q.z, 0f);
			}
			A = B;
			Debug.Log (eventData.module.transform.position);

		}
		public void OnGlobalBPressUp (WiiMoteEventData eventData)
		{
			isbutton = false;
			A_ = eventData.module.transform.position;
			B_ = eventData.module.transform.position;
			Debug.Log (eventData.module.transform.position);
		}
		public void OnGlobalAPressDown(WiiMoteEventData eventData)
		{
			movement = new Vector3 ();
			movement = box.position - eventData.module.transform.position;
		}

		public void OnGlobalAPress (WiiMoteEventData eventData)
		{
			box.position = eventData.module.transform.position + movement;
		}
		public void OnGlobalAPressUp (WiiMoteEventData eventData)
		{
		}

	}

}
