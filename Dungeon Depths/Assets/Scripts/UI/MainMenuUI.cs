using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}