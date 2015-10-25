using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class TextGetter : MonoBehaviour {

	private string fullText;

	void Start () {
		
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		
		string txt = "random";//mr.getCurrentArticle ();
		Debug.Log ("article::" + txt);
		//		TextAsset mytxtData=(TextAsset)Resources.Load("The Wolf and the Lamb");
		//		string txt = mytxtData.text;
		string noHTML = Regex.Replace(txt, @"<[^>]+>|&nbsp;", " ").Trim();
		string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");

		fullText = noHTMLNormalised;

		getLeftPage ();
	
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

	public void getLeftPage() {
		string leftPageText = fullText.Substring(0, indexOfNth(fullText, "\n", 20));

		Debug.Log (leftPageText);

	}
}
