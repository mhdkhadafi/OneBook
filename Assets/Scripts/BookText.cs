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
	private string lastLeft = "vuforia1";
	private string lastRight = "bricks";
	private string currentPage = "";
	private MediaRetriever mr;
	
	// Use this for initialization
	void Start () {
		mr = GameObject.Find("Retriever").GetComponent<MediaRetriever> ();
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
		Debug.Log (currentLeftPage);
		int minLine = currentLeftPage*linesPerPage;
		ArrayList pagesTextArray = new ArrayList();
		
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			pagesTextArray = ResolveTextSize(currentArticle, 50); // TODO: limit range
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			pagesTextArray = currentBookArray.GetRange(minLine, 2*linesPerPage);
		}
		currentLeftPage += 2;
		return pagesTextArray;
		
	}
	
	public void updateCurrentPages(string text){
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			currentArticle = cleanBook(text);
		}
		//		displayPage("splinters");
		//		displayPage("rocks");
		ArrayList pagesTextArray;
		string pageText = string.Empty;
		
		string objectName = "";
		switch(currentPage){
		case "splinters":
			mr.incrementArticle();
			
			pagesTextArray = getTextForCurrentPages();
			lastRight = "splinters";
			objectName = "PageTextRight";
			pageText = string.Join("\n", (string[])pagesTextArray.GetRange(linesPerPage, linesPerPage - 1).ToArray(typeof(string)));
			updatePage(objectName, pageText);
			
			lastLeft = "rocks";
			objectName = "PageTextLeft";
			pageText = string.Join("\n", (string[])pagesTextArray.GetRange(0, linesPerPage - 1).ToArray(typeof(string)));
			updatePage(objectName, pageText);
			
			break;
			
		case "bricks":
			mr.incrementArticle();
			
			pagesTextArray = getTextForCurrentPages();
			lastRight = "bricks";
			objectName = "PageTextRight2";
			pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (linesPerPage, linesPerPage - 1).ToArray (typeof(string)));
			updatePage(objectName, pageText);
			
			lastLeft = "vuforia1";
			objectName = "PageTextLeft2";
			pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (0, linesPerPage - 1).ToArray (typeof(string)));
			updatePage(objectName, pageText);
			
			break;	
		}
		
		
		
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
		if( pageName!=lastLeft && pageName!=lastRight ){
			if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
				currentPage = pageName;
				currentArticle = mr.getCurrentArticle (updateCurrentPages);
				
			} 
			else {
				ArrayList pagesTextArray;// = getTextForCurrentPages();
				string pageText = string.Empty;
				
				string objectName = "";
				switch(pageName){
				case "tarmac_bw":
					pagesTextArray = getTextForCurrentPages();
					lastRight = "edges_bw";
					objectName = "PageTextEdges";
					pageText = string.Join("\n", (string[])pagesTextArray.GetRange(linesPerPage, linesPerPage - 1).ToArray(typeof(string)));
					updatePage(objectName, pageText);
					
					lastLeft = "tarmac_bw";
					objectName = "PageTextTarmac";
					pageText = string.Join("\n", (string[])pagesTextArray.GetRange(0, linesPerPage - 1).ToArray(typeof(string)));
					updatePage(objectName, pageText);
					
					break;
					
				case "chips_bw":
					pagesTextArray = getTextForCurrentPages();
					lastRight = "polygons_bw";
					objectName = "PageTextPolygons";
					pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (linesPerPage, linesPerPage - 1).ToArray (typeof(string)));
					updatePage(objectName, pageText);
					
					lastLeft = "chips_bw";
					objectName = "PageTextChips";
					pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (0, linesPerPage - 1).ToArray (typeof(string)));
					updatePage(objectName, pageText);
					
					break;
					
				case "rocks_bw":
					pagesTextArray = getTextForCurrentPages();
					lastRight = "bricks_bw";
					objectName = "PageTextBrick";
					pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (linesPerPage, linesPerPage - 1).ToArray (typeof(string)));
					updatePage(objectName, pageText);
					
					lastLeft = "rocks_bw";
					objectName = "PageTextRocks";
					pageText = string.Join ("\n", (string[])pagesTextArray.GetRange (0, linesPerPage - 1).ToArray (typeof(string)));
					updatePage(objectName, pageText);
					
					break;
				}
			}
		}
	}
	
	private void updatePage(string objectName, string pageText){
		TextMesh tm = GameObject.Find(objectName).GetComponent<TextMesh> ();
		tm.text = pageText;
	}
	
	public void changeBook() {
		TextMesh tm = GameObject.Find("pageText").GetComponent<TextMesh> ();
		
		mr = GetComponent<MediaRetriever> ();
		mr.incrementTitle();
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine){
			currentArticle = mr.getCurrentArticle (updateCurrentPages);
		} else if (currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBook = mr.getCurrentBook ();
			currentLeftPage = 0;
		}
		
		tm.text = mr.getCurrentTitle();
		tm.richText = true;
	}
	
	public void changeMediaType(){
		mr = GetComponent<MediaRetriever> ();
		mr.incrementMediaType();
		mr.incrementTitle();
		
		if(currentMediaType == (int)MediaRetriever.MediaTypes.Magazine)
			mr.loadMostRecentArticle(mr.currentTitleID, updateCurrentPages);
		else if(currentMediaType == (int)MediaRetriever.MediaTypes.Book) {
			currentBookArray = ResolveTextSize (mr.getCurrentBook(), 50);
			currentLeftPage = 0;
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
