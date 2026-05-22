
using UnityEngine.UI;

public class ControlsMenu : BaseMenu
{
    public Button backButton;

    public override void Init(MenuController currentContext)
    {
        base.Init(currentContext);
        state = MenuStates.Controls;

        if (backButton) backButton.onClick.AddListener(() => JumpBack());

    }
}
