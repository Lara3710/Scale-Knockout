using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    private float enemySpeed = 1.5f;
    private float maxPunchDistance = 1.5f;
    private float maxPunchDistanceToAdd = 0.2f;

    private float enemyScaleToAdd = 0.1f;

    public static bool enemyIsPunching = false;
    private float waitingTimeBetweenPunches = 5f;

    public Animator enemyAnim;
    public AudioSource enemyAudioSource;
    public AudioClip[] enemyPunchSounds = new AudioClip[3];

    private GameManager gameManager;

    private float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isSinglePlayer == true && GameManager.isPlaying == true)
        {
            LookAtPlayer();
            MoveTowardsPlayer();
        }
        else if(GameManager.isSinglePlayer == false)
        {
            MovePlayer();
            Punch();
            LookAtPlayerWithMouse();
        }

        EnemyBorders();
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

        private void Punch()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Mouse0) && GameManager.isPlaying == true)
        {
            enemyAnim.SetTrigger("Left");
            enemyIsPunching = true;
            GenerateRandomPunchSound();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Mouse1) && GameManager.isPlaying == true)
        {
            enemyAnim.SetTrigger("Right");
            enemyIsPunching = true;
            GenerateRandomPunchSound();
        }
    }

    private void LookAtPlayerWithMouse()
    {
        Vector3 mousePos;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float rotaitonZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.Euler(0, 0, rotaitonZ);
    }

    private void LookAtPlayer()
    {
        Transform arms = transform.GetChild(0);

        Vector2 direction = player.transform.position - transform.position;

        float rotaitonZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        arms.localRotation = Quaternion.Euler(0, 0, rotaitonZ);
    }

    private void MoveTowardsPlayer()
    {
        Vector2 difference = player.transform.position - transform.position;
        float distance = difference.magnitude;

        if(distance <= maxPunchDistance)
        {
            // if in range stop and punch

            EnemyPunch();
            
        }
        else if(distance > maxPunchDistance)
        {
            // if not in range move towards player

             transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
        }
    }

    private void EnemyPunch()
    {
        if(!enemyIsPunching)
        {
            int punchNum = Random.Range(0, 2);

            switch(punchNum)
            {
                case 0:
                    enemyAnim.SetTrigger("Left");
                    enemyIsPunching = true;
                    GenerateRandomPunchSound();
                    StartCoroutine(WaitBetweenPunches());
                    break;

                case 1:
                    enemyAnim.SetTrigger("Right");
                    enemyIsPunching = true;
                    GenerateRandomPunchSound();
                    StartCoroutine(WaitBetweenPunches());
                    break;
            }
            StartCoroutine(WaitBetweenPunches());
        }
    }

    private IEnumerator WaitBetweenPunches()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(waitingTimeBetweenPunches);
        enemyIsPunching = false;
    }

    private void EnemyBorders()
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

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Player Arms") && PlayerMovement.playerIsPunching == true)
        {
            this.transform.localScale += new Vector3(enemyScaleToAdd, enemyScaleToAdd, 0);
            maxPunchDistance += maxPunchDistanceToAdd;
            PlayerMovement.playerIsPunching = false;
            gameManager.ReduceEnemyHealth(1);
        }
    }

    
    private void GenerateRandomPunchSound()
    {
        int i = Random.Range(0,3);
        enemyAudioSource.PlayOneShot(enemyPunchSounds[i]);
    }
}
