using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    [Header("Gesture Detection Settings")]
    [Tooltip("Min distance for swipe as percentage of screen height/width")]
    [Range(0.01f, 0.5f)]
    public float minSwipeDistance = 0.05f;

    [Tooltip("Max time for a swipe gesture in seconds")]
    public float maxSwipeTime = 0.5f; // Max time for a swipe gesture

    [Tooltip("Directional threshold for swipe detection (0 to 1)")]
    [Range(0f, 1f)]
    public float directionalThreshold = 0.8f;

    [Tooltip("Max time for a tap gesture in seconds")]
    public float maxTapTime = 0.2f; // Max time for a tap gesture

    // Public facing events for detected gestures
    public event System.Action OnTap;
    public event System.Action<SwipeDirection> OnSwipe;

    // Internal state for gesture detection
    private Vector2 touchStartPos = Vector2.zero;
    private float touchStartTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("GestureDetector requires an InputManager instance in the scene.");
            enabled = false;
            return;
        }

        InputManager.Instance.OnTouchBegin += HandleTouchBegin;
        InputManager.Instance.OnTouchEnd += HandleTouchEnd;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTouchBegin -= HandleTouchBegin;
            InputManager.Instance.OnTouchEnd -= HandleTouchEnd;
        }
    }

    private void HandleTouchBegin()
    {
        touchStartTime = Time.time;
        touchStartPos = InputManager.Instance.GetTouchScreenPosition();
        Debug.Log($"Touch began at {touchStartPos} at time {touchStartTime}");
    }
    private void HandleTouchEnd()
    {
        float touchDuration = Time.time - touchStartTime;
        Vector2 touchEndPos = InputManager.Instance.GetTouchScreenPosition();

        //calculate veector of the movement in pixels
        Vector2 swipeVector = touchEndPos - touchStartPos;
        float swipeDistance = swipeVector.magnitude / Screen.height;

        if (touchDuration < maxTapTime && swipeDistance < minSwipeDistance)
        {
            // Detected a tap
            Debug.Log("Tap detected");
            OnTap?.Invoke();
            return;
        }
        
        if (touchDuration < maxSwipeTime && swipeDistance >= minSwipeDistance)
        {
            // Detected a swipe, determine direction
            Vector2 swipeDirection = swipeVector.normalized;
            SwipeDirection detectedDirection = GetDetectedDirection(swipeDirection);
            Debug.Log($"Swipe detected: {detectedDirection}");
            OnSwipe?.Invoke(detectedDirection);
        }
    }

    private SwipeDirection GetDetectedDirection(Vector2 swipeDirection)
    {
        float dotUp = Vector2.Dot(swipeDirection, Vector2.up);
        float dotRight = Vector2.Dot(swipeDirection, Vector2.right);

        if (dotUp > directionalThreshold)
        {
            return SwipeDirection.Up;
        }
        
        if (dotUp < -directionalThreshold)
        {
            return SwipeDirection.Down;
        }
        
        if (dotRight > directionalThreshold)
        {
            return SwipeDirection.Right;
        }
     
        if (dotRight < -directionalThreshold)
        {
            return SwipeDirection.Left;
        }

        // Default to Up if no direction is strongly detected
        return SwipeDirection.Up;
    }

    private void Update()
    {
        
    }
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
