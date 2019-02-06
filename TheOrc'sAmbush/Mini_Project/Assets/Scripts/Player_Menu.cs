using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Menu : MonoBehaviour
{
    public float speed = 2f;
    Vector2 direction;

    float horizontalExtent;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        horizontalExtent = Camera.main.orthographicSize * Camera.main.aspect;
        direction = Vector2.right;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=(Vector3) direction * speed * Time.deltaTime;
        if(transform.position.x>horizontalExtent)
        {
            transform.position = new Vector2(-horizontalExtent, 2);
        }
    }
}
