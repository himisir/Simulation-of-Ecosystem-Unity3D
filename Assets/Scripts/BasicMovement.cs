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
    [Range(0f, 100f)]
    public float range;
    public bool foodInReach, targetReached, foodReached;


    //Set foodPosition Value to zero at awake;
    void Start()
    {
        senseBound = GetComponent<SphereCollider>();

    }

    //Check for food; 
    void SetRadius()
    {
        senseBound.radius = visionRange;
    }


    void FixedUpdate()
    {

        SetRadius();
        if (foodInReach)
        {
            targetPosition = foodPosition;
        }
        else
        {
            if (targetReached || foodReached)
            {
                targetPosition = RandomPosition();
                targetReached = false;
                foodReached = false;
            }
        }
        Debug.Log("Food Found at: " + targetPosition + "Inside vision Range: " + Vector3.Distance(transform.position, targetPosition));
        Move(targetPosition);

    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Food"))
        {
            Vector3 hitPosition = other.transform.position;
            float hitAngle = Vector3.Angle(transform.forward, hitPosition);

            if (hitAngle <= visionRadius / 2.0f)
            {
                if (Vector3.Distance(transform.position, hitPosition) < visionRange)
                {
                    foodPosition = hitPosition;
                    foodInReach = true;
                }
            }

        }

        if (foodReached)
        {
            foodInReach = false;
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
                if (Vector3.Distance(transform.position, hitPosition) < visionRange)
                {
                    foodPosition = hitPosition;
                    foodInReach = true;
                }
            }

        }

        if (foodReached)
        {
            foodInReach = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Debug.Log("Food has been consumed");
            foodReached = true;
            Destroy(other.gameObject);
        }
    }

    void Move(Vector3 target)
    {
        Vector3 targetPos = new Vector3(target.x, transform.position.y, target.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, steps * Time.deltaTime);
        transform.LookAt(Vector3.Lerp(transform.position, targetPos, steps));
        Debug.Log(transform.position + " " + targetPos + " " + Vector3.Distance(targetPos, transform.position));
        if (Vector3.Distance(targetPos, transform.position) < 1f) targetReached = true;

        //Lerp with MoveTowards
        // transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, foodPosition, steps*Time.deltaTime), moveSpeed * Time.deltaTime);

    }
    Vector3 RandomPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-range, range), transform.position.y, Random.Range(-range, range));

        if (!Physics.Raycast(newPosition, Vector3.down, rayDistance)) newPosition = RandomPosition();

        return newPosition;
    }


    void OnDrawGizmosSelected()
    {
        float totalFOV = visionRadius;
        float rayRange = visionRange;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
