using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    const string NAME_OF_SCENE = "TestLiamControls";

    AudioSource audioSource;
    public AudioClip hoverOver_default;

    public AudioClip newgame_click;
    public AudioClip options_click;
    public AudioClip quit_click;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        audioSource.PlayOneShot(newgame_click);
        SceneManager.LoadScene(NAME_OF_SCENE);
    }

    public void Options()
    {
        audioSource.PlayOneShot(options_click);
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(quit_click);
        Application.Quit();
    }
}
