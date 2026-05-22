using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMenu
{
    public Button resumeButton;
    public Button settingsButton;
    public Button restartButton;
    public Button quitButton;

    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.Pause;

        if (resumeButton) resumeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f; // Resume the game
            JumpBack();
        });
        if (settingsButton) settingsButton.onClick.AddListener(() => JumpTo(MenuStates.Settings));
        if (restartButton) restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f; // Ensure time is reset on restart
            SceneManager.LoadScene(1);
        });
        if (quitButton) quitButton.onClick.AddListener(() => QuitGame());
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f; // Freeze when pause menu opens
    }


}
