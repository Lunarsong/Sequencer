using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridBackground : VisualElement
{
    internal new class UxmlFactory : UxmlFactory<GridBackground, UxmlTraits> { }

    static CustomStyleProperty<float> s_SpacingProperty = new CustomStyleProperty<float>("--spacing");
    static CustomStyleProperty<float> s_SpacingHorizontalProperty = new CustomStyleProperty<float>("--spacing-horizontal");
    static CustomStyleProperty<float> s_SpacingVerticalProperty = new CustomStyleProperty<float>("--spacing-vertical");
    static CustomStyleProperty<int> s_ThickLinesProperty = new CustomStyleProperty<int>("--thick-lines");
    static CustomStyleProperty<Color> s_LineColorProperty = new CustomStyleProperty<Color>("--line-color");
    static CustomStyleProperty<Color> s_ThickLineColorProperty = new CustomStyleProperty<Color>("--thick-line-color");
    static CustomStyleProperty<Color> s_GridBackgroundColorProperty = new CustomStyleProperty<Color>("--grid-background-color");
    static CustomStyleProperty<int> s_HorizontalLinesOffsetProperty = new CustomStyleProperty<int>("--horizontal-lines-offset");
    static CustomStyleProperty<int> s_VerticalLinesOffsetProperty = new CustomStyleProperty<int>("--vertical-lines-offset");

    static readonly float s_DefaultSpacing = 50f;
    static readonly int s_DefaultThickLines = 10;
    static readonly int s_DefaultHorizontalLinesOffset = 0;
    static readonly int s_DefaultVerticalLinesOffset = 0;
    static readonly Color s_DefaultLineColor = new Color(0f, 0f, 0f, 0.18f);
    static readonly Color s_DefaultThickLineColor = new Color(0f, 0f, 0f, 0.38f);
    static readonly Color s_DefaultGridBackgroundColor = new Color(0.17f, 0.17f, 0.17f, 1.0f);

    private float SpacingHorizontal { get; set; } = s_DefaultSpacing;
    private float SpacingVertical { get; set; } = s_DefaultSpacing;
    private float ThickLines { get; set; } = s_DefaultThickLines;
    private float HorizontalLinesOffset { get; set; } = s_DefaultHorizontalLinesOffset;
    private float VerticalLinesOffset { get; set; } = s_DefaultVerticalLinesOffset;
    Color LineColor { get; set; } = s_DefaultLineColor;
    Color ThickLineColor { get; set; } = s_DefaultThickLineColor;
    Color GridBackgroundColor { get; set; } = s_DefaultGridBackgroundColor;

    private VisualElement m_Container;

    public GridBackground()
    {
        pickingMode = PickingMode.Ignore;
        this.StretchToParentSize();
        RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        generateVisualContent = OnGenerateVisualContent;
    }

    private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
    {
        ICustomStyle customStyle = e.customStyle;
        if (customStyle.TryGetValue(s_SpacingProperty, out float spacingValue))
        {
            SpacingHorizontal = spacingValue;
            SpacingVertical = spacingValue;
        }
        if (customStyle.TryGetValue(s_SpacingHorizontalProperty, out float spacingHorizontalValue))
        {
            SpacingHorizontal = spacingHorizontalValue;
        }
        if (customStyle.TryGetValue(s_SpacingVerticalProperty, out float spacingVerticalValue))
        {
            SpacingVertical = spacingVerticalValue;
        }
        if (customStyle.TryGetValue(s_ThickLinesProperty, out int thicklinesValue))
        {
            ThickLines = thicklinesValue;
        }
        if (customStyle.TryGetValue(s_HorizontalLinesOffsetProperty, out int horizontalLinesOffset))
        {
            HorizontalLinesOffset = horizontalLinesOffset;
        }
        if (customStyle.TryGetValue(s_VerticalLinesOffsetProperty, out int verticalLinesOffset))
        {
            VerticalLinesOffset = verticalLinesOffset;
        }
        if (customStyle.TryGetValue(s_ThickLineColorProperty, out Color thicklineColorValue))
        {
            ThickLineColor = thicklineColorValue;
        }
        if (customStyle.TryGetValue(s_LineColorProperty, out Color lineColorValue))
        {
            LineColor = lineColorValue;
        }
        if (customStyle.TryGetValue(s_GridBackgroundColorProperty, out Color gridColorValue))
        {
            GridBackgroundColor = gridColorValue;
            style.backgroundColor = GridBackgroundColor;
        }
    }

    void OnGenerateVisualContent(MeshGenerationContext context)
    {
        int numVerticalLines = SpacingVertical == 0 ? 0 : ((int)(contentRect.width / SpacingVertical) + 1);
        int numHorizontalLines = SpacingHorizontal == 0 ? 0 : ((int)(contentRect.height / SpacingHorizontal) + 1);
        int numVertices = (numVerticalLines + numHorizontalLines) * 4;
        int numIndices = (numVerticalLines + numHorizontalLines) * 6;

        MeshWriteData mesh = context.Allocate(numVertices, numIndices);
        int vertexOffset = 0;
        for (int i = 0; i < numVerticalLines; ++i)
        {
            float x0 = contentRect.xMin + i * SpacingVertical + VerticalLinesOffset;
            float x1 = x0 + 1.0f;
            float y0 = contentRect.yMin;
            float y1 = contentRect.yMax;
            Color color = LineColor;
            if (i % ThickLines == 0)
            {
                color = ThickLineColor;
                x0 -= 1.0f;
                x1 += 1.0f;
            }
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, y0, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, y0, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, y1, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, y1, Vertex.nearZ), tint = color });

            mesh.SetNextIndex((ushort)vertexOffset);
            mesh.SetNextIndex((ushort)(vertexOffset + 1));
            mesh.SetNextIndex((ushort)(vertexOffset + 2));

            mesh.SetNextIndex((ushort)(vertexOffset + 1));
            mesh.SetNextIndex((ushort)(vertexOffset + 3));
            mesh.SetNextIndex((ushort)(vertexOffset + 2));
            vertexOffset += 4;

        }

        for (int i = 0; i < numHorizontalLines; ++i)
        {
            float x0 = contentRect.xMin;
            float x1 = contentRect.xMax;
            float y0 = contentRect.yMin + i * SpacingHorizontal + HorizontalLinesOffset;
            float y1 = y0 + 1.0f;
            Color color = LineColor;
            if (i % ThickLines == 0)
            {
                color = ThickLineColor;
                y0 -= 1.0f;
                y1 += 1.0f;
            }
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, y0, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, y0, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x0, y1, Vertex.nearZ), tint = color });
            mesh.SetNextVertex(new Vertex() { position = new Vector3(x1, y1, Vertex.nearZ), tint = color });

            mesh.SetNextIndex((ushort)vertexOffset);
            mesh.SetNextIndex((ushort)(vertexOffset + 1));
            mesh.SetNextIndex((ushort)(vertexOffset + 2));

            mesh.SetNextIndex((ushort)(vertexOffset + 1));
            mesh.SetNextIndex((ushort)(vertexOffset + 3));
            mesh.SetNextIndex((ushort)(vertexOffset + 2));
            vertexOffset += 4;
        }
    }
}
