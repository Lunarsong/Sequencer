using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SequencerView : VisualElement
{
    static readonly string kUssResourceName = "SequencerStyles";
    static readonly string kUxmlResourceName = "SequencerLayout";
    static readonly string kClassName = "Sequencer";

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

        m_SequencerItemsView.AddButtonClicked += () => {
            VisualElement itemElement = new VisualElement();
            itemElement.AddToClassList("Sequencer__Item");
            m_SequencerItemsView.Add(itemElement);
            VisualElement timeElement = new VisualElement();
            timeElement.AddToClassList("Sequencer__TimeItem");
            m_SequencerTimeView.Add(timeElement);
        };
    }
}
