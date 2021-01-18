using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SequencerView : VisualElement
{
    static readonly string kUssResourceName = "SequencerStyles";
    static readonly string kUxmlResourceName = "SequencerLayout";
    static readonly string kClassName = "Sequencer";

    public event Action AddButtonClicked;

    SequencerItemsView m_SequencerItemsView;
    SequencerTimeView m_SequencerTimeView;

    public SequencerView()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>(kUssResourceName);
        styleSheets.Add(styleSheet);
        AddToClassList(kClassName);

        VisualTreeAsset visualAssetTree = Resources.Load<VisualTreeAsset>(kUxmlResourceName);
        visualAssetTree.CloneTree(this);

        m_SequencerItemsView = this.Q<SequencerItemsView>();
        m_SequencerTimeView = this.Q<SequencerTimeView>();

        m_SequencerItemsView.AddButtonClicked += () => AddButtonClicked?.Invoke();

        var itemsScrollView = m_SequencerItemsView.Q<ScrollView>();
        var timeScrollView = m_SequencerTimeView.Q<ScrollView>();
        itemsScrollView.verticalScroller.valueChanged += value =>
        {
            timeScrollView.verticalScroller.value = value;
        };
        timeScrollView.verticalScroller.valueChanged += value =>
        {
            itemsScrollView.verticalScroller.value = value;
        };
    }

    public void AddTrack(ITrack track)
    {
        m_SequencerItemsView.Add(track.ItemElement);
        m_SequencerTimeView.Add(track.TimeElement);
    }
}
