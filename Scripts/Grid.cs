using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid<GridObject>
{
    /*
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    */
    private int sizeX;
    private int sizeY;
    private float cellsize;
    private GridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    public const int sortingOrderDefault = 5000;
    private Vector3 originPosition;

    public Grid(int sizeX, int sizeY, float cellsize, Vector3 originPosition, Func<Grid<GridObject>, int, int, GridObject> createGridObject)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.cellsize = cellsize;
        this.originPosition = originPosition;
        gridArray = new GridObject[sizeX, sizeY];
        debugTextArray = new TextMesh[sizeX, sizeY];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
      /*
        for (int i=0; i<gridArray.GetLength(0); i++)
        {
            for(int j=0; j<gridArray.GetLength(1); j++)
            {
                debugTextArray[i,j] = CreateWorldText(gridArray[i,j]?.ToString(), null, cell(i,j) + new Vector3(cellsize,cellsize)*0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                debugTextArray[i, j].text = "";
                Debug.DrawLine(cell(i, j), cell(i, j + 1), Color.white, 100f);
                Debug.DrawLine(cell(i, j), cell(i+1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(cell(0, sizeX), cell(sizeX, sizeY), Color.white, 100f);
        Debug.DrawLine(cell(sizeY, 0), cell(sizeX, sizeY), Color.white, 100f);
        /*
        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
            //debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            debugTextArray[eventArgs.x, eventArgs.y].text = "xd";
        };
        */
    }

    public float getCellSize()
    {
        return cellsize;
    }

    public int getWidth()
    {
        return sizeX;
    }

    public int getHeight()
    {
        return sizeY;
    }

    public Vector3 cell(int x, int y)
    {
        return new Vector3(x, y) * cellsize;
    }

    public void getXY(Vector3 worldpos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldpos.x / cellsize);
        y = Mathf.FloorToInt(worldpos.y / cellsize);
    }

    public void setValue(int x, int y, string value)
    {
        if (x >= 0 && y >= 0 && x < sizeX && y < sizeY)
        {
            //gridArray[x, y] = value;
            debugTextArray[x, y].text = value;
            debugTextArray[x, y].color = Color.red;
        }
    }
    /*
    public void setValue(Vector3 worldpos, GridObject value)
    {
        int x, y;
        getXY(worldpos, out x, out y);
        setValue(x, y, value);
    }
    */
    public GridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < sizeX && y < sizeY)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridObject);
        }
    }


    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

}
