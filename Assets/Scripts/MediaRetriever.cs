using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MediaRetriever : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Starting Media Retriever");
//		apiKey = System.IO.File.ReadAllText("Assets/Resources/hearstSecret.txt");
//		Debug.Log(apiKey);
		currentTitleID = 10;
		currentMediaType = 0;
		populateTitles();

		if(currentMediaType == 0)
			getMostRecentArticle(currentTitleID);
		else if(currentMediaType == 1)
			incrementPage();
	}

	// Update is called once per frame
	void Update () {
	}
	
	private string[][] titles;
	private int currentMediaType;
	private string[] mediaTypes;
	private int currentTitleID;
	private int currentArticleID = 0;
	private int currentPage = 0;
	private string currentPageText;
	private int linesPerPage = 50;
	private string apiKey = "2dyz29j4caczfam4y3bd5cpc";
	private WWW currentArticle;

	enum MediaTypes {Magazine, Book};

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
	}
	
	public void decrementMediaType() {
		currentMediaType = (currentMediaType - 1)%titles.Length;
	}

	public string getCurrentMediaType(){
		return mediaTypes[currentMediaType];
	}

	// Book / Magazine Title Section
	public void incrementTitle() {
		currentTitleID = (currentTitleID + 1)%titles[currentMediaType].Length;
	}

	public void decrementTitle() {
		currentTitleID = (currentTitleID - 1)%titles[currentMediaType].Length;
	}

	public string getCurrentTitle() {
		return titles[currentMediaType][currentTitleID];
	}

	// Page / Article Section
	
	public void incrementPage(){
		if(currentMediaType == (int)MediaTypes.Magazine){
			if(currentArticleID > 0) currentArticleID--;
			currentArticle = getArticle(currentArticleID, currentTitleID);

		} else if(currentMediaType == (int)MediaTypes.Book) {
			currentPage++;
			currentPageText = getPage(currentPage, currentTitleID);
		}
	}
	
	public void decrementPage(){
		if(currentMediaType == (int)MediaTypes.Magazine) {
			currentArticleID++;
			currentArticle = getArticle(currentArticleID, currentTitleID);

		} else if (currentMediaType == (int)MediaTypes.Book) {
			if(currentPage > 0) currentPage--;
			currentPageText = getPage(currentPage, currentTitleID);
		}
	}

	public string getCurrentPage(){
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
	
	private string getPage(int page, int titleID){
		int minLine = (page - 1)*linesPerPage;
		int maxLine = minLine + linesPerPage;
		string text = "";

		using (var reader = new System.IO.StreamReader("Assets/Books/" + titles[currentMediaType][titleID] + ".txt"))
		{
			int lineCount = 0;

			while (!reader.EndOfStream && lineCount <= maxLine)
			{
				var line = reader.ReadLine();
				if (string.IsNullOrEmpty(line))
					continue;
				
				if (lineCount > minLine)
					text += line;

				lineCount++;
			}
		}

		return text;
	}
	
//	void OnGUI(){
//		GUIStyle style = new GUIStyle();
//		style.richText = true;
//		GUILayout.Label(currentPageText);
//	}

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
