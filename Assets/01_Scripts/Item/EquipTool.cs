using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    private Animator animator;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }

   
}
