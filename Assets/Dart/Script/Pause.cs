using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public GameObject pauseButton, pausePanel;

	public void Start()
	{
		OnUnPause ();
	}

	public void OnPause()
	{
		SplineWalker.moveCameraToReview(Vector3.zero);
		//pausePanel.SetActive (true);
		//pauseButton.SetActive (false);
		//Time.timeScale = 0;
	}

	public void OnUnPause()
	{
		pausePanel.SetActive(false);
		//pauseButton.SetActive(true);
		Time.timeScale = 1;
	}
}
