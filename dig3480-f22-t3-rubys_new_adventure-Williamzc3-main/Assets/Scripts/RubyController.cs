using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 4.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public int ammo { get { return currentAmmo; } }
    public int currentAmmo;
    public TextMeshProUGUI ammoText;

    public ParticleSystem healthPickup;
    public ParticleSystem hitEffect;

    AudioSource audioSource;

    public AudioClip Cog;
    public AudioClip Damage;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip Footsteps;
    public AudioClip speedPotion;
    public AudioSource backgroundManager;

    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;
    public TextMeshProUGUI destroyedText;
    private int totemText = 0;
    public GameObject newSceneTextObject;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject altMusic1Object;
    public GameObject altMusic2Object;
    public GameObject fixedRobotObject;


    bool gameOver;
    bool winGame;
    public static int level = 1;
    public float boostTimer;
    private bool boosting;

    private int scoreText = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        fixedText.text = "Fixed Robots: " + scoreText.ToString() + "/12";
        destroyedText.text = "Destroyed Totems: " + totemText.ToString() + "/12";
        winTextObject.SetActive(false);
        newSceneTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        altMusic1Object.SetActive(false);
        altMusic2Object.SetActive(false);
        fixedRobotObject.SetActive(false);
        gameOver = false;
        winGame = false;
        level = 1;

        rigidbody2d = GetComponent<Rigidbody2D>();
        AmmoText();

        boostTimer = 0;
        boosting = false;
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (scoreText >= 2)
                    {
                        SceneManager.LoadScene("Level 2");
                        level = 2;
                    }
                    else
                    {
                        character.DisplayDialog();
                    }
                }
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }
            /*RaycastHit2D hit2 = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC2"));
            if (hit2.collider != null)
            {
                NonPlayerCharacter2 character = hit.collider.GetComponent<NonPlayerCharacter2>();
                if (hit2.collider != null)
                {
                    character.DisplayDialog();
                }
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }*/
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (winGame == true)
            {
                SceneManager.LoadScene("Level 1");
            }
        }

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= 8)
            {
                speed = 4.0f;
                boostTimer = 0;
                boosting = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            audioSource.clip = winMusic;
            audioSource.Play();
            backgroundManager.Stop();
            altMusic1Object.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            audioSource.Stop();
            backgroundManager.Play();
            altMusic1Object.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            audioSource.clip = loseMusic;
            audioSource.Play();
            backgroundManager.Stop();
            altMusic2Object.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            audioSource.Stop();
            backgroundManager.Play();
            altMusic2Object.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();

            if (currentAmmo > 0)
            {
                ChangeAmmo(-1);
                AmmoText();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Play();
            audioSource.loop = true;
        }


        if (Input.GetKeyUp(KeyCode.W))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            audioSource.clip = Footsteps;
            audioSource.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount <= -1)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            audioSource.PlayOneShot(Damage);
            Instantiate(hitEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            loseTextObject.SetActive(true);
            speed = 0.0f;
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            audioSource.clip = loseMusic;
            audioSource.Play();
            backgroundManager.Stop();
            gameOver = true;
        }
        Debug.Log("Ruby's Health : " + currentHealth + "/" + maxHealth);
    }

    public void FixedRobots(int amount)
    {
        scoreText += amount;
        fixedText.text = "Fixed Robots: " + scoreText.ToString() + "/12";

        Debug.Log("Fixed Robots: " + scoreText);


        if (scoreText == 12 && level == 1)
        {
            newSceneTextObject.SetActive(true);
        }

        if (scoreText == 2 && level == 2)
        {
            fixedRobotObject.SetActive(true);
        }
    }

    public void DestroyedTotems(int amount)
    {
        totemText += amount;
        destroyedText.text = "Destroyed Totems: " + totemText.ToString() + "/12";

        Debug.Log("Destroyed Totems: " + totemText);


        if (totemText == 12)
        {
            gameOver = true;
            winGame = true;
            winTextObject.SetActive(true);
            speed = 0.0f;
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            audioSource.clip = winMusic;
            audioSource.Play();
            backgroundManager.Stop();

        }
    }

    public void PlayEffect(ParticleSystem effect)
    {
        healthPickup.Play(effect);
    }


    public void ChangeAmmo(int amount)
    {
        currentAmmo = Mathf.Abs(currentAmmo + amount);
        Debug.Log("Ammo: " + currentAmmo);
    }

    public void AmmoText()
    {
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }

    void Launch()
    {
        if (currentAmmo > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");

            audioSource.PlayOneShot(Cog);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "speedBoost")
        {
            audioSource.clip = speedPotion;
            audioSource.Play();
            boosting = true;
            speed = 6.0f;
            Destroy(other.gameObject);
        }
    }
}