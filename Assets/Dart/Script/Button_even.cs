using UnityEngine;
using System.Collections;

public class Button_even : MonoBehaviour {
	public GameObject bt_camera, bt_eye, bt_foot, bt_infor, bt_eye1, bt_foot1, bt_foot2, bt_foot3 ;
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

		// objDrag.isMoveCameraReview = true;
		// objDrag.goingReviewForward = !objDrag.goingReviewForward;
		// objDrag.cameraMode = CameraMode.ReviewBoard;
		
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
		bt_camera.SetActive(false);
		bt_eye.SetActive(false);
		bt_eye1.SetActive(true);
		bt_foot.SetActive(false);
		bt_foot1.SetActive(false);
		bt_foot2.SetActive(false);
		bt_foot3.SetActive(true);

		// if (objDrag.cameraMode == CameraMode.ReviewDarts) {
			// objDrag.cameraMode = CameraMode.MoveBoard;
		// } else {
			// objDrag.cameraMode = CameraMode.ReviewDarts;
		// }
	}
	public void OnInfor(int level)
	{
		Application.LoadLevel(level);
	}
	public void OnEye1()
	{
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

	}

	public void notice()
	{

	}

}
