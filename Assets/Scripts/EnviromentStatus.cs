using UnityEngine;

public class EnvironmentStatus : MonoBehaviour
{
    // 定義狀態
    public enum State
    {
        StateA,
        StateB
    }

    // 當前狀態
    private State currentState;

    void Start()
    {
        // 初始化狀態
        currentState = State.StateA;
    }

    // 切換狀態的函數
    public void ToggleState()
    {
        if (currentState == State.StateA)
        {
            currentState = State.StateB;
        }
        else
        {
            currentState = State.StateA;
        }

        Debug.Log("當前狀態: " + currentState);
    }
}
