using UnityEngine;
using System.Collections;

public class BackMenu : MonoBehaviour {

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }
}
