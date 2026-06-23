using UnityEngine;

public enum MenuStates // Enum to represent different menu states in the game, allowing for easy navigation and management of menus through the MenuController
{
    
    MainMenu,
    Settings,
    Pause,
    Audio,
    Controls,
    Credits,
    WinScreen,
    GameOver
} 

public class BaseMenu : MonoBehaviour
{
    [HideInInspector]
    public MenuStates state;
    protected MenuController context;



    public virtual void Init(MenuController currentContext) => context = currentContext; // Initialize the menu with a reference to the MenuController context, allowing it to call navigation methods like JumpTo and JumpBack
    public virtual void Enter() { }
    public virtual void Exit() { }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void JumpBack() => context.JumpBack();
    public void JumpTo(MenuStates nextState) => context.JumpTo(nextState);

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}

