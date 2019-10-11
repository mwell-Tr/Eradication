using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the UI navigation of the main menu

public class MainMenu : MonoBehaviour {

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}