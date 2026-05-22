
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;


    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.MainMenu;

        
        if (settingsButton) settingsButton.onClick.AddListener(() => JumpTo(MenuStates.Settings));
        if (quitButton) quitButton.onClick.AddListener(() => QuitGame());

    }
   
}

