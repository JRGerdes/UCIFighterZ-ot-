﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattis : Character
{
    // Start is called before the first frame update
    void Start()
    {

    }
//    float TeleportDistance;

    //teleport
    public override IEnumerator SpecAtk(BoxCollider2D SpecHitBox)
    {

        Player P = SpecHitBox.GetComponent<Player>();
        SpecHitBox.gameObject.layer = 10;
        P.HighBlocking = true;
        P.LowBlocking = true;
        int F = SpecAtkHit;

        SpecHitBox.GetComponent<SpriteRenderer>().enabled = false;
        float Speed = 15.0f * transform.localScale.x;
        
        while (F >= 0)
        {
            P.RB.velocity = new Vector2(Speed, 0.0f);
            F -= 1;
            yield return null;
        }
        P.HighBlocking = false;
        P.LowBlocking = false;

        SpecHitBox.GetComponent<SpriteRenderer>().enabled = true;
        SpecHitBox.gameObject.layer = 8;
        P.RB.velocity = new Vector2(0.0f, 0.0f);
        P.PlayerBox.enabled = true;
        //        Transform T = SpecHitBox.GetComponent<Transform>();
        //      T.position = new Vector2(T.position.x + Spec)
        yield return null;
    }
}
