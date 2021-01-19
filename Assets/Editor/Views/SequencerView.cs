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

    ScrollView m_ItemsViewScrollView;
    ScrollView m_TimeViewScrollView;

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
            VisualElement trackElement = new VisualElement();
            trackElement.AddToClassList("Sequencer__TimeItem__Track");
            TrackSegmentCreator trackCreationManipulator = new TrackSegmentCreator() { TickSize = 20.0f };
            trackElement.AddManipulator(trackCreationManipulator);
            trackCreationManipulator.OnCreate += OnTrackTimeCreated;
            timeElement.Add(trackElement);
            m_SequencerTimeView.Add(timeElement);
        };

        m_ItemsViewScrollView = m_SequencerItemsView.Q<ScrollView>();
        m_TimeViewScrollView = m_SequencerTimeView.Q<ScrollView>();
        m_ItemsViewScrollView.verticalScroller.valueChanged += value =>
        {
            m_TimeViewScrollView.verticalScroller.value = value;
        };
        m_TimeViewScrollView.verticalScroller.valueChanged += value =>
        {
            m_ItemsViewScrollView.verticalScroller.value = value;
        };
        
        m_ItemsViewScrollView.contentViewport.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        m_ItemsViewScrollView.contentContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        m_TimeViewScrollView.contentViewport.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        m_TimeViewScrollView.contentContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    void OnTrackTimeCreated(VisualElement target, float x0, float x1)
    {
        TrackSegment newTrack = new TrackSegment();
        newTrack.style.left = x0;
        newTrack.style.width = x1 - x0;
        target.Add(newTrack);
    }

    void OnGeometryChanged(GeometryChangedEvent e)
    {
        var itemsViewScrollerVisible = m_ItemsViewScrollView.horizontalScroller.visible;
        var timeViewScrollerVisible = m_TimeViewScrollView.horizontalScroller.visible;

        if (itemsViewScrollerVisible)
        {
            if (!timeViewScrollerVisible)
            {
                m_TimeViewScrollView.contentViewport.style.marginBottom = m_ItemsViewScrollView.contentViewport.style.marginBottom;
            }
        }
        else
        {
            if (timeViewScrollerVisible)
            {
                m_ItemsViewScrollView.contentViewport.style.marginBottom = m_TimeViewScrollView.contentViewport.style.marginBottom;
            }
            else
            {
                m_ItemsViewScrollView.contentViewport.style.marginBottom = 0f;
                m_TimeViewScrollView.contentViewport.style.marginBottom = 0f;
            }
        }
    }
}
