using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Collider2D))]

public class TextGenerator : MonoBehaviour
{
    public TextMeshPro[] textMessages;
    public GameObject[] arrows;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (TextMeshPro text in textMessages)
            {
                text.gameObject.SetActive(true);
            }
            foreach(GameObject arrow in arrows)
            {
                arrow.SetActive(true);
            }
        }
    }
}
