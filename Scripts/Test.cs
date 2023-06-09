using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Player playerPath;
    [SerializeField] private Enemy enemyPath;
    private bool flag = false;

    public Vector2 wektor;
    public PathFinding pathFinding;
    
    public GameObject cell;
    public GameObject prefab;
    public GameObject cellN;
    private bool beingHandled = false;
    

    void Start()
    {
        pathFinding = new PathFinding(10, 10);

        for (int i = 0; i < 30; i++)
        {
            int x = pathFinding.GetGrid().getWidth();
            int y = pathFinding.GetGrid().getHeight();

            int nx = Random.Range(0, x);
            int ny = Random.Range(0, y);

            

            if (pathFinding.GetNode(nx, ny).isWalkable == false || (nx==0 && ny==0) || (nx == 0 && ny == 9))
            {
                nx = Random.Range(0, x);
                ny = Random.Range(0, y);
            }

            Debug.Log("To sa wylaczone: " + nx + " " + ny);

            pathFinding.GetNode(nx, ny).isWalkable = false;
            

        }


        for (int i = 0; i < pathFinding.GetGrid().getWidth(); i++)
        {
            for (int j = 0; j < pathFinding.GetGrid().getHeight(); j++)
            {
                if (pathFinding.GetGrid().GetGridObject(i, j).isWalkable)
                {
                    wektor = pathFinding.GetGrid().cell(i, j);
                    Instantiate(cell, wektor + new Vector2(5, 5), Quaternion.identity);
                }
                else if(pathFinding.GetGrid().GetGridObject(i, j).isWalkable == false)
                {
                    wektor = pathFinding.GetGrid().cell(i, j);
                    Instantiate(cellN, wektor + new Vector2(5, 5), Quaternion.identity);
                }
               
            }
        }

        for (int i = 0; i < 20; i++)
        {
            wektor = randomNode();
            
            Instantiate(prefab, wektor + new Vector2(5, 5), Quaternion.identity);
        }


    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            //grid.setValue(GetMouseWorldPosition(), 1);

            Vector3 mouseWorldPosition = GetMouseWorldPosition();
           // Debug.Log(mouseWorldPosition);
            pathFinding.GetGrid().getXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
     
            if(path != null)
            {
                for(int i=0; i < path.Count - 1; i++)
                {
                    //Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i+1].x, path[i+1].y) * 10f + Vector3.one * 5f, Color.red, 100000f);
                }
            }

            playerPath.setTarget(mouseWorldPosition);
            //enemyPath.setTarget(setEnemyTarget());
        }

        

        if(playerPath.getPoints()%20 == 0 && playerPath.getPoints() != 0)
        {
            if (flag == false)
            {
                newCoins();
                flag = true;
            }
        }
      

        if (!beingHandled)
        {
            StartCoroutine(HandleIt());
        }
      
        /*

        if (Input.GetMouseButton(1))
                {
                    Vector3 mouseWorldPosition = GetMouseWorldPosition();
                    pathFinding.GetGrid().getXY(mouseWorldPosition, out int x, out int y);
                    pathFinding.GetNode(x, y).setToNotWalkable();
                    pathFinding.GetGrid().setValue(x, y, "Closed");

                }*/
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 GetDirToMouse(Vector3 fromPosition)
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        return (mouseWorldPosition - fromPosition).normalized;
    }

    private void newCoins()
    {
        for (int i = 0; i < 20; i++)
        {
            wektor = randomNode();
            playerPath.updatePoints();
            Instantiate(prefab, wektor + new Vector2(5, 5), Quaternion.identity);
        }
    }

    private IEnumerator HandleIt()
    {
        beingHandled = true;
        enemyPath.setTarget(randomNode());
        yield return new WaitForSeconds(1f);
        // process post-yield
        beingHandled = false;
    }

    public Vector3 randomNode()
    {
        int x = pathFinding.GetGrid().getWidth();
        int y = pathFinding.GetGrid().getHeight();

        int nx = Random.Range(0, x);
        int ny = Random.Range(0, y);

        while (pathFinding.GetNode(nx, ny).isWalkable == false)
        {
            nx = Random.Range(0, x);
            ny = Random.Range(0, y);
        }

        return pathFinding.GetGrid().cell(nx, ny);
    }

}
