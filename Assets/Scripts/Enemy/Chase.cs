
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Chase : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float speed = 1f;
    [SerializeField] float noSpeedDistance = 3f;
    [SerializeField] Enemy current;
    Rigidbody body;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!current.isAlive)
            return;

        if (Vector3.Distance(transform.position, playerTransform.position) > noSpeedDistance)
        {
           
            MoveTowards(playerTransform.position);
            RotateTowards(playerTransform.position);
            animator.SetFloat("Speed", speed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void MoveTowards(Vector3 target)
    {
        // Debug.Log(Vector3.Distance(transform.position, target));
       
        Vector3 newPos = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
      
        body.MovePosition(newPos);
    }

    private void RotateTowards(Vector3 target)
    {

        Vector3 direction = target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        body.rotation = Quaternion.Euler(Vector3.up * (angle));
    }


}
