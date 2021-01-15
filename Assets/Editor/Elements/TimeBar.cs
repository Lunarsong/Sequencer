using UnityEngine;
using UnityEngine.UIElements;

public class TimeBar : VisualElement
{
    internal new class UxmlFactory : UxmlFactory<TimeBar, UxmlTraits> { }

    static CustomStyleProperty<int> s_SpacingProperty = new CustomStyleProperty<int>("--spacing");
    static CustomStyleProperty<int> s_MediumLineSpacingProperty = new CustomStyleProperty<int>("--medium-line-spacing");
    static CustomStyleProperty<int> s_ThickLineSpacingProperty = new CustomStyleProperty<int>("--thick-line-spacing");    
    static CustomStyleProperty<int> s_LineHeightProperty = new CustomStyleProperty<int>("--line-height");
    static CustomStyleProperty<int> s_MediumLineHeightProperty = new CustomStyleProperty<int>("--medium-line-height");
    static CustomStyleProperty<int> s_ThickLineHeightProperty = new CustomStyleProperty<int>("--thick-line-height");    
    static CustomStyleProperty<Color> s_LineColorProperty = new CustomStyleProperty<Color>("--line-color");
    static CustomStyleProperty<Color> s_MediumLineColorProperty = new CustomStyleProperty<Color>("--medium-line-color");
    static CustomStyleProperty<Color> s_ThickLineColorProperty = new CustomStyleProperty<Color>("--thick-line-color");

    static readonly int s_DefaultSpacing = 20;
    static readonly int s_DefaultMediumLineSpacing = 5;
    static readonly int s_DefaultThickLineSpacing = 10;
    static readonly int s_DefaultLineHeight = 5;
    static readonly int s_DefaultMediumLineHeight = 14;
    static readonly int s_DefaultThickLineHeight = 22;
    static readonly Color s_DefaultLineColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    static readonly Color s_DefaultMediumLineColor = new Color(1.0f, 1.0f, 1.0f, 0.4f);
    static readonly Color s_DefaultThickLineColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);    

    private int Spacing { get; set; } = s_DefaultSpacing;
    private int MediumLineSpacing { get; set; } = s_DefaultMediumLineSpacing;
    private int ThickLineSpacing { get; set; } = s_DefaultThickLineSpacing;
    private int LineHeight { get; set; } = s_DefaultLineHeight;
    private int MediumLineHeight { get; set; } = s_DefaultMediumLineHeight;
    private int ThickLineHeight { get; set; } = s_DefaultThickLineHeight;
    Color LineColor { get; set; } = s_DefaultLineColor;
    Color MediumLineColor { get; set; } = s_DefaultMediumLineColor;
    Color ThickLineColor { get; set; } = s_DefaultThickLineColor;

    public TimeBar()
    {
        RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        generateVisualContent = OnGenerateVisualContent;
    }

    private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
    {
        ICustomStyle customStyle = e.customStyle;
        if (customStyle.TryGetValue(s_SpacingProperty, out int spacingValue))
        {
            Spacing = spacingValue;
        }
        if (customStyle.TryGetValue(s_MediumLineSpacingProperty, out int mediumLineSpacing))
        {
            MediumLineSpacing = mediumLineSpacing;
        }
        if (customStyle.TryGetValue(s_ThickLineSpacingProperty, out int thickLineSpacing))
        {
            ThickLineSpacing = thickLineSpacing;
        }
        if (customStyle.TryGetValue(s_LineHeightProperty, out int lineHeight))
        {
            LineHeight = lineHeight;
        }
        if (customStyle.TryGetValue(s_MediumLineHeightProperty, out int mediumLineHeight))
        {
            MediumLineHeight = mediumLineHeight;
        }
        if (customStyle.TryGetValue(s_ThickLineHeightProperty, out int thickLineHeight))
        {
            ThickLineHeight = thickLineHeight;
        }
        if (customStyle.TryGetValue(s_LineColorProperty, out Color lineColorValue))
        {
            LineColor = lineColorValue;
        }
        if (customStyle.TryGetValue(s_MediumLineColorProperty, out Color mediumLineColorValue))
        {
            MediumLineColor = mediumLineColorValue;
        }
        if (customStyle.TryGetValue(s_ThickLineColorProperty, out Color thickLineColorValue))
        {
            ThickLineColor = thickLineColorValue;
        }
    }

    void OnGenerateVisualContent(MeshGenerationContext context)
    {
        int numVerticalLines = Spacing == 0 ? 0 : ((int)(contentRect.width / Spacing) + 1);
        int numVertices = (numVerticalLines) * 4;
        int numIndices = (numVerticalLines) * 6;

        MeshWriteData mesh = context.Allocate(numVertices, numIndices);
        int vertexOffset = 0;
        for (int i = 0; i < numVerticalLines; ++i)
        {
            float x0 = contentRect.xMin + i * Spacing;
            float x1 = x0 + 1.0f;
            float y0 = contentRect.yMax - LineHeight;
            float y1 = contentRect.yMax;
            Color color = LineColor;
            if (i % ThickLineSpacing == 0)
            {
                color = ThickLineColor;
                y0 = contentRect.yMax - ThickLineHeight;
                /*x0 -= 1.0f;
                x1 += 1.0f;*/

            }
            else if (i % MediumLineSpacing == 0)
            {
                color = MediumLineColor;
                y0 = contentRect.yMax - MediumLineHeight;
                /*x0 -= 1.0f;
                x1 += 1.0f;*/

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

    void OnGeometryChanged(GeometryChangedEvent e)
    {
        float kLabelOffsetX = 2.0f;
        float kLabelOffsetY = 4.0f;
        this.Clear();
        if (Spacing == 0 || ThickLineSpacing == 0)
        {
            return;
        }
        int numLabels = ((int)(contentRect.width / (Spacing * ThickLineSpacing)) + 1);
        for (int i = 0; i < numLabels; ++i)
        {
            int thinkLineIndex = i * ThickLineSpacing;
            Label label = new Label(thinkLineIndex.ToString());
            label.style.left = this.contentRect.xMin + thinkLineIndex * Spacing + kLabelOffsetX;
            label.style.top = this.contentRect.yMin + kLabelOffsetY;
            label.style.position = Position.Absolute;
            label.style.unityTextAlign = TextAnchor.UpperLeft;
            Add(label);
        }
    }
}
