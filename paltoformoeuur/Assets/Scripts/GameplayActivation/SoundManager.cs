using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private AudioClip music;
    [SerializeField] private int timeBetweenMusics = 5;
    
    [Header("Player")]
        public AudioClip[] walk;
        public AudioClip jump;
        public AudioClip aim;
        public AudioClip launch;
        public AudioClip land;
        public AudioClip deathFinal;
        public AudioClip deathMold;
        public AudioClip collidePart;
        public AudioClip walkArm;
        public AudioClip dashBras;
        public AudioClip recall;
        public AudioClip chute;
    
    [Header("Enviro")]
        public AudioClip bumperSound;
        public AudioClip triggerButton;
        public AudioClip triggerCheckpoint;
        public AudioClip respawnCheckpoint;
        public AudioClip crochet;
        public AudioClip crochetGauche;
        public AudioClip crochetDroite;
        public AudioClip breakableWall;
    
    [Header("UI")]
        public AudioClip pause;
        public AudioClip UIButtonClick;
        public AudioClip UIButtonClickMenu;
        public AudioClip UIButtonHover;

    [Header("sources")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambientSoundSource;
        
    public float mainVolume;
    public float soundEffectVolume;
    public float musicVolume;
        
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    private void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        soundEffectVolume = PlayerPrefs.GetFloat("soundEffectVolume");
        mainVolume = PlayerPrefs.GetFloat("mainVolume");
        StartCoroutine(PlayMusic());

        if (ambientSound != null)
        {
            ambientSoundSource.clip = ambientSound;
            ambientSoundSource.volume = mainVolume * soundEffectVolume;
            ambientSoundSource.loop = true;
            ambientSoundSource.Play();
        }
    }
    
    public void PlaySound(AudioClip audio)
    {
        if (audio == null)
        {
            return;
        }
        
        audioSource.PlayOneShot(audio,mainVolume * soundEffectVolume);
    }
    
    private IEnumerator PlayMusic()
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.volume = mainVolume * musicVolume;
            musicSource.Play();

            if (timeBetweenMusics == 0)
            {
                musicSource.loop = true;
            }
            else
            {
                yield return new WaitForSeconds(music.length + timeBetweenMusics);
                StartCoroutine(PlayMusic());
            }
            
            
        }
    }
    
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = mainVolume * musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    
    public void ChangeMainVolume(float volume)
    {
        mainVolume = volume;
        musicSource.volume = mainVolume * musicVolume;
        PlayerPrefs.SetFloat("mainVolume", mainVolume);
    }

    public void ChangeEffectVolume(float volume)
    {
        soundEffectVolume = volume;
        PlayerPrefs.SetFloat("soundEffectVolume", soundEffectVolume);
    }

    public void PlayStepSound()
    {
        PlaySound(walk[Random.Range(0,walk.Length)]);
    }

    public void PlayArmStepSound()
    {
        PlaySound(walkArm);
    }
}
