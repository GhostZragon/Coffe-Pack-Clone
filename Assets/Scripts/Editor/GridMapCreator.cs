using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GridMapCreator : EditorWindow
{
    private class Node
    {
        private Rect rect;
        public GUIStyle style;
        public Node(Vector2 position,float width,float heigh, GUIStyle defaultStyle)
        {
            rect = new Rect(position.x, position.y, width, heigh);
            style = defaultStyle;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }
        
        public void Draw()
        {
            GUI.Box(rect,"",style);
        }

        public void SetStyle(GUIStyle newStyle)
        {
            style = newStyle;
        }
        
    }
    private Vector2 offset;
    private Vector2 drag;
    private GUIStyle empty;
    private List<List<Node>> nodes;
    private Vector2 nodePos;
    private StyleManager styleManager;
    private bool isEarsing;
    private void OnEnable()
    {
        SetupStyles();
        empty = new GUIStyle();
        var icon = Resources.Load("IconTex/Empty") as Texture2D;
        empty.normal.background = icon;
        SetupNodes();
     
    }

    private void SetupStyles()
    {
        styleManager = FindFirstObjectByType<StyleManager>();

        for (int i = 0; i < styleManager.ButtonStyles.Length; i++)
        {
            styleManager.ButtonStyles[i].NodeStyle = new GUIStyle();
            styleManager.ButtonStyles[i].NodeStyle.normal.background = styleManager.ButtonStyles[i].icon;

        }
    }

    private void SetupNodes()
    {
        nodes = new();
        for (int i = 0; i < 20; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < 20; j++)
            {
                nodePos.Set(i * 30,j*30);
                nodes[i].Add(new Node(nodePos,30,30,empty));
            }
        }
    }

    [MenuItem("Window/Grid map creator")]
    private static void OpenWindown()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawNodes();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void ProcessNodes(Event e)
    {
        int row = (int)(e.mousePosition.x - offset.x) / 30;
        int col = (int)(e.mousePosition.y - offset.y) / 30;

        if (e.type == EventType.MouseDown)
        {
            isEarsing = nodes[row][col].style.normal.background.name != "Empty";

            if (isEarsing)
            {
                nodes[row][col].SetStyle(empty);
                GUI.changed = true;
            }
            else
            {
                nodes[row][col].SetStyle(styleManager.ButtonStyles[1].NodeStyle);
                GUI.changed = true;
            }
 
        }
    }

    private void DrawNodes()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                nodes[i][j].Draw();
            }
        }
    }

    private void ProcessGrid(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnMouseDrag(e.delta);
                }
                break;
        }
    }

    private void OnMouseDrag(Vector2 delta)
    {
        drag = delta;
        
        for (int i = 0; i < 20; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < 20; j++)
            {
                nodes[i][j].Drag(delta);
            }
        }
        
        GUI.changed = true;
    }

    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / 20);
        int heighDivider = Mathf.CeilToInt(position.height / 20);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        offset += drag;
        
        Vector3 newOffset = new Vector3(offset.x % 20, offset.y % 20, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Handles.DrawLine(new Vector3(20 * i, -20, 0) + newOffset,
                new Vector3(20 * i, position.height, 0) + newOffset);
        }

        for (int i = 0; i < heighDivider; i++)
        {
            Handles.DrawLine(new Vector3(-20, 20 * i, 0) + newOffset,
                new Vector3(position.width, 20 * i, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}