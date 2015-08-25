using UnityEngine;
using System.Collections;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;
	BezierSpline[] splines = new BezierSpline[2];
	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress = 0f;
	private bool goingForward = true;
	public bool isThrowDart = false;
	public static float[] listRadius;
	private GameObject objectText;
	private float reduceScale = 0f;
	private float reduceSizeCame = 0f;
	private Vector3 reduceLookCame = Vector3.zero;
	bool lookCamera = false;
	bool reviewCamera = false;
	Vector3 targetReview;
	float valueScale = 0.6f;
	float valueSizeCame = 3f;
	float oldOrthoSize;
	Vector3 oldPosition;
	Vector3 oldScale;
	Vector3 oldRotation;
	Vector3 currentLookAt;
	Vector3 v2 = Vector3.zero, v3 = Vector3.zero;
	bool isCameraMode;
	public bool dartOutScreen;
	//score
	public int s_currentScore = 0;
	public int s_remainScore = 501;
	public static int s_count = 0;
	private static DragObjects dragObject;
	//private CameraSwitch jsScript;  
	void Start () {
		//init score
		s_currentScore = 0;
		s_remainScore = 501;

		isCameraMode = gameObject.name.Equals ("Main Camera") ? true : false;
		objectText = GameObject.Find("ObjectText");
		oldPosition = transform.position;
		if (isCameraMode) {
			oldOrthoSize = camera.orthographicSize;
			oldRotation = transform.localEulerAngles;
			currentLookAt = new Vector3(oldPosition.x, oldPosition.y, targetReview.z);
			splines[0] = spline;
			splines[1] = GameObject.Find("SplineCamera1").GetComponent<BezierSpline>();
		} else {
			oldScale = transform.localScale;
		}
		if (dragObject == null) {
			dragObject = GameObject.Find("ObjectBoard").GetComponent<DragObjects>();		
		}
	}
	private void Update () {
		if (isThrowDart || lookCamera) {
			if (goingForward) {
				progress += Time.deltaTime / duration;
				if (progress > 1f) {
					if (mode == SplineWalkerMode.Once) {
						progress = 1f;
						if (!isCameraMode) {
							isThrowDart = false;
							transform.localScale = new Vector3 (valueScale, valueScale, valueScale);
							reSetSplineWalker ();
						} else {
							lookCamera = false;
							camera.orthographicSize = valueSizeCame;
							StartCoroutine(backCamraGoingForward());
						}	
					} else if (mode == SplineWalkerMode.Loop) {
						progress -= 1f;
					} else {
						progress = 2f - progress;
						goingForward = false;
					}
				}
			} else {
				progress -= Time.deltaTime / duration;
				if (progress < 0f) {
					progress = 0f;
					goingForward = true;
				}
			}

			Vector3 position = spline.GetPoint (progress);
			if (isThrowDart) {
				transform.localPosition = position;
				reduceScale = (goingForward?1:-1) * (oldScale.x - valueScale) * progress;
				transform.localScale = oldScale - new Vector3 (reduceScale, reduceScale, reduceScale);
				if (lookForward) {
					transform.LookAt (position + spline.GetDirection (progress));
				}
			} else if (lookCamera) {
					camera.orthographicSize = oldOrthoSize - (oldOrthoSize - valueSizeCame) * progress;
				if(progress == 0f) {
					lookCamera = false;
					camera.orthographicSize = oldOrthoSize;
					transform.localEulerAngles = oldRotation;
					transform.localPosition = oldPosition;
					dragObject.resetAllDarts();
				} else {
					transform.localPosition = position;
					currentLookAt = new Vector3(oldPosition.x, oldPosition.y, targetReview.z) + (new Vector3((targetReview.x - oldPosition.x) * progress, (targetReview.y - position.y) * progress, 0));
					transform.LookAt(currentLookAt);
				}
			}
		} else if (reviewCamera) {
			moveCameraToReview (dragObject.getInCenterOfDarts());
		}
	}

	public void reSetSplineWalker(){

		progress -= 1f;
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0);
		//mode 501
		s_currentScore += getScores ();
		if (is2xScore () && s_remainScore == s_currentScore) {
			objectText.GetComponent<TextMesh> ().text = "WIN GAME";
			reviewCamera = true;
			return;
		}
		if (s_remainScore - s_currentScore > 1) {
			++s_count;
			if (s_count == 3) {
				reviewCamera = true;
				s_remainScore -= s_currentScore;
				s_currentScore = 0;
				s_count = 0;
			}
		} else {
			objectText.GetComponent<TextMesh> ().text = "BUG";
			s_currentScore = 0;
			s_count = 0;
			reviewCamera = true;
			return;
		}
		objectText.GetComponent<TextMesh> ().text = "Scores : " + s_remainScore;

	}	
	IEnumerator backCamraGoingForward(){
		yield return new WaitForSeconds(2);

		lookCamera = true;
		goingForward = false;
	}

	public void moveCameraToReview(Vector3 target)
	{
		reviewCamera = false;
		GameObject objCame = GameObject.Find ("Main Camera");
		SplineWalker splineCame = objCame.GetComponent<SplineWalker>();
		splineCame.lookCamera = true;
		splineCame.targetReview = target;
		if (target.x < 0)
			splineCame.spline = splineCame.splines [1];
		else
			splineCame.spline = splineCame.splines [0];
	}
	int[] arrScores = {6,13,4,18,1,20,5,12,9,14,11,8,16,7,19,3,17,2,15,10};
	int getScores(){
		int scores = 0;
		Vector3 axisOx = new Vector3 (1, 0, 0);
		Vector3 positionoSphere = transform.FindChild("Dart1").FindChild("Sphere03").position;
		Vector3 v = new Vector3 (positionoSphere.x, positionoSphere.y, 0);
		float angle = Vector3.Angle (axisOx, v);
		if (positionoSphere.y < 0) {
			angle = 360 - angle;
		}
		//Debug.Log (" ----------------------angle---------------- "+angle);
		for (int i = 0; i < arrScores.Length; i++) {
			//Debug.Log(i + " ---------------------------- "+((360/arrScores.Length*(i+1) - 360/(arrScores.Length*2))));
			if(angle > 360 - 360/(arrScores.Length*2)){
				scores = arrScores[0];
			}
			else if(angle < (360/arrScores.Length*(i+1) - 360/(arrScores.Length*2))) {
				scores = arrScores[i];
				break;
			}
		}
		//if(true) return scores;
		dartOutScreen = false;
		float d = Vector3.Distance(positionoSphere, new Vector3(0, 0, positionoSphere.z));
		for(int i = 0; i < listRadius.Length; i++){
			if(d <= listRadius[i]){
				//Debug.Log("iiiiiiiii --------------------------------------- "+i);
				switch(i){
				case 0:
					return 50;	
				case 1:
					return 25;	
				case 2:
				case 4:
					return scores;	
				case 3:
					return scores*3;	
				case 5:
					return scores*2;	
				case 6:
					return 0;
				}
				break;
			}
		}
		dartOutScreen = true;
		return 0;
	}
	public bool is2xScore(){
		Vector3 positionoSphere = transform.FindChild("Dart1").FindChild("Sphere03").position;
		float d = Vector3.Distance(positionoSphere, new Vector3(0, 0, positionoSphere.z));
		for(int i = 0; i < listRadius.Length; i++){
			if(d <= listRadius[i] && i == 5){
				return true;
			}
		}
		return false;
	}
}