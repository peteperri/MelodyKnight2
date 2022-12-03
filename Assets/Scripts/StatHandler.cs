using System;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StatHandler : MonoBehaviour
{
    //[SerializeField] private int startingHealth = 5;
    [SerializeField] private AudioClip healthBonus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip beatSound;
    [SerializeField] private bool forcedWin;
    [SerializeField] private float speed = 3.0f;
    private int currentScore;
    public int currentCombo { private set; get; }
    private float highCombo;
    private float currentHealth;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI currentComboText;
    private TextMeshProUGUI highComboText;
    private Conductor conductor;
    private TextMeshProUGUI resultText;
    private AudioSource _beatSource;
    private ScreenShake screenShaker;

    //Added to track currently spawned enemies
    public List<GameObject> enemiesCanHit = new List<GameObject>();

    public bool IsAlive { get; private set; } = true;

    //Menu buttons
    [SerializeField] GameObject buttons;

    private bool canLoseCombo;
    private bool canTakeDamage = true;

    [SerializeField] private GameObject healthUpEffect;
    [SerializeField] private GameObject comboEffect;
    //[SerializeField] private ParticleSystem comboParticles;
    [SerializeField] private int comboActivateNum;
    //[SerializeField] private GameObject healthUpSprinkle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = startingHealth;
        currentHealth = MenuSystem.startingHealth;
        healthText = GameObject.FindWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        resultText = GameObject.FindWithTag("ResultText").GetComponent<TextMeshProUGUI>();
        currentComboText = GameObject.FindWithTag("CurrentComboText").GetComponent<TextMeshProUGUI>();
        highComboText = GameObject.FindWithTag("HighComboText").GetComponent<TextMeshProUGUI>();
        buttons = GameObject.FindWithTag("GameOver");
        buttons.SetActive(false);
        healthText.text = $"{currentHealth}";
        conductor = FindObjectOfType<Conductor>();
        _beatSource = GetComponent<AudioSource>();

        try
        {
            screenShaker = Camera.main.GetComponent<ScreenShake>();
        }
        catch (Exception e)
        {
            
        }
    }

    public void ChangeHealth(float healthChangeAmount)
    {
        if (healthChangeAmount > 0)
        {
            _beatSource.PlayOneShot(healthBonus);
            Instantiate(healthUpEffect);

            
            //Instantiate(healthUpSprinkle);
        }
        else if(healthChangeAmount < 0)
        {
            _beatSource.PlayOneShot(damageSound);
        }

        if((healthChangeAmount < 0 && canTakeDamage) || healthChangeAmount > 0)
            currentHealth += healthChangeAmount;
        
        //currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        if (currentHealth <= 0)
        {
            IsAlive = false;
            currentHealth = 0;
            //Activate menu buttons
            //buttons.SetActive(true);
        }

        if (healthChangeAmount < 0)
        {
            canTakeDamage = false;
            StartCoroutine(SetCanTakeDamage());
        }
        
        healthText.text = $"{currentHealth}";
        
    }

    public void AddScore(int scoreToAdd)
    {
        //if(_beatSource!=null)
            //_beatSource.PlayOneShot(beatSound);
        screenShaker.TriggerShake();
        currentScore += scoreToAdd;
        scoreText.text = $"{currentScore}";
        
        currentCombo += 1;

        if (currentCombo % 15 == 0)
        {
            ChangeHealth(1);
            currentComboText.GetComponent<ScreenShake>().TriggerShake();
            //Debug.Log($"Health Bonus! {currentCombo} % 20 = {currentCombo%20}");
            _beatSource.PlayOneShot(healthBonus);
        }
        else
        {
            //Debug.Log($"No Health Bonus. {currentCombo} % 20 = {currentCombo%20}");
        }


        if (currentCombo > highCombo)
        {
            highCombo = currentCombo;
        }
        
        currentComboText.text = $"{currentCombo}";
        //highComboText.text = $"Best Combo: {highCombo}";
        canLoseCombo = false;
        StartCoroutine(SetCanLoseComboTrue());
    }

    public void EndCombo()
    {
        currentCombo = 0;
        currentComboText.text = $"{currentCombo}";
        comboEffect.SetActive(false);
    }

    public void Update()
    {
        if ((IsAlive && conductor.SongOver) || forcedWin)
        {
            StartCoroutine(WaitAndGoToNextLevel());
        }
        else if (!IsAlive)
        {
            resultText.text = "You lose!";
            conductor.gameObject.GetComponent<AudioSource>().Stop();
            StartCoroutine(TurnOnMenu(false));
        }

        //Track Combo
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && canLoseCombo))
        {
            if (enemiesCanHit.Count == 0)
            {
                EndCombo();
            } 
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (enemiesCanHit.Count == 0 && canLoseCombo)
            {
                EndCombo();
            } 
        }

        if (currentCombo == comboActivateNum)
        {
            comboEffect.SetActive(true);
            //comboParticles.Play();
            //Debug.Log("Combo Turn On");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            
            TapNote note = other.gameObject.GetComponent<TapNote>();
            if (note.GetHasPartner() && !note.PartnerNull())
            {
                EndCombo();
                enemiesCanHit.Remove(other.gameObject);
                enemiesCanHit.Remove(note.GetPartner().gameObject);
                Destroy(note.GetPartner().gameObject);
                Destroy(other.gameObject);
            }
            else
            {
                EndCombo();
                enemiesCanHit.Remove(other.gameObject);
                Destroy(other.gameObject);
            }
            ChangeHealth(-1);


        }
    }

    IEnumerator TurnOnMenu(bool wait)
    {
        if(wait)
            yield return new WaitForSeconds(2f);
        buttons.SetActive(true);
        highComboText.text = $"Best Combo: {highCombo}";
            
    }

    IEnumerator WaitAndGoToNextLevel()
    {
        yield return new WaitForSeconds(2f);
        resultText.text = "You win!";
        if (MenuSystem.freePlaySongToPlay == null && !SceneManager.GetActiveScene().name.Equals("DLC") && !SceneManager.GetActiveScene().name.Equals("EndlessMode"))
        {
            GetComponent<Animator>().CrossFade("Allegro Run", 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(10.5f, transform.position.y, transform.position.z), speed * Time.deltaTime);
            if (transform.position.x >= 10.0f)
            {
                if (SceneManager.GetActiveScene().name.Equals("Castle"))
                {
                    MenuSystem.level1Beaten = true;
                    SceneManager.LoadScene("Forest");
                }
                else if (SceneManager.GetActiveScene().name.Equals("Forest"))
                {
                    MenuSystem.level2Beaten = true;
                    SceneManager.LoadScene("Fire");
                } else if (SceneManager.GetActiveScene().name.Equals("Fire"))
                {
                    MenuSystem.level3Beaten = true;
                    SceneManager.LoadScene("DLC");
                }
            }
        }
        else
        {
            StartCoroutine(TurnOnMenu(true));
        }
            
        if (SceneManager.GetActiveScene().name.Equals("Castle"))
        {
            MenuSystem.level1Beaten = true;
        }
            
        if (SceneManager.GetActiveScene().name.Equals("Forest"))
        {
            MenuSystem.level2Beaten = true;
        }

        if (SceneManager.GetActiveScene().name.Equals("Fire"))
        {
            MenuSystem.level3Beaten = true;
        }
    }

    IEnumerator SetCanLoseComboTrue()
    {
        yield return new WaitForSeconds(0.3f);
        canLoseCombo = true;
    }

    IEnumerator SetCanTakeDamage()
    {
        yield return new WaitForSeconds(0.2f);
        canTakeDamage = true;
    }

}
