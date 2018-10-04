using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour {

    [Header("Labels for localization")]
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtMessage;
    [SerializeField] TextMeshProUGUI txtDecription;

    // Use this for initialization
    void Start ()
    {
        setLocalization();
    }

    //Set localized text values to corresponding labels in GUI
    private void setLocalization()
    {
        txtTitle.SetText(LocalizationManager.Instance.getText(LocalizationManager.LOC_TEST_TITLE));
        txtMessage.SetText(LocalizationManager.Instance.getText(LocalizationManager.LOC_TEST_MESSAGE));
        txtDecription.SetText(LocalizationManager.Instance.getText(LocalizationManager.LOC_TEST_DESCRIPTION));
    }

	public void GUI_ChangeLanguage(int language)
	{
		LocalizationManager.Instance.setLanguage(language);
		LevelLoader.Instance.reloadCurrentScene();
	}
}
