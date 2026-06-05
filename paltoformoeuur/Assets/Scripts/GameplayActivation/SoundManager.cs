using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip music;
    [SerializeField] private int timeBetweenMusics = 5;
    
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

    [Header("sources")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicSource;
    
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    private void Start()
    {
        StartCoroutine(PlayMusic());
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
    
    private IEnumerator PlayMusic()
    {
        musicSource.PlayOneShot(music);
        yield return new WaitForSeconds(music.length + timeBetweenMusics);
        StartCoroutine(PlayMusic());
    }
}
