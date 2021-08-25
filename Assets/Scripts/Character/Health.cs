using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health instance;
    public int life = 5;

    int startLives;

    public bool isAlive;
    public Image healthImage;

    Animator animator;

   

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startLives = life;
        animator = GetComponent<Animator>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Init();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        healthImage.fillAmount = (float)life / (float)startLives;

        if (!isAlive)
            return;

        if(Input.GetKeyDown(KeyCode.L))
        {
            DamagePlayer();
        }
        
    }

    public void Init()
    {
        life = startLives;
    }

    public void IncreaseHealth(int value = 1)
    {
        if(life < startLives)
        {
            life += value;
        }
        if(life >= startLives)
        {
            life = startLives;
        }
    }

    public void DamagePlayer(int value = 1)
    {
        // Debug.Log("Ouch : Damaged by " + value);
        if (life > 0)
            life -= value;

        if (life <= 0)
        {
            PlayerDead();
            isAlive = false;
        }
    }

    public void PlayerDead()
    {
        animator.SetTrigger("Death");
    }
}
