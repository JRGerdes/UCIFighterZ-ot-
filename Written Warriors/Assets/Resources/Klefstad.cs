﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klefstad : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    //charge
    public override IEnumerator SpecAtk(BoxCollider2D SpecHitBox)
    {
        Player P = SpecHitBox.GetComponent<Player>();
        SpecHitBox.enabled = true;
        int F = SpecAtkHit;
        P.HighBlocking = true;
        P.LowBlocking = true;
        while (F > 0)
        {
            
            P.RB.velocity = new Vector2(MoveSpeed * 0.25f * transform.localScale.x, 0.0f);
            F -= 1;
            yield return null;
        }
        P.HighBlocking = false;
        P.LowBlocking = false;

        SpecHitBox.enabled = false;


        yield return null;
    }



    
}
