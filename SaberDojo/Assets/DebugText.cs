using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour {
    public static DebugText SharedInstance;
    public float maxDisplay = 2;
    public string text {
        get {
            return textMesh.text;
        }
        set {
            lastChange = Time.time;
            textMesh.text = value;
        }
    }

    private TextMesh textMesh;
    private float lastChange = 0;

    void Awake () {
        SharedInstance = this;
        textMesh = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        if ((Time.time - lastChange) > maxDisplay) {
            textMesh.text = "";
        }
	}
}
