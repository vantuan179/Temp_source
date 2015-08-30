using UnityEngine;
using System.Collections;

public class Button_even : MonoBehaviour {
	public GameObject bt_camera, bt_eye, bt_foot, bt_infor, bt_eye1, bt_foot1 ;
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
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(true);
		
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
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(true);

		if (objDrag.cameraMode == CameraMode.ReviewDarts) {
			objDrag.cameraMode = CameraMode.MoveBoard;
		} else {
			objDrag.cameraMode = CameraMode.ReviewDarts;
		}
	}
	public void OnInfor()
	{
		
	}
	public void OnEye1()
	{
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
