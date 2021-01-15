using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackSegment : VisualElement
{
    static readonly string kUssResourceName = "TrackSegmentStyles";
    static readonly string kUxmlResourceName = "TrackSegmentLayout";
    static readonly string kClassName = "Track-Segment";

    VisualElement m_LeftResizeElement;
    VisualElement m_RightResizeElement;
    VisualElement m_ContentContainer;

    public override VisualElement contentContainer => m_ContentContainer;
    public TrackSegment()
    {
        AddToClassList(kClassName);
        this.AddManipulator(new TrackSegmentMoveManipulator() { TickSize = 20.0f });

        m_ContentContainer = new VisualElement();
        m_ContentContainer.AddToClassList("Track-Segment__Container");
        hierarchy.Add(m_ContentContainer);

        m_LeftResizeElement = new VisualElement();
        m_LeftResizeElement.AddToClassList("Track_Segment__Left-Resizer");
        m_LeftResizeElement.AddManipulator(new TrackSegmentResizer() { TickSize = 20.0f });
        m_RightResizeElement = new VisualElement();
        m_RightResizeElement.AddToClassList("Track_Segment__Right-Resizer");
        m_RightResizeElement.AddManipulator(new TrackSegmentResizer() { Direction = TrackSegmentResizer.ResizeDirection.Right, TickSize = 20.0f });

        Add(m_LeftResizeElement);
        Add(m_RightResizeElement);


        RegisterCallback<MouseOverEvent>(OnMouseOver);
        RegisterCallback<MouseOutEvent>(OnMouseOut);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    void OnMouseOver(MouseOverEvent e)
    {
        e.StopImmediatePropagation();
    }
    void OnMouseOut(MouseOutEvent e)
    {
        e.StopImmediatePropagation();
    }
    void OnMouseMove(MouseMoveEvent e)
    {
        e.StopImmediatePropagation();
    }
}
