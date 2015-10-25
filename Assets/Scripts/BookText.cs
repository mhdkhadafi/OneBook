using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BookText : MonoBehaviour {

	public string location;
	private string currentBook;
	private string currentArticle;
	private int currentLeftPage = 0;
	private int currentMediaType;

	// Use this for initialization
	void Start () {
		MediaRetriever mr = GameObject.Find("Retriever").GetComponent<MediaRetriever> ();
		currentMediaType = mr.getCurrentMediaType();
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			currentArticle = mr.getCurrentArticle (updateCurrentPages);
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBook = mr.getCurrentBook();
		}

	}

	void Init () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private string cleanBook(string text){
		text = Regex.Replace (text, @"<[^>]+>|&nbsp;", " ").Trim ();
		text = Regex.Replace (text, @"\s{2,}", " ");
		currentArticle = text;
		return text;
	}

	private string getTextForCurrentPages(){
		string pagesText = "";
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			pagesText = ResolveTextSize (currentArticle, 50); // TODO: limit range
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			pagesText = ResolveTextSize (currentBook, 50); // TODO: limit range
		}
		return pagesText;

	}

	public void updateCurrentPages(string text){
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			currentArticle = cleanBook(text);
		} 
		displayPage("splinters");
		displayPage("rocks");
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

	public void displayPage(string pageName) {

		string pageText = getTextForCurrentPages();

		string objectName = "";
		switch(pageName){
		case "splinters":
			objectName = "PageTextRight";
			pageText = pageText.Substring (indexOfNth (pageText, "\n", 29)+1, indexOfNth (pageText, "\n", 29));
			break;
			
		case "rocks":
			objectName = "PageTextLeft";
			pageText = pageText.Substring (0, indexOfNth (pageText, "\n", 29));
			break;
			
		}

		updatePage(objectName, pageText);
	}

	private void updatePage(string objectName, string pageText){
		TextMesh tm = GameObject.Find(objectName).GetComponent<TextMesh> ();
		tm.text = pageText;
	}
	
	public void changeBook() {
		TextMesh tm = GameObject.Find("pageText").GetComponent<TextMesh> ();
		
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		mr.incrementTitle();
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			currentArticle = mr.getCurrentArticle (updateCurrentPages);
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBook = mr.getCurrentBook ();
		}

		tm.text = mr.getCurrentTitle();
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
