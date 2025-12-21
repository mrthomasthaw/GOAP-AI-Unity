using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDebugger : MonoBehaviour {

    public List<System.Action> gizmosDrawList = new List<System.Action>();

    public static VisualDebugger instance;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos() {
        foreach(System.Action gizmosDraw in gizmosDrawList)
        {
            gizmosDraw();
        }
    }
}
