using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private Animator animator;
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponentInChildren<Animator>();
    }
    public void Move(bool isMove)
    {
        animator.SetBool(IsMoving, isMove);
    }
}
