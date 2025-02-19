using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public abstract void Enter(); // Khi UI xuat hien
    public abstract void Interact(); // Khi UI duoc tuong tac
    public abstract void Exit(); // Khi UI bien mat
}


