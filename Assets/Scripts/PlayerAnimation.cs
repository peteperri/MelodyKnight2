using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    StatHandler statHandler;
    Vector2 startPos;
    Vector2 touchPosition;
    bool canPlaySlash = false;
    Touch touch;
    bool hit = false;
    bool cooldown = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        statHandler = GetComponent<StatHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0 && hit == false && cooldown == false)
        {
         if (MenuSystem.difficultySet != "Hard")
                Idle();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touch = Input.GetTouch(0);
            startPos = Camera.main.ScreenToWorldPoint(touch.position);
            canPlaySlash = true;

            if (Input.touchCount == 1)
            {
                Swing();
            }
            else if (Input.touchCount >= 2)
            {
                Dual();
            }

            //Flip direction
            for (int i = 0; i < Input.touchCount; i++)
            {

                if (startPos.x < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (startPos.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        } else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            canPlaySlash = false;
            StartCoroutine(CoolDown());
        } else if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (Input.touchCount >= 2)
            {
                Dual(); 
            } else if (canPlaySlash == true)
            {
                if (touchPosition.y > startPos.y + 0.1)
                {
                    UpSlash();
                    canPlaySlash = false;
                }
                else if (touchPosition.y < startPos.y - 0.1)
                {
                    DownSlash();
                    canPlaySlash = false;
                }
            }
            
        }

        //Keyboard Controls
        /*if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                UpSlash();
            } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                DownSlash();
            } else {
                SingleTap();
            }
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<SpriteRenderer>().flipX = false;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                UpSlash();
            } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                DownSlash();
            } else {
                SingleTap();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            Dual();
        } 
        
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }*/

        
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        Debug.Log("Enemy hit player");
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(Stagger());
        }
    }

    IEnumerator Stagger()
    {
        Debug.Log("Stagger Begun");
        hit = true;
        anim.CrossFade("Allegro Hit", 0, 0);
        yield return new WaitForSeconds(0.3f);
        hit = false;
    }

    IEnumerator CoolDown()
    {
        cooldown = true;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }

    //Animations

    void Idle()
    {
        anim.CrossFade("Allegro Idle", 0, 0);
    }

    void Swing()
    {
        anim.CrossFade("Allegro Swing", 0, 0);
        anim.Play("Allegro Swing", -1, 0f);
    }

    void Dual()
    {
        anim.CrossFade("Allegro DualAttack", 0, 0);
        anim.Play("Allegro DualAttack", -1, 0f);
    }

    void UpSlash()
    {
        anim.CrossFade("Allegro UpSlash", 0, 0);
        anim.Play("Allegro UpSlash", -1, 0f);
    }

    void DownSlash()
    {
        anim.CrossFade("Allegro DownSlash", 0, 0);
        anim.Play("Allegro DownSlash", -1, 0f);
    }


}
