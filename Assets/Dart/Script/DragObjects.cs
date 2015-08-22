using UnityEngine;
using System.Collections;

public class DragObjects : MonoBehaviour {
	private bool _mouseState;
	public GameObject Target;
	private Vector3 screenSpace;
	private Vector3 oldpositionTarget;
	private Vector3 oldRotationTarget;
	private Vector3 oldRotationSpline;
	private Vector3 oldMouse;
	private Vector3 mouseSpeed;
	private Rigidbody resetRigibody;
	public int speed = 5;
	private float[] listRadius;
	private SplineWalker splineWalker;
	private Vector3 tmpV;

	private Vector3 oldPostionSpline;
	// Use this for initialization
	void Start () {
		string[] tubes = {
			"Tube01", "Tube02", "Tube03", "Tube04", "Tube05", "Tube06" 
		};
		listRadius = new float[tubes.Length];
		for (int i = 0; i < tubes.Length; i++) {
			GameObject obj = GameObject.Find(tubes[i]);
			listRadius[i] = obj.GetComponent<SphereCollider>().radius*transform.localScale.x;
		}
		splineWalker = Target.GetComponent <SplineWalker> ();
		splineWalker.listRadius = listRadius;
		tmpV = Target.transform.position - splineWalker.spline.transform.position;
		oldpositionTarget = Target.transform.position;
		oldRotationTarget = Target.transform.localEulerAngles;
		oldRotationSpline = splineWalker.spline.transform.localEulerAngles;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Target.transform.position = oldpositionTarget;
			Target.transform.localEulerAngles = oldRotationTarget;
			splineWalker.spline.transform.localEulerAngles = oldRotationSpline;
			_mouseState = true;
			Target.SetActive(true);
			oldMouse = Input.mousePosition;
			screenSpace = Camera.main.WorldToScreenPoint (Target.transform.position);
			Target.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

			/*Vector3 O = Camera.main.WorldToScreenPoint (new Vector3(0, 0, 0));
			Vector3 v = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, O.z));
			float d = Vector3.Distance(v, new Vector3(0, 0, 0));
			Debug.Log("d --------------------------------------- "+d);
			for(int i = 0; i < listRadius.Length; i++){
				if(d < listRadius[i]){
					Debug.Log("AA --------------------------------------- "+i);
					break;
				}
			}*/

		}
		if (Input.GetMouseButtonUp (0)) {
			//mouseSpeed = oldMouse - Input.mousePosition;
			//Target.transform.parent = null;
			Vector2 v1 = new Vector2(0, 1);
			Vector2 v2 = new Vector2(Input.mousePosition.x - oldMouse.x, Input.mousePosition.y - oldMouse.y);
			float angle = Vector2.Angle(v1, v2);
			//splineWalker.spline.transform.Rotate(new Vector3(0, 0, 1), angle);
			splineWalker.spline.transform.position = Target.transform.position - tmpV;
			_mouseState = false;
			splineWalker.isThrowDart = true;
		}
		if (_mouseState) {
			//keep track of the mouse position
			var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
			
			//convert the screen mouse position to world point and adjust with offset
			var curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace);
			
			//update the position of the object in the world
			Target.transform.position = curPosition;
		}
	}


}

