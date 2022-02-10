using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

    [Range(0f, 100f)]
    public float moveSpeed;
    [Range(0f, 100f)]
    public float rotationSpeed;
    public float rayDistance;
    Vector3 foodPosition, targetPosition;
    [Range(0f, 100f)]
    public float steps;
    SphereCollider senseBound;
    [Range(0f, 100f)]
    public float visionRange;
    public float visionRadius;

    [Range(50f, 100f)]
    public float foodConsumed;



    [Range(0f, 100f)]
    public float range;
    public bool foodInReach, targetReached, foodReached, lastKnownPosition;


    public Transform creature;
    public Reproduction reproductionScript;



    void Sense()
    {
        //Priority Low
        //Will detect food but won't get exacts location of it. 
        //How it works is, Another collider to check if food is present in the system then resister it's location and then go after it.
        //Location wil be actualPosition + offset;
        //Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange),Random.Range(-offsetRange, offsetRange))
        //offset will be overridden by foodPostion once foodIsfound 
    }

    //Set foodPosition Value to zero at awake;
    void Start()
    {
        senseBound = GetComponent<SphereCollider>();
        reproductionScript = creature.GetComponent<Reproduction>();
    }

    //Check for food; 
    void SetRadius()
    {
        senseBound.radius = visionRange;
    }


    void FixedUpdate()
    {
        OnDraw();
        SetRadius();
        float animalHealth = reproductionScript.animalHealth;
        float hungerThreshold = reproductionScript.hungerThreshold;

        bool flag = true;//To check if it is hungry or not;

        /*

                if (animalHealth > hungerThreshold)
                {
                    flag = (((Random.Range(4, 10)) / 10f) * 100 > 70); //Convert this hardcode to variables; 
                }
        */


        if (flag)
        {
            if (foodInReach || lastKnownPosition)
            {


                targetPosition = foodPosition;
                lastKnownPosition = false;
                foodInReach = false;

            }
            else
            {
                if (targetReached || foodReached)
                {

                    targetPosition = RandomPosition();
                    if (targetReached) targetReached = false;
                    if (foodReached) foodReached = false;
                }
            }
            //Debug.Log("Food Found at: " + targetPosition + "Inside vision Range: " + Vector3.Distance(transform.position, targetPosition));
            Move(targetPosition);
        }



    }


    //Use two(left and right Angle) raycast inside of OnTriggerStay to check enter and exit from Fov
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Food"))
        {
            Vector3 hitPosition = other.transform.position;
            float hitAngle = Vector3.Angle(transform.forward, hitPosition);

            if (hitAngle <= visionRadius / 2.0f)
            {
                if (Vector3.Distance(transform.position, hitPosition) <= visionRange)
                {
                    foodPosition = hitPosition;
                    foodInReach = true;
                }
            }

        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Food"))
        {
            Vector3 hitPosition = other.transform.position;
            float hitAngle = Vector3.Angle(transform.forward, hitPosition);

            if (hitAngle <= visionRadius / 2.0f)
            {
                if (Vector3.Distance(transform.position, hitPosition) <= visionRange)
                {
                    foodPosition = hitPosition;
                    foodInReach = true;
                }
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Food"))
        {
            Vector3 hitPosition = other.transform.position;

            float hitAngle = Vector3.Angle(transform.forward, hitPosition);

            if (hitAngle <= visionRadius / 2.0f)
            {
                if (Vector3.Distance(transform.position, hitPosition) <= visionRange)
                {
                    foodPosition = hitPosition;
                    {
                        lastKnownPosition = true;
                        targetReached = true;
                        foodInReach = false;
                    }
                }
            }

        }

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Debug.Log("Food has been consumed");
            foodReached = true;
            foodInReach = false;
            Destroy(other.gameObject);
        }
    }

    void Move(Vector3 target)
    {
        Vector3 targetPos = new Vector3(target.x, transform.position.y, target.z);

        //transform.position = Vector3.Lerp(transform.position, targetPos, steps * Time.deltaTime);
        transform.LookAt(Vector3.Lerp(transform.position, targetPos, rotationSpeed * Time.deltaTime));
        //Lerp with MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, targetPos, steps * Time.deltaTime), moveSpeed * Time.deltaTime);
        //transform.LookAt(Vector3.Lerp(transform.position, targetPos, rotationSpeed * Time.deltaTime));

        //Debug.Log(transform.position + " " + targetPos + " " + Vector3.Distance(targetPos, transform.position));
        if (Vector3.Distance(targetPos, transform.position) < 1f)
        {
            targetReached = true;
            //foodInReach = false;
        }


    }
    Vector3 RandomPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-range, range), transform.position.y, Random.Range(-range, range));

        if (!Physics.Raycast(newPosition, Vector3.down, rayDistance)) newPosition = RandomPosition();

        return newPosition;
    }


    void OnDraw()
    {
        float totalFoV = visionRadius;
        float rayRange = visionRange;
        float halfFoV = totalFoV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFoV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFoV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Debug.DrawRay(transform.position, leftRayDirection * rayRange);
        Debug.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
