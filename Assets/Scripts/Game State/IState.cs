public interface IState
{
    void PrepareState();
    void DestroyState();
    void Update();
}