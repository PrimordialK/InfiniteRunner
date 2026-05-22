using System;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    private TrailRenderer tr;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        mainCamera = Camera.main;
        InputManager.Instance.OnTouchBegin += EnableTrailRenderer;
        InputManager.Instance.OnTouchEnd += DisableTrailRenderer;

        tr.enabled = false;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTouchBegin -= EnableTrailRenderer;
            InputManager.Instance.OnTouchEnd -= DisableTrailRenderer;
        }
    }

    private void EnableTrailRenderer()
    {
        UpdatePosition();
        tr.enabled = true;
        tr.Clear();
    }

    private void DisableTrailRenderer()
    {
        tr.enabled = false;
    }
      
    // Update is called once per frame
    void Update()
    {
        if (tr.enabled)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        if (InputManager.Instance != null)
        {
            Vector3 worldPos = InputManager.Instance.GetTouchWorldPosition(mainCamera);
            worldPos.z = 0;

            Vector3 screenPos = InputManager.Instance.GetTouchScreenPosition();
            screenPos.z = mainCamera.nearClipPlane;
            transform.position = worldPos;
        }
    }
}
