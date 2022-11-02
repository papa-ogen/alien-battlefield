using UnityEngine;

public class FireTeamDrag : MonoBehaviour
{
    Camera myCam;

    [SerializeField] RectTransform boxVisual;

    Rect selectionBox;
    Vector2 startPosition;
    Vector2 endPosition;

    void Start()
    {
        myCam = Camera.main;

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    void Update()
    {
        // clicked
        if(Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        // dragged
        if(Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        // when release click
        if(Input.GetMouseButtonUp(0))
        {
            SelectFireTeams();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        // dragging left
        if(Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        // dragging right
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        // dragging down
        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        // dragging up
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectFireTeams()
    {
        foreach(FireTeam fireTeam in FireTeamSelections.Instance.fireTeamList)
        {
            if(selectionBox.Contains(myCam.WorldToScreenPoint(fireTeam.transform.position)))
            {
                FireTeamSelections.Instance.DragSelect(fireTeam);
            }
        }
    }
}
