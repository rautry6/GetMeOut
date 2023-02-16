using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
    [SerializeField] private float flashSpeed = 0.2f;

    private bool flashing = false;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashing)
        {
            flashing = true;
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {
        text.enabled = false;

        yield return new WaitForSeconds(flashSpeed);

       text.enabled = true;

        yield return new WaitForSeconds(flashSpeed);
        flashing = false;
    }
}
