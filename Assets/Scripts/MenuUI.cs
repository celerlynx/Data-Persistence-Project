using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{
    
    public TMP_InputField playerNameField;
    //public TextMeshProUGUI errorMessageText;
    public TextMeshProUGUI bestScoreText;

    public void EnterPlayerName()
    {
        string name = playerNameField.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            TextMeshProUGUI placeholderText = (TextMeshProUGUI)playerNameField.placeholder;
            placeholderText.text = "Name cannot be empty!";
        }
        else 
        {
            //add player name to Data Manager
            DataManager.Instance.currentPlayerName = name;
        }   

        // Log the text to the console (you can use it for other purposes too)
        //Debug.Log("Input received: " + name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerNameField != null)
        {
            playerNameField.onEndEdit.AddListener(delegate { EnterPlayerName(); });
            playerNameField.ActivateInputField();
        }

        
        bestScoreText.text = "Best Score" + 
            (!string.IsNullOrEmpty(DataManager.Instance.bestPlayerName)? " : " + DataManager.Instance.bestPlayerName : "") + 
            " : " + DataManager.Instance.bestScore;
    }


    //Start button click
    public void StartNew()
    {
        if (!string.IsNullOrEmpty(DataManager.Instance.currentPlayerName))
        {
            DataManager.Instance.SetCurrentPlayer();
            SceneManager.LoadScene(1);
        }
    }

    //Quit button click
    public void Exit()
    {
        //MainManager.Instance.SaveData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }

    //Back to menu button click in the main scene
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
