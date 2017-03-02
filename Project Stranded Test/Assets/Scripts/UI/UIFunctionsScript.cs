using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIFunctionsScript : MonoBehaviour {

    string newTextObjectTextString;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Abruptly changes scene - should be fine as placeholder transitioning
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Separate function for setting the string since Unity UI can't take multiple inputs
    public void ChangeTextObjectSetString(string newText)
    {
        int randomNumber = Random.Range(0, 10000);

        newTextObjectTextString = newText + randomNumber.ToString();
    }

    // Changes the text object's text to our string
    public void ChangeTextObjectSetText(GameObject textObject)
    {
        textObject.gameObject.GetComponent<Text>().text = newTextObjectTextString;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}
