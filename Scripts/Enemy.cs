using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        setPlayerSpeed();
    }

  


}
