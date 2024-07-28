using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public float activationDistance = 20f;
    public bool activateOnStart = false;
    public float moveSpeed = 3.5f;
    public int maxHealth = 1;
    public ParticleSystem hitEffect;
    public AudioSource attackSound;
    public float attackSoundCooldown = 2f;
    public float stopDistance = 2.0f; // Boss'un duracaðý mesafe

    private bool isActive = false;
    private Vector3 startPosition;
    private int currentHealth;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackSoundTime = 0f;
    private Collider wolfCollider;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        wolfCollider = GetComponent<Collider>();
        startPosition = transform.position;
        agent.speed = moveSpeed;
        currentHealth = maxHealth;

        if (hitEffect == null)
        {
            Debug.LogError("Hit Effect is not assigned in the inspector!");
        }
        else
        {
            hitEffect.Stop();
        }

        AdjustPositionToGround();

        if (activateOnStart)
        {
            Activate();
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isActive && !activateOnStart)
        {
            if (distanceToPlayer <= activationDistance)
            {
                Activate();
            }
        }
        else if (isActive)
        {
            if (distanceToPlayer <= activationDistance)
            {
                if (distanceToPlayer > stopDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.ResetPath();
                }

                if (distanceToPlayer <= stopDistance)
                {
                    if (!isAttacking)
                    {
                        int attackType = Random.Range(1, 3);
                        animator.SetInteger("AttackType", attackType);
                        animator.SetBool("IsAttacking", true);
                        isAttacking = true;
                    }
                }
                else
                {
                    if (isAttacking)
                    {
                        animator.SetBool("IsAttacking", false);
                        isAttacking = false;
                    }
                }
            }
            else
            {
                agent.SetDestination(startPosition);
                if (Vector3.Distance(transform.position, startPosition) < 0.1f)
                {
                    isActive = false;
                    agent.isStopped = true;
                }
            }
        }

        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);
    }

    void AdjustPositionToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            agent.Warp(navHit.position);
        }
        else
        {
            Debug.LogError("Failed to find a valid NavMesh position for the wolf.");
        }
    }

    void Activate()
    {
        isActive = true;
        agent.isStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            PlayHitEffect();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartAttackSound();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContinueAttackSound();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAttackSound();
        }
    }

    void StartAttackSound()
    {
        if (attackSound != null && !isAttacking && Time.time >= lastAttackSoundTime + attackSoundCooldown)
        {
            attackSound.Play();
            isAttacking = true;
            lastAttackSoundTime = Time.time;
        }
    }

    void ContinueAttackSound()
    {
        if (attackSound != null && !attackSound.isPlaying && Time.time >= lastAttackSoundTime + attackSoundCooldown)
        {
            attackSound.Play();
            lastAttackSoundTime = Time.time;
        }
    }

    void StopAttackSound()
    {
        if (attackSound != null)
        {
            attackSound.Stop();
            isAttacking = false;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.Stop();
            hitEffect.Play();
            Debug.Log("Hit effect played");
        }
        else
        {
            Debug.LogError("Hit Effect is null!");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
