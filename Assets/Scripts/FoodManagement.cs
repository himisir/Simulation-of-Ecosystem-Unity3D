using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManagement : MonoBehaviour
{

    public float foodCount, foodCon, foodCountThreshold; //Number of food in the system right now. 
    [Range(1f, 10f)]
    public float rayDistance;
    public float spawnRate, spawned, waitTimeOut; //Number of food per generation
    [Range(0f, 100f)]
    public float range;
    public float offset;

    public Transform food;
    public Transform creature;
    public BasicMovement basicMovementScript;



    // Start is called before the first frame update
    void Start()
    {
        basicMovementScript = creature.GetComponent<BasicMovement>();
        StartCoroutine(SpawnManager());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnManager()
    {
        while (true)
        {
            foodCon = basicMovementScript.foodConsumed;




            /*
            if (foodCount - foodCon < foodCountThreshold)
            {
                if (spawnRate > spawned)
                {
                    Instantiate(n);
                    foodCount++;
                    spawned++;
                }
            }
            if (spawned == spawnRate)
            {
                spawned = 0;
            }

            */

            for (int i = 0; i < foodCountThreshold; i++)
            {
                Vector3 pos = RandomPosition();
                Instantiate(food, pos, food.rotation);
            }
            yield return new WaitForSeconds(waitTimeOut);
        }
    }

    Vector3 RandomPosition()
    {
        Vector3 pos;
        float x = Random.Range(-range + Time.deltaTime, range + Time.deltaTime);
        float z = Random.Range(-range + Time.deltaTime, range + Time.deltaTime);
        float y = food.localScale.y / 2 + offset;
        pos = new Vector3(x, y, z);
        if (!Physics.Raycast(pos, Vector3.down, rayDistance)) pos = RandomPosition();
        return pos;
    }



}
