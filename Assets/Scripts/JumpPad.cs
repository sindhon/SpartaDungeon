using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpPadPower;    // 점프 패드의 힘

    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(Vector3.up * jumpPadPower, ForceMode.Impulse); // 부딪친 오브젝트를 점프패드의 힘만큼 위로 띄움
        }
    }
}
