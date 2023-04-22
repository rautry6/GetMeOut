using TMPro;
using UnityEngine;

public class PCDetectsPlayer : MonoBehaviour
{
    [SerializeField] private LoreSO loreText;
    [SerializeField] private TMP_Text loreTMPText;
    private Animator _animator;
    private readonly int _hasInteracted = Animator.StringToHash("HasInteracted");
    private readonly int _off = Animator.StringToHash("Off");
    private static readonly int IsInteractable = Animator.StringToHash("isInteractable");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerInteractable();
        }
    }

    private void TriggerInteractable()
    {
        //_animator.SetTrigger(_interactable);
        _animator.SetBool(IsInteractable, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(IsInteractable, false);
            SetHasInteractedFalse();
        }
    }

    public void SetHasInteractedTrue()
    {
        if (_animator.GetBool(IsInteractable))
        {
            loreTMPText.text = loreText.LoreText;
            _animator.SetBool(_hasInteracted, true);
        }
    }
    
    public void SetHasInteractedFalse()
    {
        _animator.SetBool(_hasInteracted, false);
    }

    public void TriggerOff()
    {
         _animator.SetTrigger(_off);
    }
}