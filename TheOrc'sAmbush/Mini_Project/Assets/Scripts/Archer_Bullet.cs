using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Bullet : MonoBehaviour
{
    float rotationSpeed = 180f;
    float currentRotation;
    public int damage = 1;

    private void Update()
    {
        currentRotation -= rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            rotationSpeed = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(collision.transform.tag=="Player")
        {
           // Debug.Log("Hit player");
            Player.OnPlayerHitRaiseEvent(damage);
        }
        Destroy(gameObject, 1.5f);
    }

}
