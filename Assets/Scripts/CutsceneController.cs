using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(WaitForCutsceneEnd());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    IEnumerator WaitForCutsceneEnd()
    {
        yield return new WaitForSeconds((float) videoPlayer.clip.length);
    }
}
