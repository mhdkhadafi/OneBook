using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BookText : MonoBehaviour {

	public string location;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int indexOfNth(string str, string value, int nth = 1) {
		int offset = str.IndexOf(value);
		for (int i = 1; i < nth; i++)
		{
			if (offset == -1) return -1;
			offset = str.IndexOf(value, offset + 1);
		}
		return offset;
	}

	public void displayBook() {
		TextMesh tm = GetComponent<TextMesh> ();
		
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		
		string txt = mr.getCurrentPage ();
		string noHTML = Regex.Replace (txt, @"<[^>]+>|&nbsp;", " ").Trim ();
		string noHTMLNormalised = Regex.Replace (noHTML, @"\s{2,}", " ");
		string lineBrokenText = ResolveTextSize (noHTMLNormalised, 50);

		if (location == "left") {
			//		Debug.Log ("article::" + txt);
			//		TextAsset mytxtData=(TextAsset)Resources.Load("The Wolf and the Lamb");
			//		string txt = mytxtData.text;

			string cutTextLeft = lineBrokenText.Substring (0, indexOfNth (lineBrokenText, "\n", 29));
			tm.text = cutTextLeft;
		} else {
			string cutTextRight = lineBrokenText.Substring (indexOfNth (lineBrokenText, "\n", 29)+1, indexOfNth (lineBrokenText, "\n", 29));
			tm.text = cutTextRight;
		}
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
