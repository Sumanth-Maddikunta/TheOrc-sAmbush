using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy_Base
{
    public delegate void OnArcherDeath();
    static public event OnArcherDeath OnArcherDeathEvent;

    public GameObject bulletPrefab;
    public Transform hand;
    public float fireRate = 3f;
    public GameObject floatingBase;
    public uint score = 5;
    public int ammoBonus = 3;
    public GameObject bloodParticleSystemPrefab;
    public AudioClip throwAudioClip;

    private Transform player;
    private float initialSpeed=25f;
    private Vector2 shootingAngle;
    private float angle;
    private float time;
    private Animator animator;
    private bool isArcheralive = true;
    

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = fireRate;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        time -= Time.deltaTime;
        if (time<=0f&&player.GetComponent<Player>().isPlayerAlive&&isArcheralive)
        {
            time = fireRate;
            Shoot();
        }
        if(PersistantManager.PM.currentScore>100)
        {
            fireRate = 1.5f;
            if (PersistantManager.PM.currentScore > 150)
            {
                fireRate = 1f;
            }
        }
       

    }
    void Shoot()
    {
        Debug.Log("threw");
        player.GetComponent<AudioSource>().PlayOneShot(throwAudioClip, 1f);
        float speed = Random.Range(5f, initialSpeed);
        shootingAngle = (player.transform.position - transform.position);
        angle = Mathf.Atan2(shootingAngle.y, shootingAngle.x);
        GameObject bulletInstance = Instantiate(bulletPrefab);
        bulletInstance.transform.position = hand.position;
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

        animator.SetBool("Throw", true);
        Invoke("SetArcherBoolFalse", 0.5f);

        rb.AddForce(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * speed, ForceMode2D.Impulse);
        Destroy(bulletInstance, 3f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            if (OnArcherDeathEvent == null)
            {
                Debug.Log("Empty Event");
            }
            else
            {
                OnArcherDeathEvent();
            }

            //GetComponent<AudioSource>().Play();
            Enemy_Base.RaiseEvent(score,ammoBonus);
            float st = Time.time;
            isArcheralive = false;
            //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0.0f, 1.0f, ((Time.time - st) / 5f)));
            GameObject bloodPrefabInstance = Instantiate(bloodParticleSystemPrefab, transform.position, Quaternion.identity, transform.parent);
            animator.SetBool("isDead", true);
            Destroy(collision.gameObject);
            Destroy(bloodPrefabInstance, 1.5f);
            Destroy(floatingBase,2.0f);
            Destroy(gameObject,2.0f);
        }

    }

    void SetArcherBoolFalse()
    {
        animator.SetBool("Throw", false);
    }
}
