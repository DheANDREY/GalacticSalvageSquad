using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed, maxSpeed, deltaSPD, brakeFactor = 0.25f;
    public float rotationSpeed = 100f;
    [SerializeField] private Transform playerPos, parentPortal, playerTransform;
    [SerializeField] private TextMeshProUGUI resoText, scoreText;    

    private Rigidbody2D rb;

    [SerializeField] private PortalHandler portalHandler;
    [SerializeField] private AudioManager audioManager;

    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
        resCounts = new int[4];
    }

    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        currentFuel = maxFuel;
    }

    private float moveHorizontal, moveVertical;
    public GameObject[] spawnIconItem;  public Transform posIcon;   public GameObject _inventoryMenu, restartMenu, audio;
    public Image fuelFill;  [SerializeField] private float maxFuel=100; private float currentFuel, accelMove=0.5f, progressFuel;
    void Update()
    {
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }

        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    Debug.Log("Masuk");
        //    GameObject icon =  Instantiate(spawnIconItem[3], transform.position, Quaternion.identity);
        //    icon.transform.SetParent(posIcon);
        //}

        if (Input.GetKey(KeyCode.Tab))
        {
            _inventoryMenu.SetActive(true);
        }
        else
        {
            _inventoryMenu.SetActive(false);
        }

        // Mendapatkan input arah horizontal dan vertikal
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Mendapatkan perubahan posisi berdasarkan input
        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);

        //MoveGridSystem();

        MoveFlexible();
        if (resoRemaining == 0)
        {            
            portalHandler.SpawnPortal();
            resoRemaining = 4;
        }

        if (maxFuel > 0)
        {
            currentFuel -= accelMove*Time.deltaTime* progressFuel;
            fuelFill.fillAmount = currentFuel / maxFuel;
        }

        if (currentFuel <= 0)
        {
            Debug.Log("You Lose");  audio.SetActive(false);
            restartMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        if (resoPoint < 30)
        {
            progressFuel = 1f;
            fuelRate = 1;
        }
        else if (resoPoint > 30 && resoPoint < 55)
        {
            progressFuel = 1.35f;
            fuelRate = 0.8f;
        }else if(resoPoint > 55 && resoPoint < 85)
        {
            progressFuel = 1.7f;
            fuelRate = 0.7f;
        }else if (resoPoint > 70 && resoPoint<105)
        {
            progressFuel = 2f;
            fuelRate = 0.6f;
        }
        else
        {
            progressFuel = 2.25f;
            fuelRate = 0.5f;
        }

        scoreText.text = resoPoint.ToString();
        resoText.text = resoRemaining.ToString();
    }

    private int resoPoint, resoRemaining=4; private float fuelRate;
    public static int[] resCounts;  public bool[] isPicked;
    public Inventory inventory;
    public GameObject[] itemButton;
    private Collider2D collidedObject;
    public TileGenerator _tileGenerator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("r1"))
        {
            collidedObject = collision;
            PickupedResources(0);
        }
        if (collision.gameObject.CompareTag("r2"))
        {
            collidedObject = collision;
            PickupedResources(1);
        }
        if (collision.gameObject.CompareTag("r3"))
        {
            collidedObject = collision;
            PickupedResources(2);
        }
        if (collision.gameObject.CompareTag("r4"))
        {
            collidedObject = collision;
            PickupedResources(3);
        }

        if (collision.gameObject.CompareTag("portal"))
        {
            Debug.Log("Masuk Portal");
            currentFuel += fuelRate*maxFuel;
            _tileGenerator.isGenerateNew = true;
            Vector3 newPosWin = new Vector3(0, Random.Range(0, 7), 0);            
            Destroy(collision.gameObject);
            portalHandler.SpawnPortalStart(newPosWin);
            playerTransform.transform.position = newPosWin;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Kena Obstacle");
            audioManager.PlaySFX_Hit();
            currentFuel -= 2.5f;
        }
    }

    private void PickupedResources(int resourceIndex)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            // Periksa apakah slot kosong atau tidak
            //if (!inventory.isFull[i])
            //{
                Destroy(collidedObject.gameObject);
                resoPoint += Random.Range(2, 5);
                //Debug.Log("Reso Point: " + resoPoint);
                resoRemaining--;
                //Debug.Log("Reso Remain: " + resoRemaining);
                if (!isPicked[resourceIndex])
                {
                    inventory.isFull[i] = true;
                    Instantiate(itemButton[resourceIndex], inventory.slots[i].transform, false);
                    //inventory.numTotal[i].SetActive(true);
                    resCounts[resourceIndex]++; Debug.Log("Res" + (resourceIndex + 1) + ": " + resCounts[resourceIndex]);
                }
                else
                {
                    resCounts[resourceIndex]++;
                    Debug.Log("Res" + (resourceIndex + 1) + ": " + resCounts[resourceIndex]);
                }                
                isPicked[resourceIndex] = true;
                break;
            //}
        }
    }



    private void MoveFlexible()
    {
        // Menghitung vektor gerak berdasarkan input
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * moveSpeed;
        if (movement != Vector2.zero)
        {
            audioManager.PlaySFX_Move();
            Vector2 velocityVector = rb.velocity;
            velocityVector += movement * deltaSPD * Time.deltaTime;
            velocityVector = Vector2.ClampMagnitude(velocityVector, maxSpeed);
            // Menggerakkan karakter
            rb.velocity = velocityVector;   accelMove = 0.85f;            
        }
        else // Jika tidak ada input, berkurang atau melakukan pengereman
        {
            // Mengurangi kecepatan secara perlahan atau melakukan pengereman
            audioManager.StopSFX_Move();
            Vector2 velocityVector = rb.velocity * (1 - brakeFactor);
            rb.velocity = velocityVector;   accelMove = 0.5f;
        }
        //rb.velocity = movement;
        // Rotasi karakter sesuai dengan arah gerak
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveVertical, moveHorizontal) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveGridSystem()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 12)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < 7)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position += new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            transform.position += new Vector3(0, -1, 0);
        }
        else
        {

        }
    }

    
}
