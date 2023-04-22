using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] bossAudioClips;
    
    private int _currentMusicIndex;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _currentMusicIndex = 0;
        MainMenuPlayMusic();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "DeafBoss")
        {
            audioSource.Stop();
            _currentMusicIndex = 0;
            PlayBossMusic();
        }
    }

    void MainMenuPlayMusic()
    {
        audioSource.clip = mainMenuClip;
        audioSource.Play();
    }
    
    public void PlayMusic()
    {
        audioSource.clip = audioClips[_currentMusicIndex];
        audioSource.Play();
        StartCoroutine(PlayingMusic());
    }
    
    public void PlayBossMusic()
    {
        audioSource.clip = bossAudioClips[_currentMusicIndex];
        audioSource.Play();
        StartCoroutine(PlayingBossMusic());
    }

    IEnumerator PlayingMusic()
    {
        yield return new WaitForSeconds(audioClips[_currentMusicIndex].length);
        _currentMusicIndex = (_currentMusicIndex + 1) % audioClips.Length;
        PlayMusic();
    }
    
    IEnumerator PlayingBossMusic()
    {
        yield return new WaitForSeconds(bossAudioClips[_currentMusicIndex].length);
        _currentMusicIndex = (_currentMusicIndex + 1) % bossAudioClips.Length;
        PlayBossMusic();
    }
}