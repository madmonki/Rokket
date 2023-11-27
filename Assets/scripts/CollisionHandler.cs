using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{    
    [SerializeField]float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashingSFX;
    [SerializeField] AudioClip landingSFX;
    
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource auS;
    
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        auS = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        RespondToDebugKeys();    
    }
    
    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }
        
        switch (other.gameObject.tag)
        {
            case "friendly":
            //    Debug.Log("It's friendly.");
                break;
            case "Finish":
                LandingSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        // todo add particle effect upon crash
        isTransitioning = true;
        auS.Stop();
        GetComponent<Movement>().enabled = false;
        auS.PlayOneShot(crashingSFX);
        crashParticles.Play();
        Invoke ("ReloadLevel", 1.75f);
    }

    void LandingSequence()
    {
        isTransitioning = true;
        auS.Stop();
        GetComponent<Movement>().enabled = false;
        auS.PlayOneShot(landingSFX);
        successParticles.Play();
        Invoke ("LoadNextLevel", levelLoadDelay);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}


