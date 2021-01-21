using System;
using System.Collections.Generic;
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

    ScrollView m_ItemsViewScrollView;
    ScrollView m_TimeViewScrollView;

    Dictionary<Type, Delegate> m_ItemCreations = new Dictionary<Type, Delegate>();
    
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

    public delegate void ItemCreation<T>(T data, out VisualElement detailElement, out VisualElement trackElement);

    public void RegisterItemCreation<T>(ItemCreation<T> itemCreation)
    {
        m_ItemCreations[typeof(T)] = itemCreation;
    }

    public void CreateItem<T>(T data)
    {
        CreateItem(data, out _, out _);
    }

    public void CreateItem<T>(T data, out VisualElement detailElement, out VisualElement trackElement)
    {
        if (m_ItemCreations.TryGetValue(typeof(T), out var itemCreation))
        {
            ((ItemCreation<T>)itemCreation)(data, out detailElement, out trackElement);
            m_SequencerItemsView.Add(detailElement);
            m_SequencerTimeView.Add(trackElement);
        }
        else
        {
            throw new Exception($"There is no ItemCreation<{typeof(T)}> registered in this {nameof(SequencerView)}.");
        }
    }
}
