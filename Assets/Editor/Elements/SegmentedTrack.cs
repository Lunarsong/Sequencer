using UnityEngine.UIElements;

public class SegmentedTrack : ITrack
{
    VisualElement m_ItemElement;
    VisualElement m_TimeElement;

    public VisualElement ItemElement => m_ItemElement;
    public VisualElement TimeElement => m_TimeElement;
    
    public SegmentedTrack()
    {
        m_ItemElement = new VisualElement();
        m_ItemElement.AddToClassList("Sequencer__Item");

        m_TimeElement = new VisualElement();
        m_TimeElement.AddToClassList("Sequencer__TimeItem");
        VisualElement trackElement = new VisualElement();
        trackElement.AddToClassList("Sequencer__TimeItem__Track");
        TrackSegmentCreator trackCreationManipulator = new TrackSegmentCreator() {TickSize = 20.0f};
        trackElement.AddManipulator(trackCreationManipulator);
        trackCreationManipulator.OnCreate += OnTrackTimeCreated;
        m_TimeElement.Add(trackElement);
    }
    
    void OnTrackTimeCreated(VisualElement target, float x0, float x1)
    {
        TrackSegment newTrack = new TrackSegment();
        newTrack.style.left = x0;
        newTrack.style.width = x1 - x0;
        target.Add(newTrack);
    }
}
