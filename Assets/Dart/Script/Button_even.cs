using UnityEngine;
using System.Collections;

public class Button_even : MonoBehaviour {
	public GameObject bt_camera, bt_eye, bt_foot, bt_infor ;
	DragObjects objDrag;
	void Start () {
		objDrag = GameObject.Find ("ObjectBoard").GetComponent<DragObjects>();	
	}
	public void OnCamera()
	{
		objDrag.cameraMode = CameraMode.ZoomBoard;
	}
	public void OnEye()
	{
		SplineWalker.reviewCamera = true;
		if (objDrag.cameraMode == CameraMode.ReviewDarts) {
			objDrag.cameraMode = CameraMode.ReviewBoard;
		} else {
			SplineWalker.timeDelayReview = 0f;
			objDrag.cameraMode = CameraMode.ReviewDarts;
		}
	}
	public void OnFoot()
	{
		if (objDrag.cameraMode == CameraMode.ReviewDarts) {
			objDrag.cameraMode = CameraMode.MoveBoard;
		} else {
			objDrag.cameraMode = CameraMode.ReviewDarts;
		}
	}
	public void OnInfor()
	{
		
	}
}
