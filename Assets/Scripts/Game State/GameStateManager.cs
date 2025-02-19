using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    [SerializeField ]private BaseState currentState;
    
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
    
    public void ChangeState(BaseState newState)
    {
        currentState.DestroyState();

        currentState = newState;
        
        currentState.PrepareState();
    }
}