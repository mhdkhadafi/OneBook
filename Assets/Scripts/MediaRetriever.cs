using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MediaRetriever : MonoBehaviour {

	private string[][] titles;
	private int currentMediaType;
	private string[] mediaTypes;
	private int currentTitleID;
	private int currentArticleID = 0;
	private int currentPage = 1;
	private string currentBookText;
	private string currentPageText;
	private int charsPerPage = 200;
	private string apiKey = "";
	private WWW currentArticle;
	
	enum MediaTypes {Magazine, Book};

	// Use this for initialization
	void Start () {
		Debug.Log("Starting Media Retriever");
		TextAsset mytxtData=(TextAsset)Resources.Load("hearstSecret");
		apiKey = mytxtData.text;

		currentTitleID = 0;
		currentMediaType = (int)MediaTypes.Magazine;
		populateTitles();

		if(currentMediaType == (int)MediaTypes.Magazine)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
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

		string[] books = new string[]{"Frankenstein", 
										"Ulysses"};

		titles = new string[][]{magazines, books};
	}

	// PUBLIC INTERFACE //

	// Media Type Section
	public void incrementMediaType() {
		currentMediaType = (currentMediaType + 1)%titles.Length;
		currentTitleID = 0;
		if(currentMediaType == (int)MediaTypes.Magazine)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}
	
	public void decrementMediaType() {
		currentMediaType = (currentMediaType - 1)%titles.Length;
		currentTitleID = 0;
		if(currentMediaType == (int)MediaTypes.Magazine)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public string getCurrentMediaType(){
		return mediaTypes[currentMediaType];
	}

	// Book / Magazine Title Section
	public void incrementTitle() {
		currentTitleID = (currentTitleID + 1)%titles[currentMediaType].Length;
		if(currentMediaType == (int)MediaTypes.Magazine)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public void decrementTitle() {
		currentTitleID = (currentTitleID - 1)%titles[currentMediaType].Length;
		if(currentMediaType == (int)MediaTypes.Magazine)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == (int)MediaTypes.Book)
			loadBook(currentTitleID);
	}

	public string getCurrentTitle() {
		return titles[currentMediaType][currentTitleID];
	}

	// Page / Article Section
	
	public void incrementPage(){
		if(currentMediaType == (int)MediaTypes.Magazine){
			if(currentArticleID > 0) currentArticleID--;
//			currentArticle = getArticle(currentArticleID, currentTitleID);

		} else if(currentMediaType == (int)MediaTypes.Book) {
			currentPage++;
//			currentPageText = getPage(currentPage, currentTitleID);
		}
	}
	
	public void decrementPage(){
		if(currentMediaType == (int)MediaTypes.Magazine) {
			currentArticleID++;
//			currentArticle = getArticle(currentArticleID, currentTitleID);

		} else if (currentMediaType == (int)MediaTypes.Book) {
			if(currentPage > 0) currentPage--;
//			currentPageText = getPage(currentPage, currentTitleID);
		}
	}

	public string getCurrentPage(){
		if(currentMediaType == (int)MediaTypes.Magazine) {
			currentArticle = getArticle(currentArticleID, currentTitleID);
			
		} else if (currentMediaType == (int)MediaTypes.Book) {
			currentPageText = getPage(currentPage);
		}	
		return currentPageText;
	}


	// PRIVATE FUNCTIONS //

	private WWW getArticle(int articleID, int titleID){
		string title = titles[currentMediaType][titleID];
		string url = "https://" + title + ".hearst.io/api/v1/articles?id=" + articleID.ToString() + "&all_images=0&get_image_cuts=0&ignore_cache=0&_key=" + apiKey;
		return GET (url, generateTexture);
	}

	private void getMostRecentArticle(int titleID){
		string title = titles[currentMediaType][titleID];
		string url = "https://" + title + ".hearst.io/api/v1/articles?visibility=1&all_images=0&get_image_cuts=0&ignore_cache=0&limit=1&order_by=date+desc&_key=" + apiKey;
		currentArticle = GET (url, generateTexture);
	}

	private void generateTexture(){
		Debug.Log("article received");
		var js = JSON.Parse(currentArticle.text);
		var item = js["items"][0];
		if(int.TryParse(item["id"], out currentArticleID)){
			currentPageText = item["body"];
			Debug.Log(currentPageText);
		} else {
			Debug.LogError("Failed to Parse Article ID");
		}
	}
	
	private string getPage(int page){
		int minChar = Mathf.Max(0, page*charsPerPage);
		int maxChar = Mathf.Min(minChar + charsPerPage, currentBookText.Length - 1);

		return currentBookText.Substring(minChar, maxChar);
	}

	private void loadBook(int titleID){
		TextAsset bookText =(TextAsset)Resources.Load(titles[currentMediaType][titleID]);
		currentBookText = bookText.text;
		currentTitleID = titleID;
		currentPage = 1;
		currentPageText = getPage(currentPage);
	}
	
	void OnGUI(){
		GUIStyle style = new GUIStyle();
		style.richText = true;
		GUILayout.Label(currentPageText);
	}

	private WWW GET(string url, System.Action onComplete ) {
		
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www, onComplete));
		return www;
	}
	
//	private WWW POST(string url, Dictionary<string,string> post, System.Action onComplete) {
//		WWWForm form = new WWWForm();
//		
//		foreach(KeyValuePair<string,string> post_arg in post) {
//			form.AddField(post_arg.Key, post_arg.Value);
//		}
//		
//		WWW www = new WWW(url, form);
//		
//		StartCoroutine(WaitForRequest(www, onComplete));
//		return www;
//	}
	
	private IEnumerator WaitForRequest(WWW www, System.Action onComplete) {
		yield return www;
		// check for errors
		if (www.error == null) {
			onComplete();
		} else {
			Debug.Log (www.error);
		}
	}
}
