using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [Range(0f, 100f)]
    public float steps, range, rayDistance;
    public bool targetReached;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

        Vector3 pos;
        if (targetReached)
        {
            targetReached = false;
            pos = RandomPosition();
            targetPos = pos;
        }

        Move(targetPos);
    }

    Vector3 RandomPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-range, range), transform.position.y, Random.Range(-range, range));

        if (!Physics.Raycast(newPosition, Vector3.down, rayDistance))
        {
            newPosition = RandomPosition();

        }
        return newPosition;
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
