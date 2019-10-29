﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

//JESSE ALLAS
//Last Updated: 10/1/2019

//This is all the controls and stuff for the player
//designers probably won' have to touch this, 

//Keep in mind that all coordinates are relative to the object, so you dont need to worry about characters switching sides

public abstract class Player : MonoBehaviour
{
    public Camera cam;
    public bool IsP1 = false;
//    public string Path = FindObjectOfType<GameManager>().ReturnPath();
    public Text HP; //Temporary, displays the current HP
    public int Health; //Since the health value changes we want to copy the character health stat
    public PlayerControls Controls; //Player Controls
    
    public Character Self; //the character we're getting all the stats from

    public Player Opponent; //the opponent player object
    public string opponentTag; //The opponent tag, for checking collisions 

    bool TakingAction = false;     //a bool to prvent the player from interupting their moves

    public Vector2 Move;    //vector to move the player

    //the hitboxes for the different attacks
    public BoxCollider2D HighHitBox;
    public BoxCollider2D MedHitBox;
    public BoxCollider2D LowHitBox;
    public BoxCollider2D SpecHitBox;
    public BoxCollider2D PlayerBox;

    //RigidBody because physics dont work if we dont
    public Rigidbody2D RB;

    //bools to check if the player is blocking
    public bool HighBlocking;
    public bool LowBlocking;
    
    public bool Hit; //true when the player is hit

    string CurrentAtk; //the string titling the current move

    [SerializeField]
    SpriteRenderer CurrentForm;

    private void Start()
    {

        PlayerBox.size = new Vector2(Self.PlayerSize.x, Self.PlayerSize.y);
        PlayerBox.offset = new Vector2(Self.PlayerOffset.x, Self.PlayerOffset.y);

        Self.transform.position = transform.position;
        CurrentForm = GetComponent<SpriteRenderer>();
        
        //setting some initial things
        Hit = false;
        Health = Self.Health;
        HP.text = Health.ToString();
        HighBlocking = false;
        LowBlocking = false;
        RB = gameObject.GetComponent<Rigidbody2D>();
        HighHitBox.offset = Self.HighHitBoxOffset;
        MedHitBox.offset = Self.MedHitBoxOffset;
        LowHitBox.offset = Self.LowHitBoxOffset;
        SpecHitBox.offset = Self.SpecHitBoxOffset;

        HighHitBox.size = Self.HighHitBoxSize;
        MedHitBox.size = Self.MedHitBoxSize;
        LowHitBox.size = Self.LowHitBoxSize;
        SpecHitBox.size = Self.SpecHitBoxSize;
        //  ready = true;
    }

    private void Awake()
    {

    }


    void Update()
    {


        if (TakingAction == false)
        {
            //Move changes when the player waggles the analog stick
            //Move.x controls left and right movement
            if (Move.x >= 0.8f)
            {

                RB.velocity = new Vector2(50.0f * Self.MoveSpeed, 0.0f) * Time.fixedDeltaTime;
               
                //change blocking bool if the player is walking or not
                if (transform.localScale.x >=0.0f)
                {
                    HighBlocking = false;
                }
                else
                {
                    HighBlocking = true;
                }

            }
            //More movement
            else if (Move.x <= -0.8f)
            {

                RB.velocity = new Vector2(-50.0f * Self.MoveSpeed, 0.0f) * Time.fixedDeltaTime;

                if (transform.localScale.x <= 0.0f)
                {
                    HighBlocking = false;
                }
                else
                {
                    HighBlocking = true;
                }
            }
            else
            {
                RB.velocity = new Vector2(0.0f, 0.0f) * Time.fixedDeltaTime;
                HighBlocking = true;
            }

        //ducking stuff
        if (Move.y <= -0.6f && !(Move.x >= 0.8f || Move.x <= -0.8f))
            {
                CurrentForm.sprite = Self.CrouchSpr;
                PlayerBox.size = new Vector2(PlayerBox.size.x, Self.PlayerSize.y/2.0f);
                PlayerBox.offset = new Vector2(PlayerBox.offset.x, Self.PlayerOffset.y - 0.5f);
                LowBlocking = true;
                HighBlocking = false;
            }
        else
            {
                CurrentForm.sprite = Self.StandSpr;
                PlayerBox.size = new Vector2(PlayerBox.size.x, Self.PlayerSize.y);
                PlayerBox.offset = new Vector2(PlayerBox.offset.x, Self.PlayerOffset.y);
                LowBlocking = false;
              
            }
        }
        else
        {
            RB.velocity = new Vector2(0.0f, 0.0f) * Time.fixedDeltaTime;
        }




        //flips the character depending on which side of the screen they are on
        //basically we want everyone to face eachother
        if (Opponent.transform.position.x < gameObject.transform.position.x)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        }

