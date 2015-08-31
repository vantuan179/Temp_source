using UnityEngine;
using System.Collections;

public class Button_even : MonoBehaviour {
	public GameObject bt_camera, bt_eye, bt_foot, bt_infor, bt_eye1, bt_foot1, bt_foot2, bt_foot3, checkout;
	DragObjects objDrag;
	void Start () {
		objDrag = GameObject.Find ("ObjectBoard").GetComponent<DragObjects>();	
	}
	void Update () {
		
	}
	public void OnCamera()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		objDrag.cameraMode = CameraMode.ZoomBoard;
	}
	public void OnEye()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		objDrag.isMoveCameraReview = true;
		objDrag.goingReviewForward = !objDrag.goingReviewForward;
		objDrag.cameraMode = CameraMode.ReviewBoard;
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(true);
		bt_foot2.SetActive(false);
		bt_foot3.SetActive(false);
	}
	public void OnFoot()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(false);
		bt_foot2.SetActive(false);
		bt_foot3.SetActive(true);
		objDrag.cameraMode = CameraMode.MoveBoard;
	}
	public void OnInfor(int level)
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		Application.LoadLevel(level);
	}
	public void OnEye1()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		if(objDrag.cameraMode != CameraMode.MoveBoard) {
			objDrag.isMoveCameraReview = true;
			objDrag.goingReviewForward = !objDrag.goingReviewForward;
			objDrag.cameraMode = CameraMode.ReviewBoard;
		} else {
			objDrag.cameraMode = CameraMode.ReviewDarts;
		}
		objDrag.isDragCameraReview = false;
		bt_camera.SetActive(true);
		bt_eye.SetActive(true);
		bt_eye1.SetActive(false);
		bt_foot.SetActive(true);
		bt_foot1.SetActive(false);
		bt_foot2.SetActive(false);
		bt_foot3.SetActive(false);
	}
	public void OnFoot1()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		objDrag.isDragCameraReview = !objDrag.isDragCameraReview;
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive (false);
		bt_foot2.SetActive (true);
		bt_foot3.SetActive(false);
	}
	public void OnFoot2()
	{
		if(objDrag._mouseState) {
			objDrag.Target.SetActive(false);
			objDrag._mouseState = false;
		}
		objDrag.isDragCameraReview = !objDrag.isDragCameraReview;
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive (true);
		bt_foot2.SetActive (false);
		bt_foot3.SetActive(false);
	}
	public void OnFoot3()
	{
		objDrag.isResetMoveCamera = true;
	}

	public void Onplayagain()
	{
		checkout.SetActive (false);
		Time.timeScale = 1;	
	}

	public void notice()
	{
		checkout.SetActive (true);
		Time.timeScale = 0;
	}

}
