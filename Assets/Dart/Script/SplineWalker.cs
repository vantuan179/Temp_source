using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress;
	private bool goingForward = true;
	public bool isThrowDart = false;
	public float[] listRadius;
	private GameObject objectText;
	private float reduceScale;
	bool lookCamera = false;
	Vector3 targetReview;
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
		reduceScale = (transform.localScale.x - 0.7f)/8;
		//Debug.Log(reduceScale + "CurveCount ------------------------------ "+spline.CurveCount);
	}
	private void Update () {
		if (isThrowDart || lookCamera) {
				if (goingForward) {
						progress += Time.deltaTime / duration;
						if (progress > 1f) {
								if (mode == SplineWalkerMode.Once) {
										progress = 1f;
										if(!gameObject.name.Equals("Main Camera")){
											isThrowDart = false;
											reSetSplineWalker();
										} else {
											
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
								progress = -progress;
								goingForward = true;
						}
				}
				Vector3 position = spline.GetPoint (progress);
				transform.localPosition = position;
				if(isThrowDart) {
					transform.localScale = transform.localScale - new Vector3(reduceScale, reduceScale, reduceScale);
					if (lookForward) {
						transform.LookAt (position + spline.GetDirection (progress));
					}
				} else if(lookCamera){
					transform.LookAt(targetReview);
				}

			}
	}

	public void reSetSplineWalker(){

		progress -= 1f;
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0);
		//mode 501
		s_currentScore += getScores ();
		if (is2xScore () && s_remainScore == s_currentScore) {
			objectText.GetComponent<TextMesh> ().text = "WIN GAME";
			return;
		}
		if (s_remainScore - s_currentScore > 1) {
			if (s_count == 3) {
				s_remainScore -= s_currentScore;
				s_currentScore = 0;
				s_count = 0;
			}
			s_count++;
		} else {
			objectText.GetComponent<TextMesh> ().text = "BUG";
			s_currentScore = 0;
			s_count = 0;
			return;
		}
		objectText.GetComponent<TextMesh> ().text = "Scores : " + s_remainScore;

	}	

	public void moveCameraToReview(Vector3 target)
	{
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