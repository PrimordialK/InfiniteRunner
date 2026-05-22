
using UnityEngine.UI;

public class SettingsMenu : BaseMenu 
{
    #region Buttons
    public Button backButton;
    public Button audioButton;
    public Button controls;
    #endregion
    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.Settings;

        if (backButton) backButton.onClick.AddListener(() => JumpBack());
        if (audioButton) audioButton.onClick.AddListener(() => JumpTo(MenuStates.Audio));
        if (controls) controls.onClick.AddListener(() => JumpTo(MenuStates.Controls));


    }


}

