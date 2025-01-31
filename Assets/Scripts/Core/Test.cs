using LitMotion;
using LitMotion.Extensions;
using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Test : MonoBehaviour
{
    public float value;
    public Transform target;

    private void Awake()
    {
        TestBtt();
    }

    [Button]
    private void TestBtt()
    {
        LMotion.Create(0f, 10f, 2f) // Animates values from 0f to 10f over 2 seconds
            .Bind(x => value = x);
        LMotion.Create(Vector3.zero, Vector3.one, 2f) // Animates values from (0f, 0f, 0f) to (1f, 1f, 1f) over 2 seconds
            .BindToPosition(target);
    }
}
