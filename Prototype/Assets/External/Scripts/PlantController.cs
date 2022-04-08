using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    Rigidbody plantRb;
    float rotMin = 45;
    float rotMax = 180;
    // on a scale of 10; e.g. 1 is once every 10 sec, 20 is twice every second
    float moveActiveness = 3;
    float moveMin = 40*1.5f;
    float moveMax = 80*1.5f;
    // per second; 100 == 100%
    float decayRate = 10;
    float regenRate = 15;
    float maxHealth = 100;
    public float health;
    public bool onSplatter;
    // if no splatter, this is the time before the pant dehydrates
    public float splatterTimer;

    [SerializeField] List<Renderer> parts;
    [SerializeField] Material matHealthy;
    [SerializeField] Material matWilted;

    void Start()
    {
        UpdateStats();
        health = maxHealth;

        plantRb = GetComponent<Rigidbody>();
        StartCoroutine(MoveAround());
    }

    void Update()
    {
        splatterTimer -= Time.deltaTime;
        if (onSplatter || splatterTimer > 0)
        {
            UpdateHealth(regenRate * Time.deltaTime);
        }
        else
        {
            UpdateHealth(-decayRate * Time.deltaTime);
        }

        foreach(Renderer part in parts)
        {
            float lerp = Mathf.Lerp(1, 0, health / maxHealth);
            part.material.Lerp(matHealthy, matWilted, lerp);
        }

        if (health <= 0)
        {
            Die();
        }

        onSplatter = false;
    }

    // Updates stats based on level count
    void UpdateStats()
    {
        int levelCount =  GameManager.Instance.levelCount;

        moveMin += Random.Range(0, 10) * levelCount;
        moveMax += Random.Range(0, 10) * levelCount;

        float maxHealthAdjuster = Random.value;
        maxHealth -= Random.Range(0, 5) * levelCount * maxHealthAdjuster;

        float moveActivenessAdjuster = Random.value;
        moveActiveness += Random.Range(0, 2) * levelCount * moveActivenessAdjuster;
    }

    void Die()
    {
        UnitManager.Instance.PlantDied();
        Destroy(gameObject);
    }

    void UpdateHealth(float update)
    {
        health += update;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    IEnumerator MoveAround()
    {
        while(true)
        {
            float moveActivenessRand = 10 / (moveActiveness + Random.Range(-3, 3));
            float waitMin = Mathf.Max(0, moveActivenessRand);
            float waitMax = Mathf.Min(10, moveActivenessRand);
            float waitDuration = Random.Range(waitMin, waitMax);
            yield return new WaitForSeconds(Random.Range(1, 4));
            
            // Handles rotation
            bool clockwiseDir = (Random.value > 0.5f);
            float rotSpeed = Random.Range(1.5f, 3);
            float elapsedTime = 0;
            while (elapsedTime < 1)
            {
                float frameRot = Time.deltaTime * rotSpeed;

                Quaternion rot = Quaternion.Euler(new Vector3(0, Random.Range(rotMin, rotMax), 0) * frameRot * (clockwiseDir ? 1 : -1));
                plantRb.MoveRotation(plantRb.rotation * rot);
                yield return null;

                elapsedTime += frameRot;
            }

            // Handles forward movement
            float moveSpeed = Random.Range(moveMin, moveMax);
            plantRb.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Impulse);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Splatter"))
        {
            splatterTimer = 1.0f;
            onSplatter = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Splatter"))
        {
            onSplatter = false;
        }
    }
}
