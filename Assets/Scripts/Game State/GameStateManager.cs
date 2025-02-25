using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    private IState currentState;
    
    private void Start()
    {
        Instance = this;

        StartFlow();
    }

    private void StartFlow()
    {
        currentState = new MainMenuState();
        currentState.PrepareState();
    }
    
    public void ChangeState(IState newState)
    {
        currentState.DestroyState();

        currentState = newState;
        
        currentState.PrepareState();
    }
}