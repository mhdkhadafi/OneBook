using UnityEngine;
using System.Collections.Generic;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {

	TextMesh tm;

	// Use this for initialization
	void Start () {
		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		for (int i=0; i < vbs.Length; ++i) {
			vbs[i].RegisterEventHandler(this);
		}
	}
	
	// Update is called once per frame
	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb) {
		tm = transform.Find("PageText").GetComponent<TextMesh>();
		BookText ip = transform.Find("PageText").GetComponent<BookText>();
		switch (vb.VirtualButtonName) {
			case "changePage":
			ip.changeBook();
			break;
		}

	
	}

	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb) {
	}
}
