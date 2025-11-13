using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [Header("점프 힘")]
    public float jumpForce = 15f;   // 원하는 만큼 높이
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider JumpingTarget)
    {

        Rigidbody rb = JumpingTarget.attachedRigidbody;
        if (rb == null) return;                  // 리짓바디 없으면 패스
        if (!rb.CompareTag("Player")) return;    

   
            Vector3 vel = rb.velocity;
            vel.y = 0f;             // 위로 날릴 때 기존 y속도 초기화(선택)
            rb.velocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (anim != null)
            {
                anim.SetTrigger("JumpTrigger");
            }
    }
}