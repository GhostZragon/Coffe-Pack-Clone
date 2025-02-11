using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public abstract void Enter(); // Khi UI xuat hien
    public abstract void Interact(); // Khi UI duoc tuong tac
    public abstract void Exit(); // Khi UI bien mat
}

public interface IEnter
{
    void Enter();
}

public interface IInteract
{
    void Interact();
}

public interface IExit
{
    void Exit();
}