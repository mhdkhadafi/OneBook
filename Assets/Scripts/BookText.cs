using UnityEngine;
using System.Collections;

public class BookText : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		//		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		//		for (int i=0; i < vbs.Length; ++i) {
		//			Debug.Log ("Button");
		//		}
		//		tm.text = "Goodbye World";
		TextAsset mytxtData=(TextAsset)Resources.Load("The Wolf and the Lamb");
		string txt = mytxtData.text;
		tm.text = ResolveTextSize(txt, 50);
		//		tm.richText = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void changeBook() {
		string[] bookName = new string[] {"The Wolf and the Lamb", "The Ass and the Grasshopper", "The Bat and the Weasels"};
		
		TextMesh tm = GetComponent<TextMesh> ();
		int randomNumber = Random.Range (0, 3);
		TextAsset mytxtData=(TextAsset)Resources.Load(bookName[randomNumber]);
		string txt = mytxtData.text;
		tm.text = ResolveTextSize(txt, 50);
	}
	
	private string ResolveTextSize(string input, int lineLength){
		
		// Split string by char " "         
		string[] words = input.Split(" "[0]);
		
		// Prepare result
		string result = "";
		
		// Temp line string
		string line = "";
		
		// for each all words        
		foreach(string s in words){
			// Append current word into line
			string temp = line + " " + s;
			
			// If line length is bigger than lineLength
			if(temp.Length > lineLength){
				
				// Append current line into result
				result += line + "\n";
				// Remain word append into new line
				line = s;
			}
			// Append current word into current line
			else {
				line = temp;
			}
		}
		
		// Append last line into result        
		result += line;
		
		// Remove first " " char
		return result.Substring(1,result.Length-1);
	}
}
