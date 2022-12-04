using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour
{
    Rigidbody rb;

    public float mainThrust = 10f;
    public float rotationThrust = 50f;
    [SerializeField] int maxHealth = 100;

    AudioSource myAudioSource;
    GameController gameController;
    HealthBar healthBar;

    [SerializeField] AudioClip mainEngine, deathExplosion, landingPadSFX;
    [SerializeField] ParticleSystem mainEngineParticles, explosionParticles;

    bool isAlive = true;
    int currentHealth;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        healthBar = FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        if(isAlive)
        {
            RocketMovement();
        }
        
    }

    void TakeDamage( int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        FindObjectOfType<CanvasFade>().Fade();

        if(currentHealth == 0)
        {
            DeathRoutine();
        }
    }

    private void RocketMovement()
    {
        float rotationSpeed = Time.deltaTime * rotationThrust;

        Thrusting();
        Rotating(rotationSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive || !gameController.collisionEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;

            case "Finish":
                FinishRoutine();
                break;

            case "Fuel":
                //gain fuel
                break;

            default:
                TakeDamage(20);
                break;
        }
    }


    private void DeathRoutine()
    {
        isAlive = false;
        explosionParticles.Play();
        AudioSource.PlayClipAtPoint(deathExplosion, Camera.main.transform.position);

        FindObjectOfType<ShakeCam>().ShakeCamera();
        gameController.ResetGame();
    }

    private void FinishRoutine()
    {
        rb.isKinematic = true;
        AudioSource.PlayClipAtPoint(landingPadSFX, Camera.main.transform.position);
        gameController.NextLevel();
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
        else
        {
            mainEngineParticles.Stop();
            myAudioSource.Stop();
        }
    }

    private void Rotating(float rotationSpeed)
    {
        rb.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rb.freezeRotation = false;
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }
}
