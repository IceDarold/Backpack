using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private bool isEnter = false;
    private Collision2D otherCollision;

    private void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isEnter = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isEnter = false;
    }

}
