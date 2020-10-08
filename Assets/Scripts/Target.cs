using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    GameManager gameManager;

    private Rigidbody targetRb;
    public ParticleSystem explosionParticle;

    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;

    public int pointValue;

    void Start()
    {
        // reference to the game manager script
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // reference to the target's rigidbody component
        targetRb = GetComponent<Rigidbody>();

        // once a target spawns, toss it up into the screenview
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        // at a randomized position
        transform.position = RandomSpawnPos();
    }

    // generates a random force
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    // generates a random Torque
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    // generates a random position
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // when a target is clicked on, it updates the score and makes the target explode
    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }

    // if a box target leaves the screen without being clicked, game over (but bombs can pass)
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isGameActive && other.CompareTag("Sensor"))
        {
            Destroy(gameObject);

            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
        }
    }
}
