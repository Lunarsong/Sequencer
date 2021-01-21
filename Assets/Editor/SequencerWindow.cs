using UnityEditor;
using UnityEngine.UIElements;

public class SequencerWindow : EditorWindow
{
    [MenuItem("Sequencer/Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SequencerWindow window = (SequencerWindow)EditorWindow.GetWindow(typeof(SequencerWindow));
        window.Show();
    }

    SequencerView m_SequencerView;
    
    private void OnEnable()
    {
        m_SequencerView = new SequencerView();
        m_SequencerView.RegisterItemCreation<SegmentedTrack>(CreateSegmentedTrackView);

        m_SequencerView.AddButtonClicked += () =>
        {
            m_SequencerView.CreateItem(new SegmentedTrack());
        };

        rootVisualElement.Add(m_SequencerView);
    }

    class SegmentedTrack
    {
        // TODO: Here should be a collection of segments data. And the whole class shouldn't be here, but serves as an example for now.
    }
    
    static void CreateSegmentedTrackView<SegmentedTrack>(SegmentedTrack data, out VisualElement detailElement, out VisualElement trackElement)
    {
        detailElement = new VisualElement();
        detailElement.AddToClassList("Sequencer__Item");

        trackElement = new VisualElement();
        trackElement.AddToClassList("Sequencer__TimeItem");
        VisualElement segmentsArea = new VisualElement();
        segmentsArea.AddToClassList("Sequencer__TimeItem__Track");
        TrackSegmentCreator trackCreationManipulator = new TrackSegmentCreator() {TickSize = 20.0f};
        segmentsArea.AddManipulator(trackCreationManipulator);
        trackCreationManipulator.OnCreate += CreateTrackSegment;
        trackElement.Add(segmentsArea);
        
        // TODO: Tie the track view with the provided SegmentedTrack data.
    }
    
    static void CreateTrackSegment(VisualElement target, float x0, float x1)
    {
        TrackSegment newTrack = new TrackSegment();
        newTrack.style.left = x0;
        newTrack.style.width = x1 - x0;
        target.Add(newTrack);
        
        // TODO: This only creates the view. Need to add data for it in SegmentedTrack and keep the view in sync with it.
    }
}
