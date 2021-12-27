using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    [SerializeField] BowlController bowlController;


    private void OnTriggerEnter(Collider other)
    {
        bowlController.destroyBall();
    }
}
