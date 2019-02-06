using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_Golem : Enemy_Base
{
    public  float speed = 5f;
    public static float difficulty = 1.0f;
    public uint score = 2;
    public int ammoBonus = 2;
    public int damage = 1;
    public GameObject bloodParticleSystemPrefab;

    private bool isGolemDead = false;
    private Transform player;
    Vector2 direction;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        Init();
    }	

    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        direction = Vector2.left;
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position += (Vector3)direction *  speed*difficulty*Time.deltaTime;//Movement

        if (player.GetComponent<Player>().isPlayerAlive == false)//To set animator false
        {
            speed = 0f;
            animator.SetBool("isPlayerDead", true);
            animator.SetBool("CanAttack", false);
            
        }
        //if(transform.position.x)
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bb = Resources.Load("DeadGolem_ParticleEffect") as GameObject;

        if (collision.transform.tag=="Bullet")
        {

            isGolemDead = true;
            speed = 0f;
            animator.SetBool("Dead", true);
            
            Enemy_Base.RaiseEvent(score,ammoBonus);
            
            GameObject bloodPrefabInstance= Instantiate(bloodParticleSystemPrefab, transform.position, Quaternion.identity,transform.parent);
            //GameObject bloodPrefabInstance= Instantiate(bb, transform.position, Quaternion.identity,transform.parent);
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject);
            Destroy(bloodPrefabInstance,1.5f);
            Destroy(gameObject, 2f);
        }
        if(collision.transform.tag=="Player")
        {
            speed = 0;
            if (player.GetComponent<Player>().isPlayerAlive)
            {
                animator.SetBool("CanAttack", true);
            }
            if (player.GetComponent<Player>().isPlayerAlive)
            {
                
                    InvokeRepeating("callPlayerHitEvent", 0, 1.0f);
                    Invoke("method", 2.5f);
                
            }
           
        }

    }

    void callPlayerHitEvent()
    {
        if (isGolemDead == false)
        {
            Player.OnPlayerHitRaiseEvent(damage);
        }
    }
    void method()
    {
        CancelInvoke("callPlayerHitEvent");
    }
}
