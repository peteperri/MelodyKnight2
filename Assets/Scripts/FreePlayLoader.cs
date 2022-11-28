using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayLoader : MonoBehaviour
{
    public static GameObject[] freePlaySongs;
    [SerializeField] private GameObject template;
    [SerializeField] private Transform scrollBarContent;
    private MenuSystem menuSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        freePlaySongs = Resources.LoadAll<GameObject>("FreePlaySongs");
        float yPositionOffset = 0;
        menuSystem = FindObjectOfType<MenuSystem>();
        foreach (var songObject in freePlaySongs)
        {
            FreePlaySong freePlaySong = songObject.GetComponent<FreePlaySong>();
            GameObject songSelector = Instantiate(template, scrollBarContent);
            songSelector.GetComponent<RectTransform>().anchoredPosition = new Vector3(4.5f, 1508 + yPositionOffset);
            songSelector.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = freePlaySong.Title;
            songSelector.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = freePlaySong.Artist;
            songSelector.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = freePlaySong.BPM.ToString();
            songSelector.GetComponent<Button>().onClick.AddListener(() => menuSystem.FreePlaySongSelect(songObject));
            yPositionOffset -= 150;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
