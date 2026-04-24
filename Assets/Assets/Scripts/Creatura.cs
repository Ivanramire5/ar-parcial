using UnityEngine;

public class Creatura : ARInteractableObject
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
            // IMPORTANTE: Asegurate que en el Animator el trigger se llame exactamente IraIdle
            _animator.SetTrigger("IraIdle");
        }
    }
}