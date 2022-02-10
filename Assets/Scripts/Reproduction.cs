using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reproduction : MonoBehaviour
{


    public Transform creature;

    public BasicMovement basicMovementScript;

    //Health
    public float animalHealth, animalHealthMax, lifeFactor, hungerThreshold;

    //public Transform creature;


    //Food
    public float foodConsumed, numberOfFoodConsumed, energyFromFood, energyPerFood;//5
    public bool foodEaten, gaveBirth, initialize;



    // Pregnancy
    public float pregnancyPeriod, healthPregnancyThreshold, foodPregnancyThreshold, healthAfterPregnancy;
    public float offsringTimesCount, maxNumberOfOffspringsBeforeDeath, maxNumberOfBabiesItCanGiveBirthAtOnce, minNumberOfBabiesItCanGiveBirthAtOnce; //Convert it ot int once in use(Random between 0 and max)



    // Start is called before the first frame update
    void Start()
    {
        basicMovementScript = creature.GetComponent<BasicMovement>();
        // StartCoroutine(GaveBirth());

    }

    // Update is called once per frame
    void Update()
    {
        foodEaten = basicMovementScript.foodReached;
        EnergyConversion();
        HealthSystem();
        Birth();
        Death();

    }

    void HealthSystem()
    {

        animalHealth -= Time.deltaTime * lifeFactor;
        if (foodEaten && animalHealth + energyFromFood <= animalHealthMax) animalHealth += energyFromFood;

    }


    void EnergyConversion()
    {
        if (foodEaten)
        {
            energyFromFood = energyPerFood;
            numberOfFoodConsumed++;
        }
        else energyFromFood = 0;

    }

    void Death()
    {
        if (animalHealth <= 0 || offsringTimesCount >=maxNumberOfOffspringsBeforeDeath)
        {
            offsringTimesCount =0;
            Debug.Log("Die of age/huger: ");
            Destroy(gameObject);
        }

    }

    /*
        IEnumerator GaveBirth()
        {

            while (true)
            {
                if (gaveBirth)
                {
                    numberOfFoodConsumed = 0;
                    animalHealth = healthAfterPregnancy;
                    gaveBirth = false;
                    yield return new WaitForSeconds(pregnancyPeriod);
                }


                //pregnancy will reduce their speed and strength // will implement it later.

            }

        }
        */

    void Birth()
    {
        if ((animalHealth >= healthPregnancyThreshold) && (numberOfFoodConsumed >= foodPregnancyThreshold))
        {
            numberOfFoodConsumed = 0;
            animalHealth = healthAfterPregnancy;

            float babies = Random.Range(minNumberOfBabiesItCanGiveBirthAtOnce, maxNumberOfBabiesItCanGiveBirthAtOnce);
            if (babies > maxNumberOfBabiesItCanGiveBirthAtOnce)
            {
                babies = maxNumberOfBabiesItCanGiveBirthAtOnce;
            }
            for (int i = 0; i < babies; i++) Instantiate(creature, transform.position, transform.rotation);
            Debug.Log("Gave Birth of " + babies + " babies");
            offsringTimesCount++;



        }
    }
}


