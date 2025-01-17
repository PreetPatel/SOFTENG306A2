using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    private float baseChanceToSpawn = 0.5f;

    private GameObject[] garbageAndThrower;

    private Transform playerTransform;

    private bool thrown = false;

    private void Awake()
    {
        //spawn the person who throws garbage with the player's transform
    
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        garbageAndThrower = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i] = transform.GetChild(i).gameObject;
        }
        OnDisable();
    }

    private void Update() 
    {   
        //get the garbage thrower to throw garbage
        if (garbageAndThrower[0].activeInHierarchy && transform.position.z < playerTransform.position.z + 25 && !thrown)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                garbageAndThrower[i].SendMessage("OnTriggerThrow");
            }
            thrown = true;
        }
    }

    private void OnEnable()
    {
        float chanceToSpawn = (TrashSpawner.garbageMultiplier * 2) + baseChanceToSpawn;
        thrown = false;
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i].SetActive(true);
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i].SetActive(false);
        }
    }
}
