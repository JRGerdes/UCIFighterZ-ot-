﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharSelectScr : MonoBehaviour
{
    public PlayerControls Controls; //Player Controls

    public Image[] CharPics;

    public Image P1Arrow;
    public Image P2Arrow;
    Vector2 SizeLarge;
    Vector2 SizeSmall;

    [SerializeField]
    private GameManager manager;
    
    int indexP1;
    int indexP2;

    bool turn1 = true;
    bool turn2 = true;

    bool ReadyP1 = false;
    bool ReadyP2 = false;


    public Vector2 MoveP1;
    public Vector2 MoveP2;

    public TextMeshProUGUI READY;
    public TextMeshProUGUI READYBG;
    public TextMeshProUGUI RDYP1;
    public TextMeshProUGUI RDYP1BG;
    public TextMeshProUGUI RDYP2;
    public TextMeshProUGUI RDYP2BG;
    public TextMeshProUGUI P1CHAR;
    public TextMeshProUGUI P1CHARBG;
    public TextMeshProUGUI P2CHAR;
    public TextMeshProUGUI P2CHARBG;

    private Image screen;
    
    static public int positionP1 = 0;
    static public int positionP2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Start playing theme song
        //StartCoroutine(FindObjectOfType<AudioManager>().PlayFadeIn("VGDCTheme"));

        //Set initial indices
        indexP1 = 0;
        indexP2 = 0;

        //Set initial positions and dimensions
        //P1
        float x = CharPics[indexP1].rectTransform.position.x;
        float y = CharPics[indexP1].rectTransform.position.y;
        float sh = CharPics[indexP1].rectTransform.rect.height;
        float sw = CharPics[indexP1].rectTransform.rect.width;
        P1Arrow.rectTransform.sizeDelta = new Vector2(sw, sh);
        P1Arrow.rectTransform.position = new Vector2(x, y + sh / 8);
        //P2
        float x2 = CharPics[indexP2].rectTransform.position.x;
        float y2 = CharPics[indexP2].rectTransform.position.y;
        float sh2 = CharPics[indexP2].rectTransform.rect.height;
        float sw2 = CharPics[indexP2].rectTransform.rect.width;
        P2Arrow.rectTransform.sizeDelta = new Vector2(sw2, sh2);
        P2Arrow.rectTransform.position = new Vector2(x2, y2 - sh2 / 8);

        //Get sizes of small and large buttons
        RectTransform rt = P1Arrow.GetComponent<RectTransform>();
        SizeLarge = rt.sizeDelta;
        SizeSmall = new Vector2(rt.rect.width*2, rt.rect.height * .6f);

        //Start f l a s h i n g
        StartCoroutine(FlashText(P1CHAR, P1CHARBG));
        StartCoroutine(FlashTextP2(P2CHAR, P2CHARBG));

        //Screen fade in
        screen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Image>();
        StartCoroutine(manager.FadeScreenIn(screen));

    }

    // Update is called once per frame
    void Update()
    {
        //If P1 has selected player
        if (ReadyP1 == true)
        {
            RDYP1.enabled = true;
            RDYP1BG.enabled = true;
        }
        else
        {
            RDYP1.enabled = false;
            RDYP1BG.enabled = false;
        }
        //If P2 has selected player
        if(ReadyP2 == true)
        {
            RDYP2.enabled = true;
            RDYP2BG.enabled = true;
        }
        else
        {
            RDYP2.enabled = false;
            RDYP2BG.enabled = false;
        }
        //If both P1 and P2 have selected
        if (ReadyP1 == true && ReadyP2 == true)
        {
            READY.enabled = true;
            READYBG.enabled = true;
        }
        else
        {
            READY.enabled = false;
            READYBG.enabled = false;
        }

        //Get User Input
        MoveP2 = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
        MoveP1 = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));

        if (Input.GetKey(KeyCode.A)) //P1 left
        {
            MoveP1 = new Vector2(-1.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D)) //P1 right
        {
            MoveP1 = new Vector2(1.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.S)) //P1 down
        {
            MoveP1 = new Vector2(0.0f, -1.0f);
        }
        else if (Input.GetKey(KeyCode.W)) //P1 down
        {
            MoveP1 = new Vector2(0.0f, 1.0f);
        }
        if (Input.GetKey(KeyCode.K)) //P2 left
        {
            MoveP2 = new Vector2(-1.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.Semicolon)) //P2 right
        {
            MoveP2 = new Vector2(1.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.L)) //P2 down
        {
            MoveP2 = new Vector2(0.0f, -1.0f);
        }
        else if (Input.GetKey(KeyCode.O)) //P1 down
        {
            MoveP1 = new Vector2(0.0f, 1.0f);
        }

        //If P2 movement detected
        if (ReadyP2 == false && turn2 == true && ((MoveP2.x > 0.8f || MoveP2.x < -0.8f) || (MoveP2.y > 0.8f || MoveP2.y < -0.8f)))
        {
            turn2 = false;
            StartCoroutine(ShiftP2Cursor());
        }

        //If P1 movement detected
        if (ReadyP1 == false && turn1 == true && ((MoveP1.x > 0.8f || MoveP1.x < -0.8f) || (MoveP1.y > 0.8f || MoveP1.y < -0.8f)))
        {
            turn1 = false;
            StartCoroutine(ShiftP1Cursor());
        }

        //???
        if (ReadyP1 == true && ReadyP2 == true)
        {
        }
        else
        {
        }

        //If P1 locks in character
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Alpha4))
        {
            SelectP1();
        }
        //If P2 locks in character
        if (Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKey(KeyCode.Minus))
        {
            SelectP2();
        }
        //If both players have selected
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick2Button3) || Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Equals))
        {
            StartMatch();
        }
        //P1 back
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.R))
        {
            BackP1();
        }
        //P2 back
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKey(KeyCode.LeftBracket))
        {
            BackP2();
        }

    }

    //If P1 selects
    void SelectP1()
    {
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        ReadyP1 = true;
        FindObjectOfType<GameManager>().PathP1 = FindSource(indexP1);
       
    }
    //If P2 selects
    void SelectP2()
    {
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        ReadyP2 = true;
        FindObjectOfType<GameManager>().PathP2 = FindSource(indexP2);
    }
    //P1 back
    void BackP1()
    {
        ReadyP1 = false;
    }
    //P2 back
    void BackP2()
    {
        ReadyP2 = false;
    }
    //Begin Match if both players are ready
    void StartMatch()
    {
        if (ReadyP1 && ReadyP2)
        {
            StartCoroutine(Wait());
        }

    }

    //Find what button was selected
    string FindSource(int index)
    {
        string path = "SampleCharactePreFab";

        switch (index)
        {
            case 0:
                path = "Thornton/Thornton";
                break;
            case 1:
                path = "Klefstad/Klefstad";
                break;
            case 2:
                path = "Pattis/Pattis";
                break;

            case 3:
                path = "Shindler/Shindler";
                break;
            case 4:
                path = "Wong Ma/WongMa";
                break;
            case 5:
                path = "Navarro/Navarro";
                break;
            case 6:
                path = "Options";
                SceneManager.LoadScene("OptionsMenu");
                break;
            case 7:
                path = "ZotFighter";
                SceneManager.LoadScene("ZotFighter");
                break;
            case 8:
                path = "Health";
                SceneManager.LoadScene("Health");
                break;
            case 9:
                path = "Controls";
                SceneManager.LoadScene("Controls");
                break;
            case 10:
                path = "Specials";
                SceneManager.LoadScene("Specials");
                break;
            case 11:
                path = "SuddenDeath";
                SceneManager.LoadScene("Health");
                break;
            default:
                path = "SampleCharactePreFab";
                break;
        }
        return path;
    }

    //Move cursor for P2
    IEnumerator ShiftP2Cursor()
    {
        turn2 = false;
        FindObjectOfType<AudioManager>().Play("MenuScroll");
        while (turn2 == false)
        {
            if (MoveP2.x > 0.8f)
            {
                if (indexP2 == 11)
                {
                    indexP2 = 0;
                }
                else
                {
                    indexP2 += 1;
                }
            }

            else if (MoveP2.x < -0.8f)
            {
                if (indexP2 == 0)
                {
                    indexP2 = 11;
                }
                else
                {
                    indexP2 -= 1;
                }
            }

            else if (MoveP2.y > 0.8f)
            {
                if (indexP2 <= 2)
                    indexP2 = 6;
                else if (indexP2 <= 5)
                    indexP2 -= 3;
                else
                    indexP2 = 4;
            }

            else if (MoveP2.y < -0.8f)
            {
                if (indexP2 <= 2)
                    indexP2 += 3;
                else if (indexP2 <= 5)
                    indexP2 = 9;
                else
                    indexP2 = 0;
            }

            yield return new WaitForSeconds(0.15f);
            turn2 = true;


            float x = CharPics[indexP2].rectTransform.position.x;
            float y = CharPics[indexP2].rectTransform.position.y;
            float sh = CharPics[indexP2].rectTransform.rect.height;
            float sw = CharPics[indexP2].rectTransform.rect.width;
            P2Arrow.rectTransform.sizeDelta = new Vector2(sw, sh);
            P2Arrow.rectTransform.position = new Vector2(x,y - sh/8);
            if (indexP2 > 5)
            {
                P2Arrow.rectTransform.sizeDelta = SizeSmall;
                P2Arrow.rectTransform.position = new Vector2(x, y-18);
            }
            else
                P2Arrow.rectTransform.sizeDelta = SizeLarge;

            if (indexP2 == 0)
            {
                P2CHAR.text = "THORNTON";
                P2CHARBG.text = "THORNTON";
            }
            else if (indexP2 == 1)
            {
                P2CHAR.text = "KLEFSTAD";
                P2CHARBG.text = "KLEFSTAD";
            }
            else if (indexP2 == 2)
            {
                P2CHAR.text = "PATTIS";
                P2CHARBG.text = "PATTIS";
            }
            else if (indexP2 == 3)
            {
                P2CHAR.text = "SHINDLER";
                P2CHARBG.text = "SHINDLER";
            }
            else if (indexP2 == 4)
            {
                P2CHAR.text = "WONG MA";
                P2CHARBG.text = "WONG MA";
            }
            else if (indexP2 == 5)
            {
                P2CHAR.text = "NAVARRO";
                P2CHARBG.text = "NAVARRO";
            }
            else if (indexP2 == 6)
            {
                P2CHAR.text = "OPTIONS";
                P2CHARBG.text = "OPTIONS";
            }
            else if (indexP2 == 7)
            {
                P2CHAR.text = "ZOTFIGHTER";
                P2CHARBG.text = "ZOTFIGHTER";
            }
            else if (indexP2 == 8)
            {
                P2CHAR.text = "HEALTH";
                P2CHARBG.text = "HEALTH";
            }
            else if (indexP2 == 9)
            {
                P2CHAR.text = "CONTROLS";
                P2CHARBG.text = "CONTROLS";
            }
            else if (indexP2 == 10)
            {
                P2CHAR.text = "SPECIALS";
                P2CHARBG.text = "SPECIALS";
            }
            else if (indexP2 == 11)
            {
                P2CHAR.text = "SUDDEN DEATH";
                P2CHARBG.text = "SUDDEN DEATH";
            }
        }
    }
        //Move cursor for P1
        IEnumerator ShiftP1Cursor()
        {
            turn1 = false;
        FindObjectOfType<AudioManager>().Play("MenuScroll");
        while (turn1 == false)
            {
                if (MoveP1.x > 0.8f)
                {
                    if (indexP1 == 11)
                    {
                        indexP1 = 0;
                    }
                    else
                    {
                        indexP1 += 1;
                    }
                }

                else if (MoveP1.x < -0.8f)
                {
                    if (indexP1 == 0)
                    {
                        indexP1 = 11;
                    }
                    else
                    {
                        indexP1 -= 1;
                    }
                }

            else if (MoveP1.y > 0.8f)
            {
                if (indexP1 <= 2)
                    indexP1 = 6;
                else if (indexP1 <= 5)
                    indexP1 -= 3;
                else
                    indexP1 = 4;
            }

            else if (MoveP1.y < -0.8f)
            {
                if (indexP1 <= 2)
                    indexP1 += 3;
                else if (indexP1 <= 5)
                    indexP1 = 9;
                else
                    indexP1 = 0;
            }

            yield return new WaitForSeconds(0.15f);
            turn1 = true;

            float x = CharPics[indexP1].rectTransform.position.x;
            float y = CharPics[indexP1].rectTransform.position.y;
            float sh = CharPics[indexP1].rectTransform.rect.height;
            float sw = CharPics[indexP1].rectTransform.rect.width;
            P1Arrow.rectTransform.sizeDelta = new Vector2(sw, sh);
            P1Arrow.rectTransform.position = new Vector2(x, y + sh / 8);
            if (indexP1 > 5)
            {
                P1Arrow.rectTransform.sizeDelta = SizeSmall;
                P1Arrow.rectTransform.position = new Vector2(x, y+18);
            }
            else
                P1Arrow.rectTransform.sizeDelta = SizeLarge;

            if (indexP1 == 0)
            {
                P1CHAR.text = "THORNTON";
                P1CHARBG.text = "THORNTON";
            }
            else if (indexP1 == 1)
            {
                P1CHAR.text = "KLEFSTAD";
                P1CHARBG.text = "KLEFSTAD";
            }
            else if (indexP1 == 2)
            {
                P1CHAR.text = "PATTIS";
                P1CHARBG.text = "PATTIS";
            }
            else if (indexP1 == 3)
            {
                P1CHAR.text = "SHINDLER";
                P1CHARBG.text = "SHINDLER";
            }
            else if (indexP1 == 4)
            {
                P1CHAR.text = "WONG MA";
                P1CHARBG.text = "WONG MA";
            }
            else if (indexP1 == 5)
            {
                P1CHAR.text = "NAVARRO";
                P1CHARBG.text = "NAVARRO";
            }
            else if (indexP1 == 6)
            {
                P1CHAR.text = "OPTIONS";
                P1CHARBG.text = "OPTIONS";
            }
            else if (indexP1 == 7)
            {
                P1CHAR.text = "ZOTFIGHTER";
                P1CHARBG.text = "ZOTFIGHTER";
            }
            else if (indexP1 == 8)
            {
                P1CHAR.text = "HEALTH";
                P1CHARBG.text = "HEALTH";
            }
            else if (indexP1 == 9)
            {
                P1CHAR.text = "CONTROLS";
                P1CHARBG.text = "CONTROLS";
            }
            else if (indexP1 == 10)
            {
                P1CHAR.text = "SPECIALS";
                P1CHARBG.text = "SPECIALS";
            }
            else if (indexP1 == 11)
            {
                P1CHAR.text = "SUDDEN DEATH";
                P1CHARBG.text = "SUDDEN DEATH";
            }
        }
    }

    //F L A S H T E X T
    IEnumerator FlashText(TextMeshProUGUI T, TextMeshProUGUI T2)
    {
        Color tmp = P1Arrow.color;
        tmp.a = 0f;
        while (true)
        {
            if (ReadyP1 == true)
            {
                yield return new WaitForSeconds(0.025f);
                T.enabled = !T.enabled;
                T2.enabled = !T2.enabled;
                tmp.a = Mathf.Abs(tmp.a - 255f);
                P1Arrow.color = tmp;
            }
            else
            {
                tmp.a = 255f;
                P1Arrow.color = tmp;
                T.enabled = true;
                T2.enabled = true;
            }
            yield return null;
        }
    }
    //F L A S H T E X T
    IEnumerator FlashTextP2(TextMeshProUGUI T, TextMeshProUGUI T2)
    {
        Color tmp = P2Arrow.color;
        tmp.a = 0f;
        while (true)
        {
            if (ReadyP2 == true)
            {
                yield return new WaitForSeconds(0.025f);
                T.enabled = !T.enabled;
                T2.enabled = !T2.enabled;
                tmp.a = Mathf.Abs(tmp.a - 255f);
                P2Arrow.color = tmp;
            }
            else
            {
                tmp.a = 255f;
                P2Arrow.color = tmp;
                T.enabled = true;
                T2.enabled = true;
            }
            yield return null;
        }
    }

    //Wait? idk what this is for
    IEnumerator Wait()
    {
        SceneManager.LoadScene(7);
        yield return null;
    }

}

