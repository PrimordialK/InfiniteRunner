using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event System.Action OnTouchBegin;
    public event System.Action OnTouchEnd;
    public event System.Action<Vector3> OnPhoneTilt;

    public Vector2 GetTouchScreenPosition() => input.Gameplay.PrimaryPosition.ReadValue<Vector2>();

    public Vector3 GetTouchWorldPosition(Camera mainCamera)
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector2 screenPos = GetTouchScreenPosition();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.nearClipPlane));

        return worldPos;
    }

    private PlayerControls input;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Gameplay.Touch.started += ctx => OnTouchBegin?.Invoke();
        input.Gameplay.Touch.canceled += ctx => OnTouchEnd?.Invoke();
        input.Gameplay.Tilt.performed += ctx => OnPhoneTilt?.Invoke(ctx.ReadValue<Vector3>());
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
