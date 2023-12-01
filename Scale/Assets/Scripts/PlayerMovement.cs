using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.5f;

    public Animator playerAnim;

    public AudioClip[] punchSounds = new AudioClip[3];
    public AudioSource audioSource;
    
    public float scaleToAdd = 0.1f;

    public static bool playerIsPunching = false;

    private GameManager gameManager;


    // Start is called before the first frame update
    private void Awake() 
    {
        
    }
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        gameManager.StartCoroutine("Countdown");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Punch();
        ChangeMouseVisibily();
        PlayerBorders();

    }

    private void MovePlayer()
    {
        if(GameManager.isPlaying == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            transform.Translate(Vector2.right * horizontalInput* speed * Time.deltaTime);
            transform.Translate(Vector2.up * verticalInput * speed * Time.deltaTime);
        }
    }

    private void PlayerBorders()
    {
        float playerLimitX = 8.3f;
        float playerLimitY = 4.4f;
        if(transform.position.x <= -playerLimitX)
        {
            transform.position = new Vector2(-playerLimitX, transform.position.y);
        }
        if(transform.position.x >= playerLimitX)
        {
            transform.position = new Vector2(playerLimitX, transform.position.y);
        }

        if(transform.position.y <= -playerLimitY)
        {
            transform.position = new Vector2(transform.position.x, -playerLimitY);
        }
        if(transform.position.y >= playerLimitY)
        {
            transform.position = new Vector2(transform.position.x, playerLimitY);
        }
    }

    private void Punch()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Mouse0) && GameManager.isPlaying == true)
        {
            playerAnim.SetTrigger("Left");
            playerIsPunching = true;
            GenerateRandomPunchSound();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Mouse1) && GameManager.isPlaying == true)
        {
            playerAnim.SetTrigger("Right");
            playerIsPunching = true;
            GenerateRandomPunchSound();
        }
    }

    private void GenerateRandomPunchSound()
    {
        int i = Random.Range(0,3);
        audioSource.PlayOneShot(punchSounds[i]);
    }
    
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Enemy Arms") && EnemyMovement.enemyIsPunching == true)
        {
            this.transform.localScale += new Vector3(scaleToAdd, scaleToAdd, 0);
            EnemyMovement.enemyIsPunching = false;
            gameManager.ReducePlayerHealth(1);
        }
    }

    private void ChangeMouseVisibily()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.isPlaying = false;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }

}
