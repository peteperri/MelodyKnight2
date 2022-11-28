using System;
using UnityEngine;
using UnityEngine.UI;

public class Conductor : MonoBehaviour
{
    [SerializeField] protected float songBpm;
    [SerializeField] protected float firstBeatOffset;
    public float SecPerBeat { get; protected set; }
    public bool SongOver { get; protected set; }
    protected float songPosition;
    protected float dspSongTime;
    protected AudioSource musicSource;
    protected float songDuration;
    [SerializeField] private GameObject castleLevel;
    [SerializeField] private GameObject forestLevel;
    [SerializeField] private GameObject fireLevel;
    

    //Difficulty integer
    // Start is called before the first frame update
    public virtual void Start()
    {
        musicSource = GetComponent<AudioSource>();

        if (MenuSystem.freePlaySongToPlay == null)
            Debug.Log("Free Play Song is Null");
        else 
            Debug.Log("Free Play Song is Not Null");
        
        if (MenuSystem.freePlaySongToPlay != null)
        {
            
            SetBackground();
            musicSource.clip = MenuSystem.freePlaySongToPlay.Song;
            songBpm = MenuSystem.freePlaySongToPlay.BPM;
            firstBeatOffset = MenuSystem.freePlaySongToPlay.LeadTime;
            musicSource.volume = MenuSystem.freePlaySongToPlay.volume;
        }
        
        SecPerBeat = (60f) / songBpm;
        
        dspSongTime = (float) AudioSettings.dspTime;
        musicSource.Play();
        songDuration = musicSource.clip.length;

        
    }


    protected void SetBackground()
    {
        if (MenuSystem.freePlayBG == "Castle")
        {
            castleLevel.SetActive(true);
        }
            
        if (MenuSystem.freePlayBG == "Forest")
        {
            forestLevel.SetActive(true);
        }
            
        if (MenuSystem.freePlayBG == "Fire")
        {
            fireLevel.SetActive(true);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        songPosition = (float) AudioSettings.dspTime - dspSongTime - firstBeatOffset;
        if (songPosition >= songDuration)
        {
            SongOver = true;
        }
    }

    public void PauseSong(bool paused)
    {
        if(paused)
            musicSource.Pause();
        else
            musicSource.Play();
    }
}
