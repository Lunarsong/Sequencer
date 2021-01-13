using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SequencerTimeView : VisualElement
{
    internal new class UxmlFactory : UxmlFactory<SequencerTimeView, UxmlTraits> { }

    //static readonly string kUssResourceName = "SequencerStyles";
    static readonly string kUxmlResourceName = "SequencerTimeViewLayout";
    static readonly string kClassName = "Sequencer__TimeView";
    private TimeBar m_TimeBar;
    private ScrollView m_ScrollView;
    public override VisualElement contentContainer => m_ScrollView;
    public SequencerTimeView()
    {
        AddToClassList(kClassName);
        VisualTreeAsset visualAssetTree = Resources.Load<VisualTreeAsset>(kUxmlResourceName);
        visualAssetTree.CloneTree(this);

        m_TimeBar = this.Q<TimeBar>();
        m_ScrollView = this.Q<ScrollView>();
    }
}
