using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    public int lifeSpanDuration = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LifeSpan");
    }

    IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifeSpanDuration);
        Destroy(gameObject);
    }
}
