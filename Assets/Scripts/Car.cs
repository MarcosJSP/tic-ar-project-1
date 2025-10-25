using UnityEngine;

public class Car : ARInteractableObject
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void SetState(State newState)
    {
        base.SetState(newState);
        switch (newState)
        {
            case State.Idle:
                _animator.SetTrigger("GoToIdle");
                break;
            case State.Active:
                _animator.SetTrigger("StartInteraction");
                break;
        }
    }
}

