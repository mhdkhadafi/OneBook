using UnityEngine;
using System.Collections;

public class TextGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {

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

	// Update is called once per frame
	void Update () {
	
	}

	public void getLeftPage() {

	}
}
