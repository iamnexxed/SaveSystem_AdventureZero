using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    [SerializeField] Image enemyHealthImage;
    [HideInInspector] public bool isAlive;

    NavMeshAgent agent;
    Rigidbody rb;

    Animator animator;

    public GameObject target;

    enum EnemyState
    {
        Chase,
        Patrol,
        Attack
    };

    [SerializeField] EnemyState state;

    enum PatrolType
    {
        DistanceBased,
        TriggerBased
    };
    [SerializeField] PatrolType patrolType;


    // Path finding/making
    public bool autoGenPath;

    public string pathName;
    public GameObject[] path;

    public int pathIndex;
    public float distanceToNextNode;

    GameObject currentPatrolTarget;

    // Player Check
    Transform playerTransform;
    public float maxDistToChase = 7f;
    public float distToAttack = 2f;

    public GameObject attackCheckObject;

    // Spawn Collectibles/PowerUps
    public GameObject objectSpawner;
    public Transform objectSpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        animator.applyRootMotion = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        isAlive = true;

        playerTransform = GameObject.FindWithTag("Player").transform;
        state = EnemyState.Patrol;

        if (string.IsNullOrEmpty(pathName))
        {
            pathName = "PathNode";
        }

        if(distanceToNextNode <= 0)
        {
            distanceToNextNode = 1.0f;
        }

        if(state == EnemyState.Chase)
        {
            target = GameObject.FindWithTag("Player");
        }
        else if(state == EnemyState.Patrol)
        {
            if(autoGenPath)
            {
                path = GameObject.FindGameObjectsWithTag(pathName);

                if(path.Length > 0)
                {
                    target = path[pathIndex];
                }
            }
        }

        if(target)
        {
            agent.SetDestination(target.transform.position);
        }

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        enemyHealthImage.fillAmount = (float) currentHealth / (float) maxHealth;
        // Debug.Log("Current enemy health percent : " + enemyHealthImage.fillAmount);
        if (!isAlive)
            return;
        // Debug.Log("Distance to player : " + Vector3.Distance(playerTransform.position, transform.position));

        if (Vector3.Distance(playerTransform.position, transform.position) > maxDistToChase)
        {
            state = EnemyState.Patrol;
            target = currentPatrolTarget;
        }

        if(Health.instance.isAlive)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) <= maxDistToChase && Vector3.Distance(playerTransform.position, transform.position) > distToAttack)
            {
                state = EnemyState.Chase;
                target = playerTransform.gameObject;
                
            }
            if (Vector3.Distance(playerTransform.position, transform.position) <= distToAttack)
            {
                state = EnemyState.Attack;
                if(Vector3.Distance(playerTransform.position, transform.position) > 0.6f)
                    transform.LookAt(target.transform);
            }
        }
        else
        {
            animator.SetBool("shouldAttack", false);
            state = EnemyState.Patrol;
            target = currentPatrolTarget;
        }
        
        

        if (state == EnemyState.Patrol && patrolType == PatrolType.DistanceBased)
        {

            if(agent.remainingDistance < distanceToNextNode)
            {
                if(path.Length > 0)
                {
                    pathIndex++;
                    pathIndex %= path.Length;

                    target = path[pathIndex];
                    currentPatrolTarget = target;
                }
            }

        }
        
        if (target && state != EnemyState.Attack)
        {
            // Debug.Log("Enemy should move towards target");
            agent.SetDestination(target.transform.position);
            // Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }

        if (state == EnemyState.Attack)
        {
            animator.SetBool("shouldAttack", true);
        }
        else
        {
            animator.SetBool("shouldAttack", false);
        }

       
        animator.SetBool("IsGrounded", !agent.isOnOffMeshLink);
        animator.SetFloat("Speed", transform.TransformDirection(agent.velocity).magnitude);
    }

    public void DamageEnemy(int value = 1)
    {
        if (currentHealth > 0)
            currentHealth -= value;

        if (currentHealth <= 0)
        {
            EnemyDeath();
            
        }
    }

    public void EnemyDeath()
    {
        // Debug.Log("Enemy should die");
        animator.SetTrigger("Death");
        isAlive = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == EnemyState.Patrol && patrolType == PatrolType.TriggerBased)
        {
            if (agent.remainingDistance < distanceToNextNode)
            {
                if (path.Length > 0)
                {
                    pathIndex++;
                    pathIndex %= path.Length;

                    target = path[pathIndex];
                }
            }

        }
    }

    public void AttackStart()
    {
        attackCheckObject.SetActive(true);
    }

    public void AttackEnd()
    {
        attackCheckObject.SetActive(false);
    }

    public void SpawnPowerUp()
    {
        Instantiate(objectSpawner, objectSpawnLocation.position, objectSpawnLocation.rotation);
        Destroy(gameObject);
    }
}
