using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrabEnemy : MonoBehaviour
{
    [SerializeField] private Move playerMove;
    [SerializeField] private Jump playerJump;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private bool escaping = false;
    [SerializeField] private int inputsToEscape = 15;
    private List<int> inputs = new List<int>();

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject grabbedText;
    [SerializeField] private Animator tentacleAnimator;

    [Header("Camera Changes")]
    [SerializeField] private CinemachineVirtualCamera virCamera;
    [SerializeField, Range(2, 8)] private float cameraZoomLevel = 3;
    [SerializeField] private float cameraShakeAmplitude = 1;
    [SerializeField] private float cameraShakeFrequency = 1;
    [SerializeField] private PixelPerfectCamera pixelCamera;

    [Header("Grab Damage")]
    [SerializeField] private bool grabbing = false;
    [SerializeField] private float timeBetweenSqueezes = 1f;
    private static readonly int Spawn1 = Animator.StringToHash("Spawn");


    // Update is called once per frame
    void Update()
    {
        if (escaping)
        {
            if (Input.anyKeyDown)
            {
                inputs.Add(0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (inputs.Count > inputsToEscape)
        {
            escaping = false;
            grabbing = false;
            inputs.Clear();

            pixelCamera.assetsPPU = 16;

            CinemachineBasicMultiChannelPerlin cm = virCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cm.m_AmplitudeGain = 0.1f;
            cm.m_FrequencyGain = 0.1f;

            grabbedText.SetActive(false);

            playerMove.RegainMovement();
            playerJump.EnableJumping();
            virCamera.m_Lens.OrthographicSize = 8;

            Disapear();
        }
    }

    public void Spawn()
    {
        spriteRenderer.enabled = true;
        tentacleAnimator.SetTrigger(Spawn1);
        playerMove.transform.position = transform.position;
        GrabPlayer();
    }

    public void GrabPlayer()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();

        pixelCamera.assetsPPU = 32;

        virCamera.m_Lens.OrthographicSize = cameraZoomLevel;
        grabbedText.SetActive(true);

        void GrabbedCameraShake()
        {
            CinemachineBasicMultiChannelPerlin cm = virCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cm.m_AmplitudeGain = cameraShakeAmplitude;
            cm.m_FrequencyGain = cameraShakeFrequency;
        }

        GrabbedCameraShake();

        escaping = true;
        grabbing = true;

        StartCoroutine(Squeeze());
    }

    public void Disapear()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Spawn();
        }
    }

    private IEnumerator Squeeze()
    {
        yield return new WaitForSeconds(timeBetweenSqueezes);

        if (grabbing && escaping)
        {
            playerHealth.TakeDamage();
            StartCoroutine(Squeeze());
        }
    }
}
