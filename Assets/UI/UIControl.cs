using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    public GameObject Bar;
    public GameObject PillowName;
    public GameObject PillowState;

    public string NameText = "I AM A PILLOW";

    private float yVal;
    private float valChange = 0.01f;

	// Use this for initialization
	void Start () {
        PillowName.GetComponent<Text>().text = NameText;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PillowName.GetComponent<Text>().text = NameText;
        }
	}

    public void SetLoveBar(float value)
    {
        Bar.transform.localScale = new Vector3(1, value, 1);
    }

    public void SetStateText(string form)
    {
        PillowState.GetComponent<Text>().text = form;
    }
}
