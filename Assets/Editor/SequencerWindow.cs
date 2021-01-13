using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SequencerWindow : EditorWindow
{
    [MenuItem("Sequencer/Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SequencerWindow window = (SequencerWindow)EditorWindow.GetWindow(typeof(SequencerWindow));
        window.Show();
    }

    private void OnEnable()
    {
        rootVisualElement.Add(new SequencerView());
    }
}
