using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    Player player;
    float rotationSpeed;
    float currentRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rotationSpeed = player.speed * 40;
    }

    private void Update()
    {
       
        currentRotation -= rotationSpeed*Time.deltaTime;
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
            Destroy(gameObject, 1f);
        }
        if(collision.transform.tag=="Arrow")
        {
            Player.OnPlayerHitRaiseEvent(0);
        }
    }
}
