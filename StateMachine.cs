using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public Stack<State> States { get; set; }

    private void Awake()
    {
        States = new Stack<State>();
    }

    private void Update()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().Execute();
        }
    }

    public void PushState(System.Action active, System.Action onEnter = null, System.Action onExit = null)
    {
        if (GetCurrentState() != null)
            GetCurrentState().OnExit();

        State state = new State(active, onEnter, onExit);
        States.Push(state);
        GetCurrentState().OnEnter();

    }

    public void PopState()
    {
        GetCurrentState().OnExit();
        GetCurrentState().ActiveAction = null;
        States.Pop();

        if (GetCurrentState() != null)
            GetCurrentState().OnEnter();
    }

    private State GetCurrentState()
    {
        return States.Count > 0 ? States.Peek() : null;
    }
}
