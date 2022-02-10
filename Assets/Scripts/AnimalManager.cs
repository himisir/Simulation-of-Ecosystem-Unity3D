using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform animal;
    public bool stopQueue;
    void Start()
    {
        StartCoroutine(Spawn());


    }
    IEnumerator Spawn()
    {
        while (!stopQueue)
        {
            stopQueue = true;
            Instantiate(animal, animal.transform.position, animal.transform.rotation);
            yield return new WaitForSeconds(1);
        }
    }
}
