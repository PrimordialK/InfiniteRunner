using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public AudioClip pauseSound;
    private AudioSource audioSource;


    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;

    public Button resumeGame;
    public Button returnToMenu;


    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject pauseMenuPanel;

    [Header("Text Elements")]
    public TMP_Text scoreText;

    private bool isPaused = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseSound != null)
        {

            TryGetComponent(out audioSource);

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("AudioSource component was missing. Added one dynamically.");
            }
        }



        if (playButton) playButton.onClick.AddListener(() => {
            Time.timeScale = 1f; // Unpause the game
            SceneManager.LoadScene(1);
        });
        if (settingsButton) settingsButton.onClick.AddListener(() => SetMenus(settingsPanel, mainMenuPanel));
        if (backButton) backButton.onClick.AddListener(() => SetMenus(mainMenuPanel, settingsPanel));
        if (backButton) backButton.onClick.AddListener(() => SetMenus(pauseMenuPanel, settingsPanel));
        if (settingsButton) settingsButton.onClick.AddListener(() => SetMenus(settingsPanel, pauseMenuPanel));
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
        if (resumeGame) resumeGame.onClick.AddListener(() => {
            isPaused = false;
            Time.timeScale = 1f;
            SetMenus(null, pauseMenuPanel);
        });


        if (returnToMenu) returnToMenu.onClick.AddListener(() => SceneManager.LoadScene(0));

    }





    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate) menuToActivate.SetActive(true);
        if (menuToDeactivate) menuToDeactivate.SetActive(false);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    
}
