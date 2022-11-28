using System;
using UnityEngine;
using UnityEngine.UI;
public class EndlessConductor : Conductor
{
    private FreePlaySong currentSong;
    private FreePlaySong[] allSongs;
    private int playlistIndex;
    private bool nextSongInitiated;
    
    private new void Start()
    {
        musicSource = GetComponent<AudioSource>();
        Debug.Log(FreePlayLoader.freePlaySongs is null);
        allSongs = new FreePlaySong[FreePlayLoader.freePlaySongs.Length];
        SetBackground();

        int i = 0;
        foreach (GameObject song in FreePlayLoader.freePlaySongs)
        {
            allSongs[i] = song.GetComponent<FreePlaySong>();
            i++;
        }
        Debug.Log(allSongs.Length);

        Shuffle(allSongs);
        songBpm = allSongs[0].BPM;
        firstBeatOffset = allSongs[0].LeadTime;
        musicSource.clip = allSongs[0].Song;
        musicSource.volume = allSongs[0].volume;

        SecPerBeat = (60f) / songBpm;
        dspSongTime = (float) AudioSettings.dspTime;
        songDuration = musicSource.clip.length;
        musicSource.Play();
    }

    private new void Update()
    {
        base.Update();
        if (SongOver && !nextSongInitiated)
        {
            musicSource.Pause();
            nextSongInitiated = true;
            NextSong();
        }
    }

    private void NextSong()
    {
        playlistIndex++;
        if (playlistIndex >= allSongs.Length)
        {
            playlistIndex = 0;
        }
        songPosition = 0;
        songBpm = allSongs[playlistIndex].BPM;
        firstBeatOffset = allSongs[playlistIndex].LeadTime;
        musicSource.clip = allSongs[playlistIndex].Song;
        musicSource.volume = allSongs[playlistIndex].volume;

        SecPerBeat = (60f) / songBpm;
        dspSongTime = (float) AudioSettings.dspTime;
        songDuration = musicSource.clip.length;
        SongOver = false;
        nextSongInitiated = false;
        
        musicSource.Play();
    }
    
    private static void Shuffle (FreePlaySong[] array)
    {
        var rng = new System.Random();
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            FreePlaySong temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
