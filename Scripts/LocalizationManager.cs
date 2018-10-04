using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

	// Static singleton property
    public static LocalizationManager Instance { get; private set; }

    // Variable to store the localization json depending on language
    private string localizationDataFileName;

	// Dictionary to store <key, value> pairs of localization
    private Dictionary<string, string> localizedText;

    LANGUAGES currentLang;

    public enum LANGUAGES
    {
        EN,
        ES
    }

#region String Keys
    public const string LOC_TEST_TITLE = "LOC_TITLE";
    public const string LOC_TEST_MESSAGE = "LOC_MSG";
    public const string LOC_TEST_DESCRIPTION = "LOC_DESC";
#endregion

	private void Start()
    {
    	// Singleton initialization. If the instance already exists it should self-destruct to avoid duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("An instance of LocalizationManager already exists");
        }
        else
        {
        	// Initialize singleton instance
            Instance = this;

            // Obtain current language from PlayerPrefs. Local method for presentation purposes only
            currentLang = (LANGUAGES) loadLanguage();

            //Initialize dictionary
            localizedText = new Dictionary<string, string>();

            // Load dictionary contents with correct JSON
            setLanguage((int)currentLang);            

            //Allow GameObject to exist through all execution
            DontDestroyOnLoad(gameObject);
        }
    }

    // Get language from Player Preferences
	private int loadLanguage()
    {
        return PlayerPrefs.GetInt("LANGUAGE_KEY", (int)LocalizationManager.LANGUAGES.EN);
    }

    // Store language on Player Preferences
    private void saveLanguage(int lang)
    {
		PlayerPrefs.SetInt("LANGUAGE_KEY", lang);
    }

	/**
	 * GUI Callback to set current language.
	 *
	 * @param lang - A value determined by the enum above.
	 * @return void.
	 */
    public void setLanguage(int lang)
    {
    	// Set language in PlayerPreferences
        saveLanguage(lang);

        // Set current language to <lang>
        currentLang = (LANGUAGES)lang;

        // Clear dictionary to re-load correct localization
        localizedText.Clear();

        // Store correct JSON localization file
        switch (currentLang)
        {
            case LANGUAGES.EN:
                localizationDataFileName = "Localization_EN.json";
                break;
            case LANGUAGES.ES:
                localizationDataFileName = "Localization_ES.json";
                break;
        }

        // Append complete path string
        string filePath = Path.Combine(Application.streamingAssetsPath, localizationDataFileName);

        string dataAsJson = "";
		
		/** We use WWW here because on Android, the files are contained within a compressed .jar file 
		 * (which is essentially the same format as standard zip-compressed files). This means that if 
		 * you do not use Unity’s WWW class to retrieve the file, you need to use additional software 
		 * to see inside the .jar archive and obtain the file. 
		 * More Info: https://docs.unity3d.com/Manual/StreamingAssets.html
		 */
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(filePath);
            while (!reader.isDone) { }

            dataAsJson = reader.text;
        }
        else
        {
        	// This is for test purposes on the Editor
            dataAsJson = File.ReadAllText(filePath);
        }

        // Load JSON data to data pairs we can insert into our dictionary
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        // Populate dictionary
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
    }

	/**
	 * Gets the localized text paired to the key received.
	 *
	 * @param textKey - The key value to search in dictionary (ie: "LOC_TEST_01").
	 * @return string containing value paired to key.
	 */
    public string getText(string textKey)
    {
        return localizedText[textKey];
    }

    [System.Serializable]
	public class LocalizationData
	{
	    public LocalizationItem[] items;
	}

	[System.Serializable]
	public class LocalizationItem
	{
	    public string key;
	    public string value;
	}
}
