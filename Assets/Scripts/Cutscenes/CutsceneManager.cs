using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup cutsceneCanvasGroup;
    [SerializeField] private CanvasGroup innerGroup;
    [SerializeField] private Canvas cutsceneCanvas;
    [SerializeField] private TextMeshProUGUI typewriterText;
    [SerializeField] private Image slideshow;

    [SerializeField] private List<Sprite> introCutscene;
    [SerializeField] private List<Sprite> finalBossCutscene;
    [SerializeField] private List<Sprite> endingCutscene;

    [SerializeField] private List<string> introWriting;
    [SerializeField] private List<string> bossWriting;
    [SerializeField] private List<string> endWriting;

    [SerializeField] private Move playerMove;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Jump playerJump;

    private int _listIndex = 0;
    private int _currentCutscene;

    void Start()
    {
        cutsceneCanvasGroup.alpha = 1f;
        innerGroup.alpha = 0f;
        _currentCutscene = 0;

        StartCutscene();
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && innerGroup.alpha == 1f)
        {
            Debug.Log("space pressed");
            NextSlide();
        }
    }

    /// <summary>
    /// Hide all elements (not including the black background)
    /// </summary>
    public void HideElements()
    {
        
    }

    /// <summary>
    /// Go to the next "slide" image in a given cutscene
    /// </summary>
    public void NextSlide()
    {
        // If already on last slide, close cutscene
        if ((_currentCutscene == 0 && _listIndex == introCutscene.Count - 1) || 
        (_currentCutscene == 1 && _listIndex == finalBossCutscene.Count - 1) || 
        (_currentCutscene == 2 && _listIndex == endingCutscene.Count - 1))
        {
            innerGroup.DOFade(0f, 2f).OnComplete(() =>
            {
                cutsceneCanvasGroup.DOFade(0f, 3f).OnComplete(() =>
                {
                    Debug.Log("ending the cutscene");
                    EndCutscene();
                });
            });
            return;

        }

        _listIndex++;
        if (_currentCutscene == 0)
        {
            innerGroup.DOFade(0f, 1f).OnComplete(() =>
            {
                slideshow.sprite = introCutscene[_listIndex];
                typewriterText.text = introWriting[_listIndex];
                innerGroup.DOFade(1f, 1f);
            });
            
        } else if (_currentCutscene == 1) {
            slideshow.sprite = finalBossCutscene[_listIndex];
            typewriterText.text = bossWriting[_listIndex];
        } else {
            slideshow.sprite = endingCutscene[_listIndex];
            typewriterText.text = endWriting[_listIndex];
        }
        
    }

    /// <summary>
    /// Begins a cutscene
    /// </summary>
    public void StartCutscene()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();
        playerHealth.UpdateInvulnerable(true);
        
        if (_currentCutscene == 0)
        {
            slideshow.sprite = introCutscene[_listIndex];
            typewriterText.text = introWriting[_listIndex];
        } else if (_currentCutscene == 1)
        {
            slideshow.sprite = finalBossCutscene[_listIndex];
            typewriterText.text = bossWriting[_listIndex];
        } else if (_currentCutscene == 2)
        {
            slideshow.sprite = endingCutscene[_listIndex];
            typewriterText.text = endWriting[_listIndex];
        } else
        {
            Debug.LogWarning("No more cutscenes!");
            return;
        }
        
        cutsceneCanvasGroup.DOFade(1.001f, 4).OnComplete(() =>
        {
            StartCoroutine(FadeInGroup());
        });
    }

    /// <summary>
    /// Ends the cutscene
    /// </summary>
    public void EndCutscene()
    {
        //cutsceneCanvas.gameObject.SetActive(false);
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        playerHealth.UpdateInvulnerable(false);
        _currentCutscene++;
    }

    private IEnumerator FadeInGroup()
    {
        yield return new WaitForSeconds(1f);
        innerGroup.DOFade(1.001f, 2f).OnComplete(() =>
        {
            Debug.Log("yeet");
        });
    }

    private IEnumerator FadeOutGroup()
    {
        yield return new WaitForSeconds(4.5f);
        cutsceneCanvasGroup.DOFade(0, 1.5f).OnComplete(() =>
        {
            EndCutscene();
        });
    }
}
