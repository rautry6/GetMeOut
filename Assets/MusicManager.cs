using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private AudioSource audioSource;
    
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

    IEnumerator PlayingMusic()
    {
        yield return new WaitForSeconds(audioClips[_currentMusicIndex].length);
        _currentMusicIndex = (_currentMusicIndex + 1) % audioClips.Length;
        PlayMusic();
    }
}