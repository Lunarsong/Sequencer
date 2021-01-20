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

    Dictionary<Type, object> m_ViewCreations = new Dictionary<Type, object>();
    
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

    public delegate void ViewCreation<T>(T data, out VisualElement leftHandSide, out VisualElement rightHandSide);

    public void RegisterViewCreation<T>(ViewCreation<T> viewCreation)
    {
        m_ViewCreations.Add(typeof(T), viewCreation);
    }

    public void CreateView<T>(T data)
    {
        CreateView(data, out _, out _);
    }

    public void CreateView<T>(T data, out VisualElement leftHandSide, out VisualElement rightHandSide)
    {
        if (m_ViewCreations.TryGetValue(typeof(T), out var viewCreation))
        {
            ((ViewCreation<T>)viewCreation)(data, out leftHandSide, out rightHandSide);
            m_SequencerItemsView.Add(leftHandSide);
            m_SequencerTimeView.Add(rightHandSide);
        }
        else
        {
            throw new Exception($"There is no ViewCreation registered for type {typeof(T)}.");
        }
    }
}
