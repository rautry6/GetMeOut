using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckForVerticalOneWayPlatform : MonoBehaviour
{
    [SerializeField] private LayerMask verticalPlatformLayer;
    [SerializeField] private float colliderDisableTime;
    [SerializeField] private float rayDistance;

    private bool _routineStarted = false;

    private void Update()
    {
        if (!_routineStarted)
        {
            var raycastHit = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, verticalPlatformLayer);
            if (raycastHit)
            {
                StartCoroutine(DisableCollider(raycastHit));
            }
        }

        if (!_routineStarted)
        {
            var raycastHit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance * 2, verticalPlatformLayer);
            if (raycastHit)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    StartCoroutine(DisableCollider(raycastHit));
                }
            }
        }
    }

    IEnumerator DisableCollider(RaycastHit2D raycastHit2D)
    {
        _routineStarted = true;
        var platformCollider = raycastHit2D.collider;
        platformCollider.enabled = false;
        yield return new WaitForSeconds(colliderDisableTime);
        platformCollider.enabled = true;
        _routineStarted = false;
    }
    
}