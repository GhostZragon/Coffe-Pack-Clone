using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemID;
    public TextMeshPro textMeshPro;
    private AudioSource audioSource;

    public void Awake()
    {
        textMeshPro.text = itemID.ToString();
        audioSource = GetComponent<AudioSource>();
    }

   
    [Button]
    public void PlaySwapSound(float delay)
    {
        audioSource.pitch = Random.Range(0.8f, 1.5f);
        audioSource.PlayDelayed(delay);
        Debug.Log("Pitch is: " + audioSource.pitch);
    }
}

