using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemID;
    public TextMeshPro textMeshPro;

    public void Awake()
    {
        textMeshPro.text = itemID.ToString();
    }

    //public void Move(Vector3 position, Transform parent)
    //{
    //    transform.parent = parent;
    //    transform.position = position;

    //    Debug.Log($"Item {name} go to {parent.name} with position: {position}", gameObject);
    //}
}
