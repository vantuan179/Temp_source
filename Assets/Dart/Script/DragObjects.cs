using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public Text remainScoreText;
	public Text[] textGetScore;
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
			CircleCollider2D circleCollider2D = obj.GetComponent<CircleCollider2D>();
			listRadius[i] = circleCollider2D.radius*circleCollider2D.transform.localScale.x*transform.localScale.x;
		}
		for (int i = 0; i < darts.Length; i++) {
			darts[i] = Instantiate (objectDart) as GameObject;	
		}
		Target = darts [0];
		splineWalker = Target.GetComponent <SplineWalker> ();
		SplineWalker.listRadius = listRadius;
		SplineWalker.remainScoreText = remainScoreText;
		SplineWalker.textGetScore = textGetScore;
		tmpV = Target.transform.position - splineWalker.spline.transform.position;
		oldpositionTarget = Target.transform.position;
		oldRotationTarget = Target.transform.localEulerAngles;
		oldScaleTarget = Target.transform.localScale;
		oldRotationSpline = splineWalker.spline.transform.localEulerAngles;
		oldPostionnSpline = splineWalker.spline.transform.position;
		cam = GameObject.Find("Main Camera").camera;

	}
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (new Vector3(0, 0, 1), 5);
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
		target.GetComponent <SplineWalker> ().dartOutScreen = true;
		foreach (Text text in textGetScore) {
			text.text = "0";		
		}
	}


	public float distanceMax2Target = 0f;
	public Vector3 getInCenterOfDarts(){
		SplineWalker[] splineWalkers = new SplineWalker[3];
		Vector3[] pos = new Vector3[3];
		splineWalkers[0] = darts [0].GetComponent <SplineWalker> ();
		splineWalkers[1] = darts [1].GetComponent <SplineWalker> ();
		splineWalkers[2] = darts [2].GetComponent <SplineWalker> ();
		int count = 0;
		for (int i = 0; i < splineWalkers.Length; i++) {
			if (!splineWalkers[i].dartOutScreen){
				pos[i] = darts [i].transform.position;
				count++;
			} else {
				pos[i] = Vector3.zero;
			}
		}

		switch(count) {
		case 3:
			float a = Vector3.Distance (pos [1], pos [2]);
			float b = Vector3.Distance (pos [0], pos [2]);
			float c = Vector3.Distance (pos [0], pos [1]);
			distanceMax2Target = Mathf.Max(a, b, c);
			float P = a + b + c;
			Vector3 result = new Vector3 ((a * pos [0].x + b * pos [1].x + c * pos [2].x) / P,
			                              (a * pos [0].y + b * pos [1].y + c * pos [2].y) / P,
			                              pos [0].z);
			Vector3 v1 = Vector3.zero;
			Vector3 v2 = Vector3.zero;
			Vector3 midpoint = Vector3.zero;
			if(distanceMax2Target == a) {
				v1 = pos [2] - pos [1];
				midpoint = (pos[1] + pos[2]) / 2;
			} else if(distanceMax2Target == b){
				v1 = pos [2] - pos [0];
				midpoint = (pos[0] + pos[2]) / 2;
			} else if(distanceMax2Target == c){
				v1 = pos [1] - pos [0];
				midpoint = (pos[0] + pos[1]) / 2;
			}
			v2 = new Vector3(-v1.y, v1.x, pos[0].z);
			float a1 = v1.x, b1 = v1.y, c1 = -(a1*midpoint.x + b1*midpoint.y);
			float a2 = v2.x, b2 = v2.y, c2 = -(a2*result.x + b2*result.y);
			float x = (c2*b1-c1*b2)/(a1*b2-a2*b1);
			float y = -(c2*a1-c1*a2)/(a1*b2-a2*b1);
			return new Vector3(x, y, pos [0].z);
		case 2:
			Vector3[] p = new Vector3[2];
			int num = 0;
			for (int i = 0; i < splineWalkers.Length; i++) {
				if (!splineWalkers[i].dartOutScreen) {
					p[num++] = pos[i];
				}
			}
			distanceMax2Target = Vector3.Distance(p[0], p[1]);
			return new Vector3((p[0].x + p[1].x)/2, (p[0].y + p[1].y)/2, p[0].z);
		case 1:
			for (int i = 0; i < splineWalkers.Length; i++) {
				if (!splineWalkers[i].dartOutScreen) {
					return pos[i];
				}
			}
			break;
		default:
			break;
		};
		return GameObject.Find ("ObjectBoard").transform.position;
	}
	Camera cam;
	int speed = 5;
	float friction = 1f;
	float duration = 3f;
	float lerpSpeed = 1f;
	private float xDeg;
	private float yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private float progress;
	private float angleRotationMax = 50;
	private float angleRotationX = 0;
	private float angleRotationY = 0;
	void updateRotateObject () {
		progress += Time.deltaTime / duration;
		Transform transformCam = GameObject.Find ("Main Camera").transform;
		if (Input.GetMouseButtonDown (0)) {
			_mouseState = true;	
			progress = 0;
		}
		if (Input.GetMouseButtonUp (0)) {
			progress = 0;
			xDeg = Input.GetAxis("Mouse X") * speed * friction;
			yDeg = Input.GetAxis("Mouse Y") * speed * friction;
			_mouseState = false;
		}
		if (!_mouseState && progress < duration / 10) {
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler(yDeg,xDeg,0);	
			float angle = Quaternion.Angle(fromRotation, toRotation);
		
			if(transformCam.eulerAngles.x > angleRotationMax && yDeg < 0 && transformCam.eulerAngles.x < 180)
				yDeg = -1*yDeg;
			else if (360 - transformCam.eulerAngles.x > angleRotationMax && yDeg > 0 && transformCam.eulerAngles.x > 180)
				yDeg = -1*yDeg;
			
			if(transformCam.eulerAngles.y > angleRotationMax && xDeg > 0 && transformCam.eulerAngles.y < 180) 
				xDeg = -1*xDeg;
			else if (360 - transformCam.eulerAngles.y > angleRotationMax && xDeg < 0 && transformCam.eulerAngles.y > 180) 
				xDeg = -1*xDeg;
			transformCam.RotateAround(Vector3.zero, new Vector3(-yDeg, xDeg ,0), angle*(Time.deltaTime / duration));
			transformCam.eulerAngles = new Vector3(transformCam.eulerAngles.x, transformCam.eulerAngles.y, 0);
		}
		if (_mouseState) {

			xDeg = Input.GetAxis("Mouse X") * speed * friction;
			yDeg = Input.GetAxis("Mouse Y") * speed * friction;
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler(yDeg,xDeg,0);	
			float angle = Quaternion.Angle(fromRotation, toRotation);
			if(transformCam.eulerAngles.x > angleRotationMax && yDeg < 0 && transformCam.eulerAngles.x < 180)
				yDeg = 0;
			else if (360 - transformCam.eulerAngles.x > angleRotationMax && yDeg > 0 && transformCam.eulerAngles.x > 180)
				yDeg = 0;

			if(transformCam.eulerAngles.y > angleRotationMax && xDeg > 0 && transformCam.eulerAngles.y < 180) 
				xDeg = 0;
			else if (360 - transformCam.eulerAngles.y > angleRotationMax && xDeg < 0 && transformCam.eulerAngles.y > 180) 
				xDeg = 0;
			transformCam.RotateAround(Vector3.zero, new Vector3(-yDeg, xDeg ,0), angle*(Time.deltaTime / duration));
			transformCam.eulerAngles = new Vector3(transformCam.eulerAngles.x, transformCam.eulerAngles.y, 0);
		}
	}

	void Update () {
		//updateRotateObject ();
		//if (true)
			//return;
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

