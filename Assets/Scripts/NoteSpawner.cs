using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class NoteSpawner : MonoBehaviour
{
    /*[SerializeField]*/private float noteSpeedMultiplier;
    /*[SerializeField]*/ private float noteSpawnsPerBeat;
    private bool spawnSwipeNotes;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private GameObject tapNotePrefab;
    [SerializeField] private GameObject swipeNotePrefab;
    
    
    private GameObject[] noteSelect;
    private Transform[] sideSelect;
    private int side;
    private int noteType;
    private bool canSpawn;
    private StatHandler player;
    private Conductor conductor;
    private float _spawnSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        noteSpeedMultiplier = MenuSystem.noteSpeedMultiplier;
        noteSpawnsPerBeat = MenuSystem.noteSpawnsPerBeat;
        spawnSwipeNotes = MenuSystem.spawnSwipeNotes;
        player = FindObjectOfType<StatHandler>();
        side = 0;
        canSpawn = true;
        conductor = FindObjectOfType<Conductor>();
        //SwipeNote.gameObject.GetComponent<SwipeNote>().speed = noteSpeed;

        //Fill Arrays
        noteSelect = new GameObject[2];
        sideSelect = new Transform[2];
        
        noteSelect[0] = tapNotePrefab;
        noteSelect[1] = swipeNotePrefab;

        sideSelect[0] = leftSpawnPoint;
        sideSelect[1] = rightSpawnPoint;
        
    }

    // Update is called once per frame
    void Update()
    {
        //doing this in update because doing it in start results in it being 0 for some reason
        _spawnSpeed = conductor.SecPerBeat * 2; 
        if (canSpawn && player.IsAlive && !conductor.SongOver)
        {
            StartCoroutine(SpawnNote());
        }
    }

    IEnumerator SpawnNote()
    {
        canSpawn = false;

        //0 for left, 1 for right, 2 for both
        side = Random.Range(0, 3);
        noteType = Random.Range(1, 21);
        
        //Debug.Log(side);
        if (side == 2)
        {
            /*if (noteType < 15)
                SpawnTwoTapNotes();
            else 
            {
                SpawnTwoSwipeNotes();
            }*/

            if (noteType > 15 && spawnSwipeNotes == true)
            {
                SpawnTwoSwipeNotes();
            }
            else
            {
                SpawnTwoTapNotes();
            }

        }
        else
        {
            /*if (noteType < 15)
                SpawnOneTapNote();
            else
                SpawnOneSwipeNote(side);
            */

            if (noteType > 15 && spawnSwipeNotes == true)
            {
                SpawnOneSwipeNote(side);
            } else
            {
                SpawnOneTapNote();
            }
        }

        yield return new WaitForSeconds(_spawnSpeed * noteSpawnsPerBeat);
        canSpawn = true;
    }

    private void SpawnTwoTapNotes() 
    {
        TapNote leftNote = Instantiate(tapNotePrefab, new Vector2(sideSelect[0].position.x, sideSelect[0].position.y), Quaternion.identity).GetComponent<TapNote>();
        TapNote rightNote = Instantiate(tapNotePrefab, new Vector2(sideSelect[1].position.x, sideSelect[1].position.y), Quaternion.identity).GetComponent<TapNote>();
        leftNote.speed = _spawnSpeed * noteSpeedMultiplier;
        rightNote.speed = _spawnSpeed * noteSpeedMultiplier;
        rightNote.SetPartner(leftNote);
        leftNote.SetPartner(rightNote);
        leftNote.side = "Left";
        rightNote.side = "Right";
    }
    

    private void SpawnOneTapNote() 
    {
        GameObject note = Instantiate(tapNotePrefab, new Vector2(sideSelect[side].position.x, sideSelect[side].position.y), Quaternion.identity);
        TapNote tapNote = note.GetComponent<TapNote>();
        tapNote.speed = _spawnSpeed * noteSpeedMultiplier;
        if (side.Equals(0))
        {
            tapNote.side = "Left";
        }
        else
        {
            tapNote.side = "Right";
        }
    }

    private void SpawnOneSwipeNote(int side) {
        SwipeNote note = Instantiate(swipeNotePrefab, new Vector2(sideSelect[side].position.x, sideSelect[side].position.y), Quaternion.identity).GetComponent<SwipeNote>();
        note.speed = _spawnSpeed * noteSpeedMultiplier;
        int upDown = Random.Range(0, 2);
        if (upDown == 0) 
        {
            note.SwipeDir = "Up";
        }
        if (upDown == 1)
        {
            note.SwipeDir = "Down"; 
        }

        if (side == 0)
        {
            note.side = "Left";
        }
        else if (side == 1) 
        {
            note.side = "Right";
        }
    }

    private void SpawnTwoSwipeNotes()
    {
        SwipeNote note1 = Instantiate(swipeNotePrefab, new Vector2(sideSelect[0].position.x, sideSelect[0].position.y), Quaternion.identity).GetComponent<SwipeNote>();
        SwipeNote note2 = Instantiate(swipeNotePrefab, new Vector2(sideSelect[1].position.x, sideSelect[1].position.y), Quaternion.identity).GetComponent<SwipeNote>();
        note1.SetPartner(note2);
        note2.SetPartner(note1);
        note1.speed = _spawnSpeed * noteSpeedMultiplier;
        note2.speed = _spawnSpeed * noteSpeedMultiplier;
        note1.side = "Left";
        note2.side = "Right";
        int upDown1 = Random.Range(0, 2);
        int upDown2 = Random.Range(0, 2);

        if (upDown1 == 0)
        {
            note1.SwipeDir = "Up";
            note2.SwipeDir = "Up";
        }
        else
        {
            note1.SwipeDir = "Down";
            note2.SwipeDir = "Down";
        }
        
        /*if (upDown2 == 0)
        {
            note2.SwipeDir = "Up";
        }
        else
        {
            note2.SwipeDir = "Down";
        }*/

    }

}
