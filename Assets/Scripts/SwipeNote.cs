using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwipeNote : TapNote
{

    public string SwipeDir { get; set; }
    public RawImage swipeIcon;
    public Texture2D swipeUpIcon;
    public Texture2D swipeDownIcon;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private TextMeshProUGUI detailText;
    private Animator anim;
    private GameObject player;

    new void Start() 
    {
        base.Start();
        detailText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();

        SetSceneSprite();

        player = GameObject.Find("Sir Allegro");
    }

    new void Update() 
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
            //Debug.Log(startTouchPosition.y);
        }

        //End of Swipe
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            //Checks if player swiped up *enough* if note is an Up note
            if (startTouchPosition.y <= endTouchPosition.y - 10 && canHit == true && SwipeDir == "Up")
            { 
                HitNote();
                //player.GetComponent<PlayerAnimation>().UpSlash();

            } //Checks if player swiped down *enough* if note is a Down note 
            else if (startTouchPosition.y >= endTouchPosition.y + 10 && canHit == true && SwipeDir == "Down")
            {
                HitNote();
                //player.GetComponent<PlayerAnimation>().DownSlash();
            }
        }

        if (((Input.GetKeyDown(KeyCode.UpArrow) && side == "Right" && SwipeDir == "Up")
             || (Input.GetKey(KeyCode.DownArrow) && side == "Right" && SwipeDir == "Down")
             || (Input.GetKeyDown(KeyCode.W) && side == "Left" && SwipeDir == "Up")
             || (Input.GetKeyDown(KeyCode.S) && side == "Left" && SwipeDir == "Down")) && canHit)
        {
            HitNote();
        }

    }

    private void SwipeCheck()
    {
        if (canHit && side == "Left" && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HitNote();
        }

        if (canHit && side == "Right" && Input.GetKeyDown(KeyCode.RightArrow))
        {
            HitNote();
        }
    }

    private void SetSceneSprite()
    {
        if (SwipeDir == "Up")
        {
            swipeIcon.texture = swipeUpIcon;
        }
        if (SwipeDir == "Down")
        {
            swipeIcon.texture = swipeDownIcon;
            anim.SetBool("Down", true);
        }

        if (GameObject.Find("FireBackground"))
        {
            anim.SetBool("Fire", true);
        } else if (GameObject.Find("ForestBackground"))
        {
            anim.SetBool("Forest", true);
        }
    }
}
