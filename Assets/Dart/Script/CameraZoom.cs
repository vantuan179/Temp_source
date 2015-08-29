using UnityEngine;
using System.Collections;
public class CameraZoom
{
	public Animation pressAnimation;
	
	void Awake()
	{

	}
	void OnPress(bool isDown)
	{
		if (!isDown)
		{
			//StartCoroutine("ButtonClick");
		}
	}
	
	IEnumerator ButtonClick()
	{
		pressAnimation.PlayQueued("Button");
		yield return new WaitForSeconds(0.3f);
		//gameObject.transform.localScale = new Vector3(1, 1, 1);
	}
}
