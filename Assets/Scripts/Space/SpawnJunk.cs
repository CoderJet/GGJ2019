using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJunk : MonoBehaviour
{
    public GameObject[] junkObjects;
    public float SpawnChance = 2f;
    public GravityAttractor attractor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float currentChanceOfSpawn = 0;

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, SpawnChance) < currentChanceOfSpawn)
        {
            currentChanceOfSpawn = 0;
            GameObject junk = junkObjects[Random.Range(0, junkObjects.Length)];
            Vector3 newPos = Random.onUnitSphere * 70;
            newPos.z = 0;

            var j = Instantiate(junk, newPos, Quaternion.identity);
            j.GetComponent<GravityBody>().attractor = attractor;
        }
        else
        {
            currentChanceOfSpawn += Time.deltaTime;
        }
    }
}
