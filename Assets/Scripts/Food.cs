using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [Range(0f, 100f)]
    public float steps, range, offset, rayDistance;
    public int seed;
    public bool targetReached;
    Vector3 targetPos, randomPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = RandomPosition();
    }

    void Update()
    {
        


        if (targetReached)
        {
            targetReached = false;
            randomPos = RandomPosition();
            targetPos = randomPos;
        }

        Move(targetPos);
    }

    Vector3 RandomPosition()
    {

        Vector3 pos;
        float x = Random.Range(-range + Time.deltaTime * offset, range + Time.deltaTime * offset);
        float z = Random.Range(-range + Time.deltaTime * offset, range + Time.deltaTime * offset);
        float y = transform.position.y;
        pos = new Vector3(x, y, z);
        if (!Physics.Raycast(pos, Vector3.down, rayDistance)) pos = RandomPosition();
        return pos;
    }
    void Move(Vector3 target)
    {
        Vector3 targetPos = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(targetPos);
        transform.position = Vector3.Lerp(transform.position, targetPos, steps * Time.deltaTime);
        Debug.Log(transform.position + " " + targetPos + " " + Vector3.Distance(targetPos, transform.position));
        if (Vector3.Distance(targetPos, transform.position) < 1f) targetReached = true;

    }



}
