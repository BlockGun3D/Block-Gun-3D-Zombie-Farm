using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentBehaviour : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject); // UnityEngine.GameObject.DontDestroyOnLoad(base.gameObject);
	}

    public void Update()
    {
        // apparently doesnt work but just to be sure im keeping it
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Screen.lockCursor = !Screen.lockCursor;
        }
    }
}
