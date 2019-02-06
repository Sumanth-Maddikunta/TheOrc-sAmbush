using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject player;
    public GameObject walkingEnemyPrefab;
    public GameObject archerPrefab;
    public float walkingEnemyFrequency = 5f;//
    public float archerFrequncy = 10f;//
    public Text scoreText;
    public Text highScoreText;
    public Text ammoCountText;
    public Text healthCountText;
    public GameObject handAnimation;
    public ButtonManager_Game buttonManager_Game;
    public AudioClip enemyDeathSound;
    //public GameObject blackCircle;
    

    private float horizontalExtent;
    private float verticalExtent;
    private float walkingEnemySpawnTimer;
    private float archerSpawnTimer;
    private bool isArcherAlive = false;

    
      

    // Use this for initialization

    private void OnEnable()
    {
        Enemy_Base.OnEnemyDeathEvent += UpdateScoreAndAmmo;
    }
    private void OnDisable()
    {
        Enemy_Base.OnEnemyDeathEvent -= UpdateScoreAndAmmo;
    }
    void Start ()
    {
        Time.timeScale = 1;
        //Debug.Log(Time.timeScale);
        // Debug.Log("Invoked");
        //-------------
        //blackCircle.GetComponent<Animator>().SetTrigger("ZoomOut");

        PersistantManager.PM.currentScore = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player==null)
        {
            Debug.Log("PLAYER MISSING");
        }

        //---------------
        walkingEnemySpawnTimer = 0f;
        archerSpawnTimer = 15f;
        verticalExtent = Camera.main.aspect;
        horizontalExtent = Camera.main.orthographicSize * verticalExtent;

        
        PersistantManager.PM.Load();
            
        
    }
    private void Update()
    {
        if (player.GetComponent<Player>().playerHealth > 0)
        {
            WalkingEnemySpawn();
            ArcherSpawner();
        }

        
        scoreText.text = "Score =" + PersistantManager.PM.currentScore;
        highScoreText.text = "High Score=" + PersistantManager.PM.highScore;
        ammoCountText.text = "x" + player.GetComponent<Player>().bulletCount;
        healthCountText.text = "x" + player.GetComponent<Player>().playerHealth;
        //Debug.Log(player.GetComponent<Player>().playerHealth);

        
         if(PersistantManager.PM.currentScore>0)
        {
            Destroy(handAnimation);
        }

        if (PersistantManager.PM.currentScore > 50)//difficulty increase
        {
            walkingEnemyFrequency = 4f;//
            archerFrequncy = 8f;//
        }

        if (PersistantManager.PM.currentScore>100)//difficulty increase
        {
              walkingEnemyFrequency = 3f;//
              archerFrequncy = 6f;//
        }

         if (player.GetComponent<Player>().bulletCount>0&&buttonManager_Game.isGamePaused==false)
        {

            Time.timeScale = 1;
        }
         else if(buttonManager_Game.isGamePaused == false)
        {
            Time.timeScale = 1.5f;
        }
        
    }

    private void ArcherSpawner()
    {
       
        archerSpawnTimer -= Time.deltaTime;
        
        if (archerSpawnTimer <= 0 && isArcherAlive==false)
        {
            isArcherAlive = true;
            
            GameObject archerInstance = Instantiate(archerPrefab);
            archerInstance.transform.parent = transform;

            Archer.OnArcherDeathEvent += ArcherDead;
          
            archerInstance.transform.position = new Vector2(
                Random.Range(player.transform.position.x+2f, horizontalExtent - 2f),
                Random.Range(-verticalExtent + 1.5f, verticalExtent - 1.5f)
                );
        }
    }

    void WalkingEnemySpawn()
    {
        walkingEnemySpawnTimer -= Time.deltaTime;

        if (walkingEnemySpawnTimer <= 0)
        {
            walkingEnemySpawnTimer =walkingEnemyFrequency;
            GameObject walkingEnemyInstance = Instantiate(walkingEnemyPrefab);
            walkingEnemyInstance.transform.position = new Vector2(horizontalExtent + 5f,-3f );//To land correctly on ground
            walkingEnemyInstance.transform.parent = transform;

        }
    }

    void ArcherDead()
    {
        
        isArcherAlive = false;
        archerSpawnTimer = archerFrequncy;
    }

    void UpdateScoreAndAmmo(uint eScore,int ammo)
    {
        
        PersistantManager.PM.currentScore += eScore;
        PersistantManager.PM.CheckHighScore();
        PersistantManager.PM.Load();
        player.GetComponent<Player>().bulletCount += ammo;
        // Debug.Log("Enemy Dead");
        if (eScore > 0)
        {
            player.GetComponent<AudioSource>().PlayOneShot(enemyDeathSound, 0.2f);
        }
    }

}
