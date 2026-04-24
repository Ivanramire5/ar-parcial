using System;
using UnityEngine;

// El script Monstro hereda toda la lógica de colisiones de ARInteractableObject.
public class Monstro : ARInteractableObject
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void SetState(State state)
    {
        base.SetState(state);

        // Activamos la animación idle.
        _animator.SetTrigger("IraIdle");
    }
}