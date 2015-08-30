using UnityEngine;
using System.Collections;

public class Button_even : MonoBehaviour {
	public GameObject bt_camera, bt_eye, bt_foot, bt_infor, bt_eye1, bt_foot1 ;
	DragObjects objDrag;
	void Start () {
		objDrag = GameObject.Find ("ObjectBoard").GetComponent<DragObjects>();	
	}
	void Update () {
		
	}
	public void OnCamera()
	{
		objDrag.cameraMode = CameraMode.ZoomBoard;
		objDrag._mouseState = false;
	}
	public void OnEye()
	{
		objDrag._mouseState = false;
		objDrag.isMoveCameraReview = true;
		objDrag.goingReviewForward = !objDrag.goingReviewForward;
		objDrag.cameraMode = CameraMode.ReviewBoard;


		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(true);
	}
	public void OnFoot()
	{
		objDrag._mouseState = false;
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(true);

		// if (objDrag.cameraMode == CameraMode.ReviewDarts) {
			// objDrag.cameraMode = CameraMode.MoveBoard;
		// } else {
			// objDrag.cameraMode = CameraMode.ReviewDarts;
		// }
	}
	public void OnInfor()
	{
		
	}
	public void OnEye1()
	{
		objDrag._mouseState = false;
		objDrag.isMoveCameraReview = true;
		objDrag.goingReviewForward = !objDrag.goingReviewForward;
		objDrag.cameraMode = CameraMode.ReviewBoard;
		bt_camera.SetActive(true);
		bt_eye.SetActive(true);
		bt_eye1.SetActive(false);
		bt_foot.SetActive(true);
		bt_foot1.SetActive(false);
	}
	public void OnFoot1()
	{

	}

}
