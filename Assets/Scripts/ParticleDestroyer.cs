using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float killTime = 1.0f;
    void Start()
    {
        Invoke("KillMe", killTime);
    }

    void KillMe()
    {
        Destroy(gameObject);
    }


}
