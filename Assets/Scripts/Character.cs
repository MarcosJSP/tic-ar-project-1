using UnityEngine;

public class Character : ARInteractableObject
{
    [SerializeField] private Renderer _renderer;
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
                if (_renderer == null) return;
                _renderer.materials[0].EnableKeyword("_EMISSION");
                _renderer.materials[0].SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f));
                break;
            case State.Active:
                _animator.SetTrigger("StartInteraction");
                if (_renderer == null) return;
                _renderer.materials[0].EnableKeyword("_EMISSION");
                _renderer.materials[0].SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f, 0.1f));

                break;
        }
    }

}
