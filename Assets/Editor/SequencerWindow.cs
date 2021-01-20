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
        m_SequencerView.RegisterViewCreation<SegmentedTrack>(CreateSegmentedTrack);

        m_SequencerView.AddButtonClicked += () => {
            m_SequencerView.CreateView<SegmentedTrack>(null);
        };

        rootVisualElement.Add(m_SequencerView);
    }

    class SegmentedTrack { }
    
    void CreateSegmentedTrack<SegmentedTrack>(SegmentedTrack data, out VisualElement leftHandSide, out VisualElement rightHandSide)
    {
        leftHandSide = new VisualElement();
        leftHandSide.AddToClassList("Sequencer__Item");

        rightHandSide = new VisualElement();
        rightHandSide.AddToClassList("Sequencer__TimeItem");
        VisualElement trackElement = new VisualElement();
        trackElement.AddToClassList("Sequencer__TimeItem__Track");
        TrackSegmentCreator trackCreationManipulator = new TrackSegmentCreator() {TickSize = 20.0f};
        trackElement.AddManipulator(trackCreationManipulator);
        trackCreationManipulator.OnCreate += OnTrackTimeCreated;
        rightHandSide.Add(trackElement);
        
        // TODO: Tie the view with the data
    }
    
    void OnTrackTimeCreated(VisualElement target, float x0, float x1)
    {
        TrackSegment newTrack = new TrackSegment();
        newTrack.style.left = x0;
        newTrack.style.width = x1 - x0;
        target.Add(newTrack);
    }
}
