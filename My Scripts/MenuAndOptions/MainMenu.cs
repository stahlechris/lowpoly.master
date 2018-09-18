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

    public Animator animator;
    const float MIN_TIME_TO_SHOW_LOADING_SCREEN = 10f;
    float loadTimer = 0f;
    bool StartedFade { get; set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        /* old way
//audioSource.PlayOneShot(newgame_click);
//SceneManager.LoadScene(NAME_OF_SCENE);
*/

        //initialize timer to when we pressed the button.
        loadTimer = Time.time;

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
            //have to change it because unity goes from 0 - .9
            progress = Mathf.Clamp01(loadOperation.progress / .9f);
            loadingSlider.value = progress;

            if(loadOperation.progress >= 0.9f)//more accurate querying loadOp(0-.9)
            {
                if (!StartedFade)
                {
                    if (loadTimer > MIN_TIME_TO_SHOW_LOADING_SCREEN)
                    {
                        //StartCoroutine(FadeOutToGame(loadOperation)); //is this costly to pass around?
                        animator.SetTrigger("Fade");
                        loadOperation.allowSceneActivation = true;
                        break;
                    }
                }
            }
            loadTimer += Time.time;
         yield return null; // wait until next frame before continuing
        }
    }

    //this has to go in update to work
    //IEnumerator FadeOutToGame(AsyncOperation loadOperation)
    //{
    //    StartedFade = true;

    //    animator.SetTrigger("Fade");

    //    //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    //    yield return new WaitForSeconds(5);
    //    loadingScreenImage.SetActive(false);
    //    sliderContainer.SetActive(false);

    //    loadOperation.allowSceneActivation = true;
    //    //Debug.Log(loadTimer);
    //}





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
