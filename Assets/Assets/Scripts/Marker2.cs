using System;
using UnityEngine;

// El script Monstro hereda toda la lógica de colisiones de ARInteractableObject.
public class Marker2 : ARInteractableObject
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void SetState(State state)
    {
        base.SetState(state);

        
        _animator.SetTrigger("idle");
    }
}