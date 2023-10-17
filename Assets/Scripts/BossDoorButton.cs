using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossDoorButton : MonoBehaviour
{
    [SerializeField] private GameObject barrier;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer buttonSpriteRenderer;
    [SerializeField] private CinemachineVirtualCamera bossDoorCamera;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            buttonSpriteRenderer.sprite = pressedSprite;
            StartCoroutine(OpeningBossDoor());
        }
    }

    private IEnumerator OpeningBossDoor()
    {
        bossDoorCamera.Priority = 100;
        yield return new WaitForSeconds(2.5f);
        barrier.SetActive(false);
        yield return new WaitForSeconds(1.75f);
        bossDoorCamera.Priority = 0;
    }
}
