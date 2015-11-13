using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MediaRetriever : MonoBehaviour {

	private string[][] titles;
	private int currentMediaType;
	private string[] mediaTypes;
	public int currentTitleID;
	private int currentArticleID = 0;
	private int currentPage = 1;
	private string currentBookText;
	private string currentArticleText;
	private string apiKey = "";
	private WWW currentArticle;
	private bool currentArticleLoading = false;
	private bool currentBookLoaded = false;
	private bool currentArticleLoaded = false;

	
	public enum MediaTypes {Magazine, Book};

	// Use this for initialization
	void Start () {
		Debug.Log("Starting Media Retriever");
		TextAsset mytxtData=(TextAsset)Resources.Load("hearstSecret");
		apiKey = mytxtData.text;
		populateTitles();
		currentTitleID = 0;
		currentMediaType = (int)MediaTypes.Book;

//		if(currentMediaType == (int)MediaTypes.Magazine)
//			loadMostRecentArticle(currentTitleID, null);
//		else if(currentMediaType == (int)MediaTypes.Book)
//			loadBook(currentTitleID);
	}

	// Update is called once per frame
	void Update () {
	}

	private void populateTitles(){
		mediaTypes = new string[]{"magazines", "books"};
		string[] magazines = new string[]{"cosmopolitan", 
		                   "countryliving",
		                   "delish",
		                   "elle",
		                   "elledecor",
		                   "esquire",
		                   "goodhousekeeping",
		                   "harpersbizarre",
		                   "housebeautiful",
		                   "marieclaire",
		                   "popularmechanics",
		                   "redbookmag",
		                   "roadandtrack",
		                   "seventeen",
		                   "townandcountrymag",
		                   "veranda",
		                   "womansday"
							};

		string[] books = new string[]{"Ulysses",
										"threelittlepigs",
										"alice",
										"Frankenstein"};
		
		titles = new string[][]{magazines, books};
	}

	// PUBLIC INTERFACE //

	// Media Type Section
	public void incrementMediaType() {
		currentMediaType = (currentMediaType + 1)%titles.Length;
		currentTitleID = 0;
		if(currentMediaType == (int)MediaTypes.Magazine)
			loadMostRecentArticle(currentTitleID, null);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}
	
	public void decrementMediaType() {
		currentMediaType = (currentMediaType - 1)%titles.Length;
		currentTitleID = 0;
		if(currentMediaType == (int)MediaTypes.Magazine)
			loadMostRecentArticle(currentTitleID, null);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public int getCurrentMediaType(){
		return currentMediaType;
	}

	// Book / Magazine Title Section
	public void incrementTitle() {
		currentArticleLoaded = false;
		currentTitleID = (currentTitleID + 1)%titles[currentMediaType].Length;
		if(currentMediaType == (int)MediaTypes.Magazine)
			loadMostRecentArticle(currentTitleID, null);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public void decrementTitle() {
		currentArticleLoaded = false;
		currentTitleID = (currentTitleID - 1)%titles[currentMediaType].Length;
		if(currentMediaType == (int)MediaTypes.Magazine)
			loadMostRecentArticle(currentTitleID, null);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public string getCurrentTitle() {
		return titles[currentMediaType][currentTitleID];
	}

	// Page / Article Section
	
	public void incrementArticle(){
		if(currentArticleID > 0) currentArticleID--;
	}
	
	public void decrementArticle(){
		currentArticleID++;
	}

	public string getCurrentArticle(System.Action<string> onComplete){
		if(!currentArticleLoading)
			loadArticle(currentArticleID, currentTitleID, onComplete);

		return currentArticleText;
	}

	public string getCurrentBook(){
		if(!currentBookLoaded)
			loadBook(currentTitleID);
		return currentBookText;
	}

	public void loadMostRecentArticle(int titleID, System.Action<string> onComplete){
		string title = titles[currentMediaType][titleID];
		string url = "https://" + title + ".hearst.io/api/v1/articles?visibility=1&all_images=0&get_image_cuts=0&ignore_cache=0&limit=1&order_by=date+desc&_key=" + apiKey;
		
		currentArticle = GET (url, extractArticle, onComplete);
	}

	// PRIVATE FUNCTIONS //

	private void loadArticle(int articleID, int titleID, System.Action<string> onComplete){
		string title = titles[currentMediaType][titleID];
		string url = "https://" + title + ".hearst.io/api/v1/articles?id=" + articleID.ToString() + "&all_images=0&get_image_cuts=0&ignore_cache=0&_key=" + apiKey;
			
		currentArticleLoading = true;

		currentArticle = GET (url, extractArticle, onComplete);

	}

	private void extractArticle(System.Action<string> onComplete){
		Debug.Log("article received");
		var js = JSON.Parse(currentArticle.text);
		var item = js["items"][0];
		var id = item["id"];
		if(int.TryParse(id, out currentArticleID)){
			currentArticleLoading = false;
			currentArticleLoaded = true;
			currentArticleText = item["body"];
			if(onComplete != null)
				onComplete(currentArticleText);
			Debug.Log(currentArticleText);
		} else {
			Debug.LogError("Failed to Parse Article ID");
		}
	}


	private void loadBook(int titleID){
		Debug.Log("Loading Book : " + titles[currentMediaType][titleID].ToString());
		TextAsset bookText = Resources.Load(titles[currentMediaType][titleID], typeof(TextAsset)) as TextAsset;
		currentBookText = bookText.text;
		Debug.Log(currentBookText);
		currentTitleID = titleID;
	}
	

//	void OnGUI(){
//		GUIStyle style = new GUIStyle();
//		style.richText = true;
//		GUILayout.Label(currentArticleText);
//	}
	
	private WWW GET(string url, System.Action<System.Action<string>> localComplete, System.Action<string> onComplete ) {
		
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www, localComplete, onComplete));
		return www;
	}
	
	private IEnumerator WaitForRequest(WWW www, System.Action<System.Action<string>> localComplete, System.Action<string> onComplete) {
		yield return www;
		// check for errors
		if (www.error == null) {
			currentArticle = www;
			localComplete(onComplete);
		} else {
			Debug.Log (www.error);
		}
	}
}
