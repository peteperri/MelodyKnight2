using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    public static float noteSpeedMultiplier = 8f;
    public static float noteSpawnsPerBeat = 1f;
    public static bool spawnSwipeNotes;
    public static int startingHealth;
    public static bool isEndless = false;
    public static string difficultySet; 
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject modeSelectScreen;
    [SerializeField] GameObject difficultyScreen;
    [SerializeField] GameObject songSelectScreen;
    [SerializeField] GameObject FreeplayModeScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] GameObject comboCanvas;
    [SerializeField] GameObject howToPlayScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] Sprite[] comboImage;

    //For Pause Menu
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameUI;

    //Level Select
    [SerializeField] GameObject Level1Button;
    [SerializeField] GameObject Level2Button;
    [SerializeField] GameObject Level3Button;
    [SerializeField] GameObject Level4Button;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;

    private String _sceneToLoad;

    private bool isPaused;

    //private bool story;
    public static bool level1Beaten;
    public static bool level2Beaten;
    public static bool level3Beaten;
    public static FreePlaySong freePlaySongToPlay;
    public static string freePlayBG;
    private static bool cutScenePlayed;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            noteSpeedMultiplier = 8f;
            noteSpawnsPerBeat = 1f;
            spawnSwipeNotes = true;
            startingHealth = 5;
        }
        else
        {
            isPaused = false;
        }

        if (GameObject.Find("ForestBackground"))
        {
            comboCanvas.GetComponent<Image>().sprite = comboImage[1];
        }

        if (GameObject.Find("FireBackground"))
        {
            comboCanvas.GetComponent<Image>().sprite = comboImage[2];
        }

        if (!level1Beaten && !level2Beaten)
        {
            LeftArrow.SetActive(false);
            RightArrow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            level1Beaten = true;
            level2Beaten = true;
            level3Beaten = true;
            LeftArrow.SetActive(true);
            RightArrow.SetActive(true);
        }
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("SampleScene");
        //Time.timeScale = 1f;
        titleScreen.SetActive(false);
        modeSelectScreen.SetActive(true);
        isEndless = false;
    }

    public void StoryMode()
    {
        //SceneManager.LoadScene("SampleScene");
        //Time.timeScale = 1f;
        modeSelectScreen.SetActive(false);
        songSelectScreen.SetActive(true);
        isEndless = false;

    }

    public void EndlessMode()
    {
        //SceneManager.LoadScene("SampleScene");
        //Time.timeScale = 1f;
        modeSelectScreen.SetActive(false);
        ToDifficulty();
        isEndless = true;
    }

    public void FreeplayMode()
    {
        //SceneManager.LoadScene("SampleScene");
        //Time.timeScale = 1f;
        modeSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(true);
        isEndless = false;
    }

    public void SongSelect(string songName)
    {
        _sceneToLoad = songName;
        //Uncomment for final build when Cutscene is re-implemented
        /*if (songName == "Castle" && !cutScenePlayed)
        {
            _sceneToLoad = "OpeningCutscene";
            cutScenePlayed = true;
        }*/
        freePlaySongToPlay = null;
        isEndless = false;
        ToDifficulty();
    }

    public void FreePlaySongSelect(GameObject song)
    {
        freePlaySongToPlay = song.GetComponent<FreePlaySong>();
        _sceneToLoad = "Free Play";
        isEndless = false;
        ToDifficulty();
    }


    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1f;
    }

    public void ToDifficulty()
    {
        songSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(false);
        difficultyScreen.SetActive(true);
    }

    public void ToOptions()
    {
        titleScreen.SetActive(false);
        modeSelectScreen.SetActive(false);
        optionsScreen.SetActive(true);

        GameObject swipeToggle = GameObject.Find("SwipeToggle");
        if (spawnSwipeNotes == true)
        {
            swipeToggle.GetComponent<Toggle>().isOn = true;
        } else
        {
            swipeToggle.GetComponent<Toggle>().isOn = false;
        }
        healthText.text = ($"{startingHealth}");
    }

    public void BackToTitle()
    {
        difficultyScreen.SetActive(false);
        optionsScreen.SetActive(false);
        songSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        modeSelectScreen.SetActive(true);
        creditsScreen.SetActive(false);
        freePlaySongToPlay = null;
    }
    
    public void AllTheWayBackToTitle()
    {
        difficultyScreen.SetActive(false);
        optionsScreen.SetActive(false);
        songSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(false);
        modeSelectScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        creditsScreen.SetActive(false);
        titleScreen.SetActive(true);
        freePlaySongToPlay = null;
    }

    public void HowToPlay()
    {
        difficultyScreen.SetActive(false);
        optionsScreen.SetActive(false);
        songSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(false);
        modeSelectScreen.SetActive(false);
        howToPlayScreen.SetActive(true);
        titleScreen.SetActive(false);
        creditsScreen.SetActive(false);
        freePlaySongToPlay = null;
    }

    public void Credits()
    {
        difficultyScreen.SetActive(false);
        optionsScreen.SetActive(false);
        songSelectScreen.SetActive(false);
        FreeplayModeScreen.SetActive(false);
        modeSelectScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        titleScreen.SetActive(false);
        creditsScreen.SetActive(true);
        freePlaySongToPlay = null;
    }

    public void ChangeDifficulty(string difficultyLevel)
    {
        if (difficultyLevel == "Easy")
        {
            noteSpeedMultiplier = 6f;
            noteSpawnsPerBeat = 2f;
            Debug.Log("Difficulty changed to Easy");
            freePlayBG = "Castle";
        }

        if (difficultyLevel == "Medium")
        {
            noteSpeedMultiplier = 8f;
            noteSpawnsPerBeat = 1f;
            Debug.Log("Difficulty changed to Medium");
            freePlayBG = "Forest";
        }

        if (difficultyLevel == "Hard")
        {
            noteSpeedMultiplier = 12f;
            noteSpawnsPerBeat = 0.5f;
            Debug.Log("Difficulty changed to Hard");
            freePlayBG = "Fire";
        }

        if (isEndless)
        {
            _sceneToLoad = "EndlessMode";
        }
        difficultySet = difficultyLevel;
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void NoteSelection(string selection)
    {
        if (selection == "Swipe")
        {
            if (spawnSwipeNotes == true)
            {
                spawnSwipeNotes = false;
            } else
            {
                spawnSwipeNotes = true;
            }
        }
    }

    public void AddToHealth()
    {
        if (startingHealth != 100)
        {
            startingHealth++;
            healthText.text = ($"{startingHealth}");
        }
    }

    public void SubstractFromHealth()
    {
        if (startingHealth != 1)
        {
            startingHealth--;
            healthText.text = ($"{startingHealth}");
        }
    }

    public void Pause()
    {
        Conductor conductor = FindObjectOfType<Conductor>();
        if (isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0f;
            //GameObject.Find("")
            pauseMenu.SetActive(true);
            gameUI.SetActive(false);
            conductor.PauseSong(true);
        } else if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1f;
            gameUI.SetActive(true);
            pauseMenu.SetActive(false);
            conductor.PauseSong(false);
        }
    }

    public void LevelSelectForward()
    {
        Debug.Log ("Clicked");
        if (Level1Button.activeInHierarchy && level1Beaten)
        {
            Level1Button.SetActive(false);
            Level2Button.SetActive(true);
        } 
        else if (Level2Button.activeInHierarchy && level2Beaten)
        {
            Level2Button.SetActive(false);
            Level3Button.SetActive(true);
        } 
        else if (Level2Button.activeInHierarchy && !level2Beaten)
        {
            Level2Button.SetActive(false);
            Level1Button.SetActive(true);
        }
        else if (Level3Button.activeInHierarchy && !level3Beaten)
        {
            Level3Button.SetActive(false);
            Level1Button.SetActive(true);
        } else if (Level3Button.activeInHierarchy && level3Beaten)
        {
            Level3Button.SetActive(false);
            Level4Button.SetActive(true);
        } else if (Level4Button.activeInHierarchy)
        {
            Level4Button.SetActive(false);
            Level1Button.SetActive(true);
        }
    }

    public void LevelSelectBackward()
    {
        if (Level1Button.activeInHierarchy && level2Beaten && !level3Beaten)
        {
            Level1Button.SetActive(false);
            Level3Button.SetActive(true);
        } 
        else if (Level1Button.activeInHierarchy && level2Beaten && level3Beaten)
        {
            Level1Button.SetActive(false);
            Level4Button.SetActive(true);
        }
        else if (Level1Button.activeInHierarchy && !level2Beaten)
        {
            Level1Button.SetActive(false);
            Level2Button.SetActive(true); 
        }
        else if (Level2Button.activeInHierarchy)
        {
            Level2Button.SetActive(false);
            Level1Button.SetActive(true);
        } 
        else if (Level3Button.activeInHierarchy)
        {
            Level3Button.SetActive(false);
            Level2Button.SetActive(true);
        }
        else if (Level4Button.activeInHierarchy)
        {
            Level4Button.SetActive(false);
            Level3Button.SetActive(true);
        }
    }
}

