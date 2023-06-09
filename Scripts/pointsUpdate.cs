using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointsUpdate : MonoBehaviour
{

    public Text punkty;
    private GameObject user;
    private int userScore;

    void Start()
    {
        punkty = GameObject.FindGameObjectWithTag("tekst").GetComponent<Text>();
        punkty.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
       if(GameObject.FindGameObjectWithTag("Player"))
        {
            punkty.text = (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getPoints()).ToString();
        }
     
    }
}
