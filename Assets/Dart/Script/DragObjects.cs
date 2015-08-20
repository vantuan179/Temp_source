using UnityEngine;
using System.Collections;

public class DragObjects : MonoBehaviour {
	private bool _mouseState;
	public GameObject Target;
	private Vector3 screenSpace;
	private Vector3 oldMouse;
	private Vector3 mouseSpeed;
	public int speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(_mouseState);
		if (Input.GetMouseButtonDown (0)) {
			_mouseState = true;
			Target.SetActive(true);
			oldMouse = Input.mousePosition;
			screenSpace = Camera.main.WorldToScreenPoint (Target.transform.position);
			Target.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
		}
		if (Input.GetMouseButtonUp (0)) {
			mouseSpeed = oldMouse - Input.mousePosition;
			Target.rigidbody.useGravity = true;
			Target.rigidbody.isKinematic = false;
			Target.transform.parent = null;
			Target.rigidbody.AddForce(transform.forward*1000);
			//Target.SetActive(false);
			_mouseState = false;
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

