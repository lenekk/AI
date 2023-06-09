using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private float playerSpeed = 10f;
    private int currentIndex;
    private List<Vector3> pathVectorList;
    private int points = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("kolizja");
        points++;
        Destroy(collision.gameObject);
        if(collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
            SceneManager.LoadScene(1);
        }
    }


    public void movement()
    {
        if(pathVectorList != null)
        {
            Vector3 targetPos = pathVectorList[currentIndex];
            if (Vector3.Distance(transform.position, targetPos) > 1f)
            {
                Vector3 moveDir = (targetPos - transform.position).normalized;
                transform.position = transform.position + moveDir * playerSpeed * Time.deltaTime;
                transform.position.Normalize();
            }
            else
            {
                currentIndex++;
                if(currentIndex >= pathVectorList.Count)
                {
                    Stop();
                }
            }
        }
    }

    public int getPoints()
    {
        return points;
    }

    public void updatePoints()
    {
        points++;
    }

    public void setPlayerSpeed()
    {
        playerSpeed += 0.001f;
    }

    public void Stop()
    {
        pathVectorList = null;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void setTarget(Vector3 target)
    {
        //Debug.Log("z playeraa" + target);
        
        currentIndex = 0;
        pathVectorList = PathFinding.Instance.FindPath(getPosition(), target);

        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }


}
