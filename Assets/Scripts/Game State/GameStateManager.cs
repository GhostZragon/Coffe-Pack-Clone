using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    [SerializeField ]private BaseState currentState;
    
    private void Start()
    {
        Instance = this;
        
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