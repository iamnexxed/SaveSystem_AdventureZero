using System.Collections;
using UnityEngine;
using System;


[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    [SerializeField] GameObject collisionObject;
    CharacterController controller;

    Animator animator;

    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    public float gravity;

    public bool debugCheckpointPos;
    public Transform tempCheckpoint;

    // Setting type to 0 means use SimpleMove()
    // Setting type to 1 means use Move()
    [SerializeField] readonly int type = 1;

    Vector3 moveDirection;

    // Make gun shoot
    public float projectileSpeed;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;

    // Raycast variables
    public Transform thingToLookFrom;
    public float lookAtDistance;

    public int checkPointsCrossed = 0;

    public bool disableInput = false;

    public int initialLives = 3;
    public int currentLives;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerTransform = transform;
        currentLives = initialLives;
        try
        {
            animator = GetComponent<Animator>();

            animator.applyRootMotion = false;
            controller = GetComponent<CharacterController>();
            
            controller.minMoveDistance = 0.0f;

            if (speed <= 0)
            {
                speed = 6.0f;
                Debug.Log("Speed not set on " + name + " defaulting to " + speed);
            }

            if (jumpSpeed <= 0)
            {
                jumpSpeed = 8.0f;
                Debug.Log("JumpSpeed not set on " + name + " defaulting to " + jumpSpeed);
            }

            if (rotationSpeed <= 0)
            {
                rotationSpeed = 10.0f;
                Debug.Log("RotationSpeed not set on " + name + " defaulting to " + rotationSpeed);
            }

            if (gravity <= 0)
            {
                gravity = 9.81f;
                Debug.Log("Gravity not set on " + name + " defaulting to " + gravity);
            }

            moveDirection = Vector3.zero;

            //throw new ArgumentNullException("Whoops");

            if (projectileSpeed <= 0)
            {
                projectileSpeed = 6.0f;
                Debug.Log("ProjectileSpeed not set on " + name + " defaulting to " + projectileSpeed);
            }

            if (!projectilePrefab)
            {
                Debug.Log("Missing projectilePrefab on " + name);
            }

            if (!projectileSpawnPoint)
            {
                Debug.Log("Missing projectileSpawnPoint on " + name);
            }

            if (lookAtDistance <= 0)
            {
                lookAtDistance = 10.0f;
                Debug.Log("LookAtDistance not set on " + name + " defaulting to " + lookAtDistance);
            }

            collisionObject.SetActive(false);

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
        /*catch(ArgumentNullException e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            Debug.LogError("Something bad happened");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!Health.instance.isAlive)
            return;*/

        if (disableInput)
            return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("1HShoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("2HShoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("CastSpell") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death") || animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
        {
            return;
        }

        if (type == 0)
        {
            // Use if not using MouseLook.CS
            //transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            Vector3 forward = transform.TransformDirection(Vector3.forward);

            float curSpeed = Input.GetAxis("Vertical") * speed;

            controller.SimpleMove(forward * curSpeed);

        }
        // Using Move()
        else if(type == 1)
        {
            if (controller.isGrounded)
            {
                // if (Input.GetAxis("Vertical") > 0)
                    moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
                // else
                    // moveDirection = Vector3.zero;

                // Use if not using MouseLook.CS
                //transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection *= speed;

                if (Input.GetButtonDown("Jump"))
                    moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);


        }

        // Attacking Animations
/*
        #region Attacks
        if (Input.GetButtonDown("Fire1")) // Fire One Handed
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                // LightFire();
                animator.SetTrigger("SingleHandShoot");
        }

        if (Input.GetButtonDown("Fire2")) // Fire Double Handed
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                // HeavyFire();
                animator.SetTrigger("DoubleHandShoot");
        }

        if (Input.GetButtonDown("Fire3")) // Cast Spell
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                animator.SetTrigger("Spell");
        }

        if (Input.GetKeyDown(KeyCode.J)) // Punch
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                animator.SetTrigger("Punch");
        }

        if (Input.GetKeyDown(KeyCode.K)) // Kick
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                animator.SetTrigger("Kick");
        }
        #endregion*/


        /* // Usage Raycast
         // - GameObject needs a Collider
         RaycastHit hit;

         if (!thingToLookFrom)
         {
             Debug.DrawRay(transform.position, transform.forward * lookAtDistance, Color.red);

             if (Physics.Raycast(transform.position, transform.forward, out hit, lookAtDistance))
             {
                 Debug.Log("Raycast hit: " + hit.transform.name);
             }
         }
         else
         {
             Debug.DrawRay(thingToLookFrom.transform.position, thingToLookFrom.transform.forward * lookAtDistance, Color.yellow);

             if (Physics.Raycast(thingToLookFrom.transform.position, thingToLookFrom.transform.forward, out hit, lookAtDistance))
             {
                 Debug.Log("Raycast hit: " + hit.transform.name);
             }
         }*/

        animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    public void LightFire()
    {
        if(projectilePrefab && projectileSpawnPoint)
        {
            // Debug.Log("LightFire()");
            // Make bullet
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Shoot bullet
            temp.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);

        }
    }

    public void HeavyFire()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            // Debug.Log("HeavyFire()");
            // Make bullet
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Shoot bullet
            temp.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
    }

    public void SpellFire()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            // Debug.Log("SpellFire()");
            // Make bullet
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Shoot bullet
            temp.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
    }

    public void EnableHit()
    {
        collisionObject.SetActive(true);
    }

    public void DisableHit()
    {
        collisionObject.SetActive(false);
    }

    public void SaveGamePrepare()
    {
        LoadSaveManager.GameStateData.DataPlayer data = GameManager.StateManager.gameState.player;

        // Save data
        // data.collectedCash = collectedCash;
        // data.collectedWeapon = collectedWeapon;
        // data.health = Health.instance.life;
        // data.lives = currentLives;
        

        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y + 10;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;

        // Debug.Log("Player Data Saved");
        // Debug.Log(data.posRotScale.posX + " " + data.posRotScale.posY + " " + data.posRotScale.posZ);
    }

    public void LoadComplete()
    {
        if(debugCheckpointPos)
        {
            transform.position = tempCheckpoint.position;
        }
        else
        {
            //disableInput = true;
            LoadSaveManager.GameStateData.DataPlayer data = GameManager.StateManager.gameState.player;
            // Debug.Log("Data for Player fetched : " + data.posRotScale.posX + " " + data.posRotScale.posY + " " + data.posRotScale.posZ);
            // Health.instance.life = data.health;
            // currentLives = data.lives;
            // collectedCash = data.collectedCash;
            // if(weaponPickUp)
            // weaponPickUp.SendMessage("OnTriggerEnter2D", GetComponent<Collider2D>()), SendMessageOptions.DontRequireReceiver)

            // Set Transform
            transform.position = new Vector3(data.posRotScale.posX, data.posRotScale.posY, data.posRotScale.posZ);
            // Debug.Log("Current Position set : " + transform.position);
            transform.localRotation = Quaternion.Euler(data.posRotScale.rotX, data.posRotScale.rotY, data.posRotScale.rotZ);

            transform.localScale = new Vector3(data.posRotScale.scaleX, data.posRotScale.scaleY, data.posRotScale.scaleZ);
            Debug.Log("Player Data Loaded");
            
        }
        StartCoroutine(StartInput());
        //disableInput = false;
    }

    IEnumerator StartInput()
    {
        yield return new WaitForSeconds(0.1f);
        disableInput = false;
    }

}
