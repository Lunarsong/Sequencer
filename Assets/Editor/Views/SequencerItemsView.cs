using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SequencerItemsView : VisualElement
{
    internal new class UxmlFactory : UxmlFactory<SequencerItemsView, UxmlTraits> { }

    //static readonly string kUssResourceName = "SequencerStyles";
    static readonly string kUxmlResourceName = "SequencerItemsViewLayout";
    static readonly string kClassName = "Sequencer__Items-View";

    private ScrollView m_ScrollView;
    public override VisualElement contentContainer => m_ScrollView;

    private VisualElement m_AddButton;
    public Action AddButtonClicked;
    

    public SequencerItemsView()
    {
        AddToClassList(kClassName);
        VisualTreeAsset visualAssetTree = Resources.Load<VisualTreeAsset>(kUxmlResourceName);
        visualAssetTree.CloneTree(this);

        m_AddButton = this.Q("AddButton");
        m_AddButton.RegisterCallback<MouseDownEvent>(OnAddButtonClicked);

        m_ScrollView = this.Q<ScrollView>();
    }

    void OnAddButtonClicked(MouseDownEvent e)
    {
        AddButtonClicked?.Invoke();
    }
}
