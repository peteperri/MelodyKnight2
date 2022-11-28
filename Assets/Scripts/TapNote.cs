
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TapNote : MonoBehaviour
{
    public float speed;
    protected bool canHit;
    [SerializeField] protected GameObject deathEffect;
    [SerializeField] protected GameObject deathSprinkle;

    //Side of the screen the note is on ("Left" or "Right")
    public string side;

    protected StatHandler player;
    protected Transform leftStart;
    protected Transform rightStart;
    protected Vector2 playerPos;

    //Goal Check
    protected bool hasHitGoal;
    protected GameObject leftGoal;
    protected GameObject rightGoal;
    protected Vector2 leftPos;
    protected Vector2 rightPos;

    protected bool hasPartner = false;
    protected TapNote partner;

    //Animation check
    protected Animator anim;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        canHit = false;
        player = FindObjectOfType<StatHandler>();
        leftStart = GameObject.FindGameObjectWithTag("LeftSpawnPoint").transform;
        rightStart = GameObject.FindGameObjectWithTag("RightSpawnPoint").transform;
        playerPos = player.gameObject.transform.position;

        //Store Goal Info
        leftGoal = GameObject.Find("LeftNoteGoal");
        rightGoal = GameObject.Find("RightNoteGoal");
        leftPos = leftGoal.gameObject.transform.position;
        rightPos = rightGoal.gameObject.transform.position;

        if (side == "Right")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        hasHitGoal = false;

        anim = GetComponent<Animator>();
        //Change code to reflect scene names when changed
        if (GameObject.Find("ForestBackground"))
        {
            anim.SetBool("Forest", true);
        } else if (GameObject.Find("FireBackground")) 
        {
            anim.SetBool("Fire", true);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Determine Goal to move toward
        if (side == "Right" && hasHitGoal == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPos, speed * Time.deltaTime);
        } else if (side == "Left" && hasHitGoal == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
        } else
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        }
        
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            
                if (canHit == true && touchPosition.x < 0 && side == "Left")
                {
                    HitNote();
                } else if (canHit == true && touchPosition.x > 0 && side == "Right")
                {
                    HitNote();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"Left Button Press. CanHit: {canHit} Side: {side}");
            if (canHit && side == "Left")
            {
                HitNote();
            } 
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log($"Right Button Press. CanHit: {canHit} Side: {side}");
            if (canHit && side == "Right")
            {
                HitNote();
            } 
        }
    }

    public void HitNote()
    {
        int scoreToAdd = 1 * player.currentCombo;
        if (scoreToAdd == 0)
        {
            scoreToAdd = 1;
        }

        if (hasPartner && side == "Left")
        {
            player.AddScore(scoreToAdd);
        }
        else if (!hasPartner)
        {
            player.AddScore(scoreToAdd);
        }
        
        player.enemiesCanHit.Remove(this.gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Instantiate(deathSprinkle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "LeftGoal" ||  collision.gameObject.tag == "RightGoal")
        {
            canHit = true;
            hasHitGoal = true;
            player.enemiesCanHit.Add(this.gameObject);
        }
    }

    public void SetPartner(TapNote anotherNote)
    {
        hasPartner = true;
        partner = anotherNote;
    }

    public bool GetHasPartner()
    {
        return hasPartner;
    }

    public TapNote GetPartner()
    {
        return partner;
    }

    public bool PartnerNull()
    {
        if (partner == null)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
