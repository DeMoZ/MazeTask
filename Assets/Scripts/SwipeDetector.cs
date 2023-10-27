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

    private Vector2 _startTouch;
    private Vector2 _endTouch;

    public event Action<SwipeDirection> OnSwipe;

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        MouseSwipe();
#else
        TouchSwipe();
#endif

        KeyboardMove();
    }

    private void KeyboardMove()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) OnSwipe?.Invoke(SwipeDirection.Left);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) OnSwipe?.Invoke(SwipeDirection.Right);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) OnSwipe?.Invoke(SwipeDirection.Up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) OnSwipe?.Invoke(SwipeDirection.Down);
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
        _startTouch = position;
    }

    private void EndTouch(Vector2 position)
    {
        _endTouch = position;
        var swipeDistance = Vector2.Distance(_startTouch, _endTouch);

        if (swipeDistance > swipeThreshold)
        {
            var swipeDirection = _endTouch - _startTouch;

            if (swipeDirection.x > 0) OnSwipe?.Invoke(SwipeDirection.Right);
            else if (swipeDirection.x < 0) OnSwipe?.Invoke(SwipeDirection.Left);
            
            if (swipeDirection.y > 0) OnSwipe?.Invoke(SwipeDirection.Up);
            else if (swipeDirection.y < 0) OnSwipe?.Invoke(SwipeDirection.Down);
        }
    }
}