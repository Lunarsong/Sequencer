using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackSegmentCreator : MouseManipulator
{
    public delegate void OnCreateCallback(VisualElement target, float x0, float x1);
    public OnCreateCallback OnCreate;

    public float TickSize = 0.0f;
    private bool IsActive { set; get; }
    private bool IsClicked { set; get; }
    private Vector2 CurrentLocalMousePos { set; get; }
    private Vector2 ClickedLocalMousePos { set; get; }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseOverEvent>(OnMouseOver);
        target.RegisterCallback<MouseOutEvent>(OnMouseOut);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        target.generateVisualContent += OnGenerateVisualContent;
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseOverEvent>(OnMouseOver);
        target.UnregisterCallback<MouseOutEvent>(OnMouseOut);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        target.generateVisualContent -= OnGenerateVisualContent;
    }

    void OnMouseDown(MouseDownEvent e)
    {
        if (!IsActive)
        {
            return;
        }
        ClickedLocalMousePos = CurrentLocalMousePos = e.localMousePosition;
        IsClicked = true;
        target.CaptureMouse();
        e.StopImmediatePropagation();
    }
    void OnMouseMove(MouseMoveEvent e)
    {
        CurrentLocalMousePos = e.localMousePosition;
        if (IsActive || IsClicked)
        {
            target.MarkDirtyRepaint();
        }
        if (IsClicked)
        {
            e.StopImmediatePropagation();
        }
    }
    void OnMouseOver(MouseOverEvent e)
    {
        IsActive = true;
        target.MarkDirtyRepaint();
        CurrentLocalMousePos = e.localMousePosition;
    }
    void OnMouseOut(MouseOutEvent e)
    {
        IsActive = false;
        target.MarkDirtyRepaint();
    }
    void OnMouseUp(MouseUpEvent e)
    {
        if (IsClicked)
        {
            IsClicked = false;
            e.StopImmediatePropagation();

            float x0 = ClickedLocalMousePos.x;
            float x1 = CurrentLocalMousePos.x;
            if (TickSize != 0.0f)
            {
                x0 = RoundToTick(x0, TickSize);
                x1 = RoundToTick(x1, TickSize);
            }
            if (x1 < x0)
            {
                float temp = x0;
                x0 = x1;
                x1 = temp;
            }
            OnCreate?.Invoke(target, x0, x1);
        }
        if (target.HasMouseCapture())
        {
            target.ReleaseMouse();
        }
    }

    void OnGenerateVisualContent(MeshGenerationContext context)
    {
        if (!IsActive && !IsClicked)
        {
            return;
        }

        MeshWriteData mesh = context.Allocate(4, 6);
        float clickedPos = ClickedLocalMousePos.x;
        float currentPos = CurrentLocalMousePos.x;
        if (TickSize != 0.0f)
        {
            clickedPos = RoundToTick(clickedPos, TickSize);
            currentPos = RoundToTick(currentPos, TickSize);
        }
        float x0 = IsClicked ? clickedPos : currentPos;
        float x1 = IsClicked ? currentPos : (x0 + 1.0f);
        
        if (x1 < x0)
        {
            float temp = x0;
            x0 = x1;
            x1 = temp;
        }

        Color32 color = Color.blue;
        mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, target.contentRect.yMin, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, target.contentRect.yMin, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, target.contentRect.yMax, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, target.contentRect.yMax, Vertex.nearZ), tint = color });

        mesh.SetNextIndex(0);
        mesh.SetNextIndex(1);
        mesh.SetNextIndex(2);
        mesh.SetNextIndex(1);
        mesh.SetNextIndex(3);
        mesh.SetNextIndex(2);

    }

    float RoundToTick(float value, float tick)
    {
        return tick * Mathf.Round(value / tick);
    }
}
