
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : BaseMenu
{
    public Button mainMenuButton;

    public Button restartButton;
    public Button quitButton;


    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.GameOver;

        if (mainMenuButton) mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        if (restartButton) restartButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        if (quitButton) quitButton.onClick.AddListener(() => QuitGame());

    }

}