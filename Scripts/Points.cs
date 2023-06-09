using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    public PathFinding path;
    public GameObject prefab;
    private Transform pos;
    public Vector2 wektor;
   

    void Start()
    {
        //path = path.GetGrid().GetGridObject();
        wektor = randomNode();
        //Debug.Log(wektor);
        for(int i = 0; i < 20; i++)
        {
            wektor = randomNode();
            Instantiate(prefab, wektor + new Vector2(20, 44), Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        
       
    }

    public Vector3 randomNode()
    {
       return path.GetGrid().cell(Random.Range(0, path.GetGrid().getWidth()), Random.Range(0, path.GetGrid().getHeight()));
    }

}
