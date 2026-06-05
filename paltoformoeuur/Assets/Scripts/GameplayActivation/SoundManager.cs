using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    [Header("Player")]
    public AudioClip walkForest;
    public AudioClip jump;
    public AudioClip aim;
    public AudioClip launch;
    public AudioClip land;
    public AudioClip crochet;
    public AudioClip deathFinal;
    public AudioClip deathMold;
    public AudioClip collidePart;
    public AudioClip walkArm;
    public AudioClip dashBras;
    
    [Header("Enviro")]
    public AudioClip bumperSound;
    public AudioClip triggerButton;
    public AudioClip triggerCheckpoint;
    public AudioClip respawnCheckpoint;
    
    [Header("UI")]
    public AudioClip pause;
    public AudioClip UIButtonClick;
    public AudioClip UIButtonClickMenu;
    public AudioClip UIButtonHover;

    private AudioSource audioSource;
    
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
    
    
    public void PlayLongSound(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