        if (Hit == true)
        {
            RB.velocity = new Vector2(0.0f, 0.0f);
        }

    }
    





    //Coroutines for the different attack types, 
    //I'll only describe the high attack since low and mid attacks are the same as high

    public IEnumerator HighAttack()
    {

        //if the player isn't already attacking
        if (TakingAction == false)
        {
            HighBlocking = false;
            LowBlocking = false;
            // state that they are attacking    
            TakingAction = true;



            CurrentAtk = "High";
            CurrentForm.sprite = Self.HighSprStartUp;
            yield return StartCoroutine(PlayStartUpFrames(Self.HighAtkStartUp)); //start up frames
            CurrentForm.sprite = Self.HighSprHit;
            yield return StartCoroutine(PlayHitFrames(Self.HighAtkHit, HighHitBox)); //hit frames
            CurrentForm.sprite = Self.HighSprStartUp;
            yield return StartCoroutine(PlayCoolDownFrames(Self.HighAtkCoolDown)); //cooldown frames


            TakingAction = false; //attack is done
        }
    }

    public IEnumerator MedAttack()
    {
        if (TakingAction == false)
        {

            HighBlocking = false;
            LowBlocking = false;
            TakingAction = true;



            CurrentAtk = "Middle";
            CurrentForm.sprite = Self.MedSprStartUp;
            yield return StartCoroutine(PlayStartUpFrames(Self.MedAtkStartUp));
            CurrentForm.sprite = Self.MedSprHit;
            yield return StartCoroutine(PlayHitFrames(Self.MedAtkHit, MedHitBox));
            CurrentForm.sprite = Self.MedSprStartUp;
            yield return StartCoroutine(PlayCoolDownFrames(Self.MedAtkCoolDown));


            TakingAction = false;
        }
    }

    public IEnumerator LowAttack()
    {
        if (TakingAction == false)
        {
            HighBlocking = false;
            LowBlocking = false;
            TakingAction = true;



            CurrentAtk = "Low";
            CurrentForm.sprite = Self.LowSprStartUp;
            yield return StartCoroutine(PlayStartUpFrames(Self.LowAtkStartUp));
            CurrentForm.sprite = Self.LowSprHit;
            yield return StartCoroutine(PlayHitFrames(Self.LowAtkHit, LowHitBox));
            CurrentForm.sprite = Self.LowSprStartUp;
            yield return StartCoroutine(PlayCoolDownFrames(Self.LowAtkCoolDown));


            TakingAction = false;
        }
    }

    //Special attacks are a bit different
    public IEnumerator SpecAttack()
    {
        if (TakingAction == false)
        {
            CurrentAtk = "Special";
            //this is mainly to aim projectiles,
            //the character object and the player dont share positions so we have to do it manually
            Self.transform.position = transform.position;
            Self.transform.localScale = transform.localScale;

            TakingAction = true;

            CurrentForm.sprite = Self.SpecSprStartUp;
            yield return StartCoroutine(PlayStartUpFrames(Self.SpecAtkStartUp));
            CurrentForm.sprite = Self.SpecSprHit;
            yield return StartCoroutine(Self.SpecAtk(SpecHitBox)); //call the special attack directly
            CurrentForm.sprite = Self.SpecSprStartUp;
            yield return StartCoroutine(PlayCoolDownFrames(Self.SpecAtkCoolDown));
            TakingAction = false;
        }
    }

    //counts the frames
    IEnumerator PlayStartUpFrames(int Frames)
    {
        HighBlocking = false;
        LowBlocking = false;
        //copy the frames so we don't actually change the character
        int FrameCount = Frames;

        //this counts the frames
        while (FrameCount > 0)
        {
            FrameCount--;
            yield return null;
        }

        yield return null;
    }

    //counts more frames
    IEnumerator PlayHitFrames(int Frames, BoxCollider2D HitBox)
    {
        HitBox.enabled = true; //enables hitbox

        int FrameCount = Frames;
        while (FrameCount > 0)
        {
            FrameCount--;
            yield return null;
        }
        HitBox.enabled = false;
        yield return null;
    }


    IEnumerator PlayCoolDownFrames(int Frames)
    {
        int FrameCount = Frames;
        while (FrameCount > 0)
        {
            FrameCount--;
            yield return null;
        }
        yield return null;
    }

    //The player is a collider and cant activate triggers, so this is only for attacks
    void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 v = new Vector2(0.0f, 0.0f);



        //only activates if the object his is the opponent and not a projectile or another punch
        //remember all attacks are triggers, hence the !col.isTrigger
        if (col.tag == opponentTag && !col.isTrigger)
        {

            //Checks whatever attack the player is doing and compares it the whatever block the opponent is doing
            //As far as I know my logic here is solid, but it isn't too hard to change
            if (CurrentAtk == "High")
            {
                v = new Vector2(col.bounds.center.x + (col.bounds.size.x/2 * transform.localScale.x * -1.0f), HighHitBox.bounds.center.y);

                

                if (Opponent.HighBlocking == true)
                {
                    StartCoroutine(Blocking(Opponent.Self.StandBlockSpr));
                    GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                    StartCoroutine(KnockBack(0.1f, Self.HighAttackerBlockPush, Self.HighDefenderBlockPush));
                    //                    KnockBackSelf(Self.HighAttackerBlockPush);
                    //                  Opponent.KnockBackSelf(Self.HighDefenderBlockPush);
                    Debug.Log("BLOCK");
                }
                else
                {
                    Explode(v);
                    Opponent.TakeDamage(Self.HighAttackerHitPush,Self.HighDefenderHitPush);
                    Debug.Log("HIT");
                    
                }
            }
            else if (CurrentAtk == "Middle")
            {
                v = new Vector2(col.bounds.center.x + (col.bounds.size.x/2 * transform.localScale.x * -1.0f), MedHitBox.bounds.center.y);

                

                if (Opponent.HighBlocking == true)
                {
                    StartCoroutine(Blocking(Opponent.Self.StandBlockSpr));
                    GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                    KnockBack(0.1f, Self.MedAttackerBlockPush, Self.MedDefenderBlockPush);
                    //                    KnockBackSelf(Self.MedAttackerBlockPush);
                    //                  Opponent.KnockBackSelf(Self.MedDefenderBlockPush);
                    Debug.Log("BLOCK");
                }
                
                else
                {
                    Explode(v);
                    Opponent.TakeDamage(Self.MedAttackerHitPush, Self.MedDefenderHitPush);
                    Debug.Log("HIT");
                }

            }
            else if (CurrentAtk == "Low")
            {
                v = new Vector2(col.bounds.center.x + (col.bounds.size.x/2 * transform.localScale.x * -1.0f), LowHitBox.bounds.center.y);




                if (Opponent.LowBlocking == true)
                {
                    StartCoroutine(Blocking(Opponent.Self.CrouchBlockSpr));
                    GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                    StartCoroutine(KnockBack(0.1f, Self.LowAttackerBlockPush, Self.LowDefenderBlockPush));
                 //   KnockBackSelf(Self.LowAttackerBlockPush);
                   // Opponent.KnockBackSelf(Self.LowDefenderBlockPush);
                    Debug.Log("BLOCK");
                }
                else
                {

                    Opponent.TakeDamage(Self.LowAttackerHitPush, Self.LowDefenderHitPush);
                    Explode(v);
                    Debug.Log("HIT");
                }
            }
            else if (CurrentAtk == "Special")
            {
                v = new Vector2(col.bounds.center.x + (col.bounds.size.x / 2 * transform.localScale.x * -1.0f), SpecHitBox.bounds.center.y);
                if (Self.SpecialStr == "High")
                {
                    if (Opponent.HighBlocking == true)
                    {
                        StartCoroutine(Blocking(Opponent.Self.StandBlockSpr));
                        GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                        StartCoroutine(KnockBack(0.1f, Self.SpecAttackerBlockPush, Self.SpecDefenderBlockPush));
                        //   KnockBackSelf(Self.LowAttackerBlockPush);
                        // Opponent.KnockBackSelf(Self.LowDefenderBlockPush);
                        Debug.Log("BLOCK");
                    }
                    else
                    {

                        Opponent.TakeDamage(Self.SpecAttackerHitPush, Self.SpecDefenderHitPush);
                        Explode(v);
                        Debug.Log("HIT");
                    }
                }
                else if (Self.SpecialStr == "Middle")
                {
                    if (Opponent.HighBlocking == true)
                    {
                        StartCoroutine(Blocking(Opponent.Self.StandBlockSpr));
                        GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                        StartCoroutine(KnockBack(0.1f, Self.SpecAttackerBlockPush, Self.SpecDefenderBlockPush));
                        //   KnockBackSelf(Self.LowAttackerBlockPush);
                        // Opponent.KnockBackSelf(Self.LowDefenderBlockPush);
                        Debug.Log("BLOCK");
                    }
                    else
                    {

                        Opponent.TakeDamage(Self.SpecAttackerHitPush, Self.SpecDefenderHitPush);
                        Explode(v);
                        Debug.Log("HIT");
                    }
                }
                else if (Self.SpecialStr == "Low")
                {
                    if (Opponent.LowBlocking == true)
                    {
                        StartCoroutine(Blocking(Opponent.Self.CrouchBlockSpr));
                        GameObject firework = Instantiate(FindObjectOfType<GameManager>().BlockEffect, v, Quaternion.identity);
                        StartCoroutine(KnockBack(0.1f, Self.SpecAttackerBlockPush, Self.SpecDefenderBlockPush));
                        //   KnockBackSelf(Self.LowAttackerBlockPush);
                        // Opponent.KnockBackSelf(Self.LowDefenderBlockPush);
                        Debug.Log("BLOCK");
                    }
                    else
                    {

                        Opponent.TakeDamage(Self.SpecAttackerHitPush, Self.SpecDefenderHitPush);
                        Explode(v);
                        Debug.Log("HIT");
                    }
                }
                else
                {

                    Opponent.TakeDamage(Self.SpecAttackerHitPush, Self.SpecDefenderHitPush);
                    Explode(v);
                    Debug.Log("HIT");
                }
            }

            ///////////////////


        }
        
    }

    //since special attacks handle things differently
    //it would be easier to create a takedamage function
    //instead of a deal damage function

    //anyway this function causes the player to take damage
    public void TakeDamage(float AttackerPush, float DefenderPush)
    {
        //I want to impliment a slowdown/zoom in effect when someone gets hit
        //but it isn't necessary
        
        Health -= 1;
        FindObjectOfType<HealthDisplay>().ChangeHealth(gameObject.tag);
        HP.text = Health.ToString();
        Hit = true; //when hit is on the player cant move
        StartCoroutine(HitAnimation(AttackerPush,DefenderPush));
        TakingAction = false;
        Opponent.TakingAction = false;

    }

    void Explode(Vector2 position)
    {
        GameObject firework = Instantiate(FindObjectOfType<GameManager>().HitEffect, position, Quaternion.identity);
       // firework.GetComponent<ParticleSystem>().Play();
    }



    //I seperated this from the take damage because coroutines do things in the order I list them
    //functions tend to be less controlled

    //this function isnt working quite how I want it to but it works well enough right now
    IEnumerator HitAnimation(float AttackerPush, float DefenderPush)
    {
        CurrentForm.sprite = Self.HitSpr;
        //stop both players from moving
        Opponent.RB.velocity = new Vector2(0.0f, 0.0f);
        RB.velocity = new Vector2(0.0f, 0.0f);

        //send both players flying away from eachother
        StartCoroutine(KnockBack(0.3f,AttackerPush,DefenderPush));
        yield return StartCoroutine(SlowDown());
        //we send them flying for about a minute
        TakingAction = false;
        Opponent.TakingAction = false;


        Hit = false; //allow player to move now
        yield return null;
    }

    IEnumerator KnockBack(float time, float AttackerPush, float DefenderPush)
    {


        while (time > 0.0f)
        {

            RB.velocity = AttackerPush * transform.localScale * -1.0f;
            Opponent.RB.velocity = DefenderPush * Opponent.transform.localScale * -1.0f;
            time -= Time.deltaTime;
            yield return null;

        }
        yield return null;
    }

    public IEnumerator Blocking(Sprite BlockSpr)
    {
        Opponent.CurrentForm.sprite = BlockSpr;
        float time = 0.1f;
        while (time > 0.0f)
        {
            Opponent.CurrentForm.sprite = BlockSpr;
            time -= Time.deltaTime;
            yield return null;
        }


        yield return null;
    }

    public IEnumerator KnockBackSelf(float pushtime)
    {
        float t = pushtime;

        RB.AddForce(pushtime * transform.localScale * -1.0f);

        //while (t > 0.0f)
        //{
        //    RB.velocity = 10.0f * transform.localScale * -1.0f;
        //    t -= Time.deltaTime;
        //    yield return null;
        //}


        yield return null;
    }




    IEnumerator SlowDown()
    {

        TakingAction = true;
        Opponent.TakingAction = true;

        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.01f * 0.02f;
        while (cam.orthographicSize > 3.0f)
        {
            TakingAction = true;
            Opponent.TakingAction = true;
            cam.orthographicSize -= 0.5f;
           // yield return null;
        }
        yield return new WaitForSeconds(0.0025f);
        while (cam.orthographicSize < 10.0f)
        {
            TakingAction = true;
            Opponent.TakingAction = true;
            cam.orthographicSize += 0.5f;
            //yield return null;
        }


        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f * 0.02f;
        cam.orthographicSize = 10.0f;

        TakingAction = false;
        Opponent.TakingAction = false;
        yield return null;
    }
}
