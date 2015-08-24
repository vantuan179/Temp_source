using UnityEngine;
using System.Collections;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress = 0f;
	private bool goingForward = true;
	public bool isThrowDart = false;
	public float[] listRadius;
	private GameObject objectText;
	private float reduceScale = 0f;
	private float reduceSizeCame = 0f;
	private Vector3 reduceLookCame = Vector3.zero;
	bool lookCamera = false;
	bool reviewCamera = false;
	Vector3 targetReview;
	float valueScale = 0.5f;
	float valueSizeCame = 2f;
	float oldOrthoSize;
	Vector3 oldOrthoPostion;
	Vector3 oldOrthoScale;
	Vector3 currentLookAt;
	Vector3 v2 = Vector3.zero, v3 = Vector3.zero;

	//score
	public int s_currentScore = 0;
	public int s_remainScore = 501;
	public int s_count = 0;
	//private CameraSwitch jsScript;  
	void Start () {
		//init score
		s_currentScore = 0;
		s_remainScore = 501;

		objectText = GameObject.Find("ObjectText");
		oldOrthoScale = transform.localScale;
		if (gameObject.name.Equals ("Main Camera")) {
			oldOrthoSize = camera.orthographicSize;
			oldOrthoPostion = transform.position;
		}
	}
	private void Update () {
		if (isThrowDart || lookCamera) {
			float daltatime = Time.deltaTime;
			float n = (goingForward?1:-1) * (duration/daltatime);
			if(progress == 0f) {
				reduceScale = (oldOrthoScale.x - valueScale) / n;
			}
			if(lookCamera){
				if(progress == 0f) {
					if(v2 == Vector3.zero) {
						v3 = spline.GetControlPoint(3);
						v2 = spline.GetControlPoint(2);
						spline.SetControlPoint(3, new Vector3(v3.x - targetReview.x, v3.y + targetReview.y, v3.z));
						spline.SetControlPoint(2, v2);
						currentLookAt = new Vector3(oldOrthoPostion.x, oldOrthoPostion.y, targetReview.z);
					}
					reduceSizeCame = (oldOrthoSize - valueSizeCame) / n;
				}
				//reduceLookCame = (targetReview - oldOrthoPostion) / n;

			}

			if (goingForward) {
				progress += daltatime / duration;
				if (progress > 1f) {
					if (mode == SplineWalkerMode.Once) {
						progress = 1f;
						if (!gameObject.name.Equals ("Main Camera")) {
							isThrowDart = false;
							reSetSplineWalker ();
						} else {
							lookCamera = false;
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
				progress -= daltatime / duration;
				if (progress < 0f) {
					progress = 0f;
					goingForward = true;
				}
			}
			Vector3 position = spline.GetPoint (progress);
			if (isThrowDart) {
				transform.localPosition = position;
				if(transform.localScale.x > valueScale)
					transform.localScale = transform.localScale - new Vector3 (reduceScale, reduceScale, reduceScale);
				if (lookForward) {
					transform.LookAt (position + spline.GetDirection (progress));
				}
			} else if (lookCamera) {
				camera.orthographicSize = camera.orthographicSize - reduceSizeCame;
				if(progress == 0f) {
					lookCamera = false;
					camera.orthographicSize = oldOrthoSize;
					transform.LookAt(new Vector3(0, oldOrthoPostion.y, 0));
					transform.localPosition = oldOrthoPostion;

				} else {
					transform.localPosition = position;
					currentLookAt = currentLookAt + new Vector3((targetReview.x - oldOrthoPostion.x)/n, (targetReview.y - position.y)/n, 0);
					transform.LookAt(currentLookAt);
				}
			}
		} else if (reviewCamera) {
			moveCameraToReview (transform.localPosition);
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
			if (s_count == 3) {
				reviewCamera = true;
				s_remainScore -= s_currentScore;
				s_currentScore = 0;
				s_count = 0;
			}
			s_count++;
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
		reduceScale = -1*reduceScale;
		reduceSizeCame = -1*reduceSizeCame;
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

		float d = Vector3.Distance(positionoSphere, new Vector3(0, 0, positionoSphere.z));
		for(int i = 0; i < listRadius.Length; i++){
			if(d <= listRadius[i]){
				//Debug.Log("iiiiiiiii --------------------------------------- "+i);
				switch(i){
				case 0:
					return 50;	
					break;
				case 1:
					return 25;	
					break;
				case 2:
				case 4:
					return scores;	
					break;
				case 3:
					return scores*3;	
					break;
				case 5:
					return scores*2;	
					break;
				}
				break;
			}
		}
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