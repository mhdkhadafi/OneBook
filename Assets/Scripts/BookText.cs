using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BookText : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void displayBook() {
		TextMesh tm = GetComponent<TextMesh> ();
		
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		
		string txt = mr.getCurrentPage ();
		Debug.Log ("article::" + txt);
		//		TextAsset mytxtData=(TextAsset)Resources.Load("The Wolf and the Lamb");
		//		string txt = mytxtData.text;
		string noHTML = Regex.Replace(txt, @"<[^>]+>|&nbsp;", " ").Trim();
		string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");
		tm.text = ResolveTextSize(noHTML, 50);
		tm.richText = true;
	}
	
	public void changeBook() {
		TextMesh tm = GetComponent<TextMesh> ();
		
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		mr.incrementPage ();
		string txt = mr.getCurrentPage ();
		//		Debug.Log (txt);
		//		TextAsset mytxtData=(TextAsset)Resources.Load("The Wolf and the Lamb");
		//		string txt = mytxtData.text;
		string noHTML = Regex.Replace(txt, @"<[^>]+>|&nbsp;", " ").Trim();
		string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");
		tm.text = ResolveTextSize(noHTML, 50);
		tm.richText = true;
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
