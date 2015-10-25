using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BookText : MonoBehaviour {

	public string location;
	private string currentBook;
	private ArrayList currentBookArray;
	private string currentArticle;
	private int currentLeftPage = 0;
	private int linesPerPage = 30;
	private int charPerLine = 50;
	private int currentMediaType;

	// Use this for initialization
	void Start () {
		MediaRetriever mr = GameObject.Find("Retriever").GetComponent<MediaRetriever> ();
		currentMediaType = mr.getCurrentMediaType();

		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine)
			mr.loadMostRecentArticle(mr.currentTitleID, updateCurrentPages);
		else if(currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBookArray = ResolveTextSize (mr.getCurrentBook(), 50);
		}

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

	private ArrayList getTextForCurrentPages(){
		int minLine = currentLeftPage*linesPerPage;
		ArrayList pagesTextArray = new ArrayList();

		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			pagesTextArray = ResolveTextSize(currentArticle, 50); // TODO: limit range
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			pagesTextArray = currentBookArray.GetRange(minLine, 2*linesPerPage);
		}
		return pagesTextArray;

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
//		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
//			currentArticle = mr.getCurrentArticle (updateCurrentPages);
//		} 

		ArrayList pagesTextArray = getTextForCurrentPages();
		string pageText = string.Empty;

		string objectName = "";
		switch(pageName){
		case "splinters":
			objectName = "PageTextRight";
			pageText = string.Join("\n", (string[])pagesTextArray.GetRange(linesPerPage, linesPerPage - 1).ToArray(typeof(string)));
			break;
			
		case "rocks":
			objectName = "PageTextLeft";
			pageText = string.Join("\n", (string[])pagesTextArray.GetRange(0, linesPerPage - 1).ToArray(typeof(string)));

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

	public void changeMediaType(){
		MediaRetriever mr = GetComponent<MediaRetriever> ();
		mr.incrementMediaType();
		mr.incrementTitle();

		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine)
			mr.loadMostRecentArticle(mr.currentTitleID, updateCurrentPages);
		else if(currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBookArray = ResolveTextSize (mr.getCurrentBook(), 50);
		}
	}

	private ArrayList ResolveTextSize(string input, int lineLength){
		
		// Split string by char " "  
		input = input.Replace("\n", " \n ");
		string[] words = input.Split(" "[0]);
		
		// Prepare result
		ArrayList result = new ArrayList();

		// Temp line string
		string line = "";
		
		// for each all words        
		foreach(string s in words){
			if (s.Equals("\n")){
				result.Add(line);
				line = "";
			}
			else {
				// Append current word into line
				string temp = line + " " + s;
				
				// If line length is bigger than lineLength
				 if(temp.Length > lineLength){
					
					// Append current line into result
					result.Add(line);
					// Remain word append into new line
					line = s;
				}
				// Append current word into current line
				else {
					line = temp;
				}
			}

		}
		
		// Append last line into result        
		result.Add(line);
		
		return result;
	}
}
