using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
    const string NAME_OF_SCENE = "TestLiamControls"; //1 

#region Audio
    AudioSource audioSource;
    public AudioClip hoverOver_default;
    public AudioClip newgame_click;
    public AudioClip options_click;
    public AudioClip quit_click;
    #endregion

    public GameObject mainMenu;
    public GameObject loadingScreenImage;
    public GameObject sliderContainer;
    public Slider loadingSlider;

    public Animator imageAnimator;
    public Animator loaderBackgroundAnimator;
    public Animator loaderFillAnimator;


    const float MIN_TIME_TO_SHOW_LOADING_SCREEN = 4f;
    float loadTimer = 0f;
    bool StartedFade { get; set; }
    public bool AllowedToLoad { get; set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        /* old way LOL
//audioSource.PlayOneShot(newgame_click);
//SceneManager.LoadScene(NAME_OF_SCENE);
*/

        //initialize timer to when we pressed the button.
        loadTimer = Time.deltaTime;
        mainMenu.SetActive(false);
        StartCoroutine(LoadAsync(1));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        float progress = 0.0f;
        //play a ~3sec sound
        audioSource.PlayOneShot(newgame_click);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        loadOperation.allowSceneActivation = false;

        mainMenu.SetActive(false); //hide main menu buttons
        sliderContainer.SetActive(true); //show loading bar

        while(!loadOperation.isDone)
        {
            //Debug.Log(loadTimer);
            //have to change it because unity goes from 0 - .9
            progress = Mathf.Clamp01(loadOperation.progress / .9f);
            //Debug.Log(progress);
            loadingSlider.value = progress;

            if(loadOperation.progress >= 0.9f)//more accurate querying loadOp(0-.9)
            {
                if (!StartedFade)
                {
                    if (loadTimer > MIN_TIME_TO_SHOW_LOADING_SCREEN)
                    {
                        StartedFade = true;
                        loaderFillAnimator.SetTrigger("Fade");
                        loaderBackgroundAnimator.SetTrigger("Fade");
                        imageAnimator.SetTrigger("Fade");

                        StartCoroutine(WaitForAllowedToLoad(loadOperation));
                        break;
                    }
                }
            }
            loadTimer += Time.deltaTime;
         yield return null; // wait until next frame before continuing
        }
    }

    IEnumerator WaitForAllowedToLoad(AsyncOperation loadOperation)
    {
        yield return new WaitUntil(() => AllowedToLoad);
        sliderContainer.SetActive(false);
        loadOperation.allowSceneActivation = true;
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
