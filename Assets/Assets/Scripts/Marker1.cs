using UnityEngine;

public class Marker1 : ARInteractableObject
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void SetState(State state)
    {
        base.SetState(state);

        if (_animator != null)
        {
            
            _animator.SetTrigger("idle");
        }
    }
}