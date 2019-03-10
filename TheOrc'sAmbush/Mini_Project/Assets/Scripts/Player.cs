using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   
    public GameObject bulletPrefab;
    public Transform hand;
    public int bulletCount = 30;
    public int playerHealth = 4;
    public Animator animator;
    public bool isPlayerAlive = true;
    public float speed;
    public bool canThrow=true;
    public SpriteRenderer arrowMarks;
   
    

    public AudioClip throwAudioClip;
    public AudioClip hitAudioClip;

    private AudioSource audioSource;
    private Vector2 shootingAngle;
    private Vector2 startMousePosition;
    private Vector2 endMousePosition;
    private float angle;
    


    public delegate void OnPlayerHit(int damage);
    public static event OnPlayerHit OnPlayerHitEvent;

    private void OnEnable()
    {
        OnPlayerHitEvent += playerHit;
    }

    private void OnDisable()
    {
        OnPlayerHitEvent -= playerHit;
    }

    #region OnPlayerHitEvent Calling Function
    public static void OnPlayerHitRaiseEvent(int damage)
    {
        if (OnPlayerHitEvent != null)
        {
            OnPlayerHitEvent(damage);
        }
        else
        {
            Debug.Log("OnPlayerHitEvent is NULL||not subscribed by any funcition");
        }
    }
    #endregion


    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	
	void Update ()
    {
        if (isPlayerAlive)
        {
            Shoot();
        }
        else
        {
            animator.SetBool("isAlive", false);
            Invoke("disableAnimator", 0.4f);
            //play dead animation
        }

        if(Input.touchCount==0)
        {
            canThrow = true;
        }
        //Debug.Log(bulletCount);
    }

    private void Shoot()
    {
        if (bulletCount > 0)
        {
            #region PC Input Code
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    shootingAngle = (startMousePosition - (Vector2)transform.position);
                    angle = Mathf.Atan2(shootingAngle.y, shootingAngle.x);

                    //PC TRIAL FOR ARROW REPRESENTATION

                    arrowMarks.transform.rotation = Quaternion.Euler(arrowMarks.transform.rotation.x, arrowMarks.transform.rotation.y, angle);



                }
                if (Input.GetMouseButtonUp(0))
                {
                    audioSource.clip = throwAudioClip;
                    audioSource.Play();

                    bulletCount--;
                    animator.SetBool("Throw", true);
                    Invoke("setPlayerBoolFalse", 0.5f);
                    endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    speed = (startMousePosition - endMousePosition).magnitude * 2;
                    GameObject bulletInstance = Instantiate(bulletPrefab);
                    bulletInstance.transform.position = hand.position;
                    bulletInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
                    rb.AddForce(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * speed, ForceMode2D.Impulse);
                    Destroy(bulletInstance, 3f);

                }
            }
            #endregion

            #region Phone Touch Code

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {

                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        startMousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        endMousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                        if ((endMousePosition - startMousePosition).magnitude > 1f)
                        {
                            shootingAngle = (startMousePosition - (Vector2)transform.position);
                            angle = Mathf.Atan2(shootingAngle.y, shootingAngle.x);

                            bulletCount--;
                            animator.SetBool("Throw", true);
                            Invoke("setPlayerBoolFalse", 0.5f);
                            //endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            audioSource.PlayOneShot(throwAudioClip, 0.5f);
                            speed = (startMousePosition - endMousePosition).magnitude * 2;
                            GameObject bulletInstance = Instantiate(bulletPrefab);
                            bulletInstance.transform.position = hand.position;
                            bulletInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                            Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
                            rb.AddForce(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * speed, ForceMode2D.Impulse);
                            Destroy(bulletInstance, 3f);


                            //
                        }
                    }
                }
            }

                #endregion
                                   
           
        }


    }
    void setPlayerBoolFalse()
    {
        animator.SetBool("Throw", false);
       
    }
    void disableAnimator()
    {
        animator.enabled = false;
    }

    void playerHit(int damage)
    {
        if (damage==0)//if 0 i.e it the players bullet collided with archers arrow
        {
            if (Random.Range(1, 10) % 2 == 0)
            {
                bulletCount += 5;//bonus for saving himself from that arrow
            }
            else
            {
                playerHealth++;
            }
            Debug.Log("BC:" + bulletCount);
            Debug.Log("Health:" + playerHealth);

            return;
        }

        audioSource.clip = hitAudioClip;
        audioSource.PlayOneShot(hitAudioClip);

        playerHealth -= damage;
        if(playerHealth<=0)
        {
            playerHealth = 0;
            isPlayerAlive = false;
            animator.SetBool("isAlive", false);
        }
        //Debug.Log("Health" + playerHealth);
    }

    

}
