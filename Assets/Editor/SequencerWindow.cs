using UnityEditor;

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

        m_SequencerView.AddButtonClicked += () => {
            m_SequencerView.AddTrack(new SegmentedTrack());
        };

        rootVisualElement.Add(m_SequencerView);
    }
}
