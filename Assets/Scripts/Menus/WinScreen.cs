
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : BaseMenu
{
    public Button mainMenuButton;
   
    public Button quitButton;


    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.WinScreen;

        if (mainMenuButton) mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        if (quitButton) quitButton.onClick.AddListener(() => QuitGame());

    }

}
