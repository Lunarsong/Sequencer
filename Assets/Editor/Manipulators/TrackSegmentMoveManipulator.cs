using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackSegmentMoveManipulator : MouseManipulator
{
    public float TickSize = 0.0f;
    private bool IsActive { set; get; }
    private Vector2 ClickedWorldMousePos { set; get; }
    private float StartPosition;

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    void OnMouseDown(MouseDownEvent e)
    {
        ClickedWorldMousePos = e.mousePosition;
        StartPosition = target.style.left.value.value;
        IsActive = true;
        target.CaptureMouse();
        e.StopImmediatePropagation();
    }
    void OnMouseMove(MouseMoveEvent e)
    {
        if (IsActive)
        {
            e.StopImmediatePropagation();

            float xDiff = RoundToTick(e.mousePosition.x - ClickedWorldMousePos.x, TickSize);
            target.style.left = StartPosition + xDiff;
        }
    }

    void OnMouseUp(MouseUpEvent e)
    {
        if (IsActive)
        {
            IsActive = false;
            e.StopImmediatePropagation();
        }
        if (target.HasMouseCapture())
        {
            target.ReleaseMouse();
        }
    }

    float RoundToTick(float value, float tick)
    {
        return tick * Mathf.Round(value / tick);
    }
}
