using UnityEngine;
using System.Collections.Generic;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {

	TextMesh tm;
	public Material[] m_materials;

	// Use this for initialization
	void Start () {

		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		for (int i=0; i < vbs.Length; ++i) {
			vbs[i].RegisterEventHandler(this);
		}
	}
	
	// Update is called once per frame
	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb) {

		switch (vb.VirtualButtonName) {
			case "changePage":
			tm = transform.Find("PageText").GetComponent<TextMesh>();
			BookText ip = transform.Find("PageText").GetComponent<BookText>();

			ip.changeBook();
			break;
			case "changeBook":
				Texture2D p = new Texture2D(10, 10);
				Debug.Log ("OnButtonPressed called");
				
				p = Resources.Load("CatsCradleBook.Front", typeof(Texture2D)) as Texture2D;
				//		Plane plane = transform.Find ("Front").GetComponent<Plane>();
				GameObject mt = GameObject.Find("MultiTarget");
				Debug.Log(mt);
				Transform go = mt.transform.Find("Front");
				if(go!=null){
//					Material mat = go.GetComponent<Material> ();
//					mat.mainTexture = p;
					foreach(Component c in go.GetComponents<Component>()){
						Debug.Log(c);
					}
//					Material m = Resources.Load("CatsCradleBook.Front.mat", typeof(Material)) as Material;
					Renderer r = go.GetComponent<MeshRenderer>();
					r.material = m_materials[0];
//					r.material =m;
				}
				break;
		}

	
	}

	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb) {
		switch (vb.VirtualButtonName) {
		case "changePage":
			break;
		case "changeBook":
//			Texture2D p = new Texture2D(10, 10);
//			p = Resources.Load("CatsCradleBook.Back", typeof(Texture2D)) as Texture2D;
//			Transform go = transform.Find("MultiTarget").Find("Front");
//			if(go!=null){
//				Material mat = go.GetComponent<Material> ();
//				mat.mainTexture = p;
//			}			
//			Debug.Log ("OnButtonReleased called");
			break;
		}
	}
}
