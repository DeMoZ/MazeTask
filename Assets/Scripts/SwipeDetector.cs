using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private float swipeThreshold = 0.5f;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;

    public Action<SwipeDirection> OnSwipe;

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        MouseSwipe();
#else
        TouchSwipe();
#endif
    }

    private void MouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
            StartTouch(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            EndTouch(Input.mousePosition);
    }

    private void TouchSwipe()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartTouch(touch.position);
                break;

            case TouchPhase.Ended:
            {
                EndTouch(touch.position);
                break;
            }
        }
    }

    private void StartTouch(Vector2 position)
    {
        _startTouchPosition = position;
    }

    private void EndTouch(Vector2 position)
    {
        _endTouchPosition = position;
        var swipeDistance = Vector2.Distance(_startTouchPosition, _endTouchPosition);

        if (swipeDistance > swipeThreshold)
        {
            var swipeDirection = _endTouchPosition - _startTouchPosition;

            if (swipeDirection.x > 0)
            {
                Debug.Log("Swipe right");
                OnSwipe?.Invoke(SwipeDirection.Right);
            }
            else if (swipeDirection.x < 0)
            {
                Debug.Log("Swipe left");
                OnSwipe?.Invoke(SwipeDirection.Left);
            }
            else if (swipeDirection.y > 0)
            {
                Debug.Log("Swipe up");
                OnSwipe?.Invoke(SwipeDirection.Up);
            }
            else if (swipeDirection.y < 0)
            {
                Debug.Log("Swipe down");
                OnSwipe?.Invoke(SwipeDirection.Down);
            }
        }
    }
}