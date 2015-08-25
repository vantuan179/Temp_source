using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DragObjects : MonoBehaviour {
	private bool _mouseState;
	private GameObject Target;
	private GameObject[] darts = new GameObject[3];
	public GameObject objectDart;
	private Vector3 screenSpace;
	private Vector3 oldpositionTarget;
	private Vector3 oldRotationTarget;
	private Vector3 oldScaleTarget;
	private Vector3 oldRotationSpline;
	private Vector3 oldPostionnSpline;
	private Vector3 oldMouse;
	private Vector3 mouseSpeed;
	private float[] listRadius;
	private SplineWalker splineWalker;
	private Vector3 tmpV;

	private Vector3 oldPostionSpline;
	
	// Use this for initialization
	void Start () {
#if USE_REVMOB_ANDROID
		//for ads
		RevMobBanner banner = revmob.CreateBanner();
		banner.Show();
#endif
		string[] tubes = {
			"Tube01", "Tube02", "Tube03", "Tube04", "Tube05", "Tube06", "Tube07" 
		};
		listRadius = new float[tubes.Length];
		for (int i = 0; i < tubes.Length; i++) {
			GameObject obj = GameObject.Find(tubes[i]);
			listRadius[i] = obj.GetComponent<SphereCollider>().radius*transform.localScale.x;
		}
		for (int i = 0; i < darts.Length; i++) {
			darts[i] = Instantiate (objectDart) as GameObject;	
		}
		Target = darts [0];
		splineWalker = Target.GetComponent <SplineWalker> ();
		SplineWalker.listRadius = listRadius;
		tmpV = Target.transform.position - splineWalker.spline.transform.position;
		oldpositionTarget = Target.transform.position;
		oldRotationTarget = Target.transform.localEulerAngles;
		oldScaleTarget = Target.transform.localScale;
		oldRotationSpline = splineWalker.spline.transform.localEulerAngles;
		oldPostionnSpline = splineWalker.spline.transform.position;
	}
	
	// Update is called once per frame
	public void resetAllDarts(){
		foreach (GameObject target in darts) {
			resetDart(target);
			target.SetActive(false);
		}
	}
	public void resetDart(GameObject target){
		target.transform.position = oldpositionTarget;
		target.transform.localEulerAngles = oldRotationTarget;
		target.transform.localScale = oldScaleTarget;
	}


	public float distanceMax2Target = 0f;
	public Vector3 getInCenterOfDarts(){
		SplineWalker[] splineEalkers = new SplineWalker[3];
		splineEalkers[0] = darts [0].GetComponent <SplineWalker> ();
		splineEalkers[1] = darts [1].GetComponent <SplineWalker> ();
		splineEalkers[2] = darts [2].GetComponent <SplineWalker> ();
		int count = 0;
		for (int i = 0; i < splineEalkers.Length; i++) {
			if (!splineEalkers[i].dartOutScreen) count++;
		}
		switch(count) {
		case 3:
			float a = Vector3.Distance (darts [1].transform.position, darts [2].transform.position);
			float b = Vector3.Distance (darts [0].transform.position, darts [2].transform.position);
			float c = Vector3.Distance (darts [0].transform.position, darts [1].transform.position);
			distanceMax2Target = Mathf.Max(a, b, c);
			float P = a + b + c;
			Vector3 result = new Vector3 ((a * darts [0].transform.position.x + b * darts [1].transform.position.x + c * darts [2].transform.position.x) / P,
                      (a * darts [0].transform.position.y + b * darts [1].transform.position.y + c * darts [2].transform.position.y) / P,
                      darts [0].transform.position.z);
			return result;
		case 2:
			Vector3[] p = new Vector3[2];
			int num = 0;
			for (int i = 0; i < splineEalkers.Length; i++) {
				if (!splineEalkers[i].dartOutScreen) {
					p[num] = splineEalkers[num++].transform.position;
				}
			}
			distanceMax2Target = Vector3.Distance(p[0], p[1]);
			return new Vector3((p[0].x + p[1].x)/2, (p[0].y + p[1].y)/2, p[0].z);
		case 1:
			Vector3 point = Vector3.zero;
			for (int i = 0; i < splineEalkers.Length; i++) {
				if (!splineEalkers[i].dartOutScreen) {
					point = splineEalkers[i].transform.position;
					break;
				}
			}
			return point;

		};
		Camera cam = GameObject.Find("Main Camera").camera;
		return cam.transform.position+cam.transform.forward;
	}
	void Update () {
		if (!SplineWalker.normalMode)
			return;
		if (Input.GetMouseButtonDown (0)) {
			if(SplineWalker.resetDart) {
				SplineWalker.resetDart = false;
				resetAllDarts();
			}
			Target = darts[SplineWalker.s_count];
			splineWalker = Target.GetComponent <SplineWalker> ();
			splineWalker.spline.transform.localEulerAngles = oldRotationSpline;
			splineWalker.spline.transform.position = oldPostionnSpline;
			Target.SetActive(true);
			_mouseState = true;
			oldMouse = Input.mousePosition;
			screenSpace = Camera.main.WorldToScreenPoint (Target.transform.position);
			Target.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
		}
		if (Input.GetMouseButtonUp (0)) {
			float d = Vector2.Distance(Input.mousePosition, oldMouse);
			if(d > 5 && Input.mousePosition.y > oldMouse.y) {
				Vector2 v1 = new Vector2(0, 1);
				Vector2 v2 = new Vector2(Input.mousePosition.x - oldMouse.x, Input.mousePosition.y - oldMouse.y);
				float angle = Vector2.Angle(v1, v2);
				splineWalker.spline.transform.Rotate(new Vector3(0, 0, 1), (Input.mousePosition.x > oldMouse.x ? -angle : angle));
				splineWalker.spline.transform.position = Target.transform.position - tmpV;
				_mouseState = false;
				splineWalker.isThrowDart = true;
			} else {
				Target.SetActive(false);
			}
		}
		if (_mouseState) {
			//keep track of the mouse position
			var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
			
			//convert the screen mouse position to world point and adjust with offset
			var curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace);
			
			//update the position of the object in the world
			Target.transform.position = new Vector3(curPosition.x, curPosition.y+0.3f,curPosition.z);
		}
	}
#if USE_REVMOB_ANDROID
	//for ads
	private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
		{ "Android", "55d9e985d247913e04c81c8a"}
	};
	private RevMob revmob;
	
	void Awake() {
		revmob = RevMob.Start(REVMOB_APP_IDS, "Darts for real money");
	}
#endif
}

