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
		objDrag.isMoveCameraReview = true;
		objDrag.goingReviewForward = !objDrag.goingReviewForward;
		objDrag.cameraMode = CameraMode.ReviewBoard;

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
