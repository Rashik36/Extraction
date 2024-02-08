using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator animEnemy;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float walkSpeed;
    public float chaseSpeed;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private PlayerController playerController;
    public float damage;
    public float damageRate;
    public float nextTimetoDamage = 0f;

    public float health;
    public bool isRunning;
    public bool isAttacking;
    public bool isTakingDamage;
    public bool isAlert = false;
    public AudioSource chaseAudio;
    private Achievements achievements;

    private void Awake() {
        player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        animEnemy = GetComponent<Animator>();
        achievements = GameObject.Find("Achievements").GetComponent<Achievements>();
    }

    void FixedUpdate()
    {
        if(health <= 0f){
            achievements.DeadEnemiesCount();
            chaseAudio.enabled = false;
            animEnemy.SetTrigger("Death");
            isAlert = false;
            nav.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            this.enabled = false;
        }
        
    }

    private void Start() {
        health = 10f;
        StartCoroutine(UpdateEnemyHealth());
    }

    void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInSightRange && !playerInAttackRange && health > 0f){
            Patroling();
            isRunning = false;
            isAttacking = false;
            chaseAudio.enabled = false;
        }

        if(playerInSightRange && !playerInAttackRange && health > 0f || isAlert){
            chaseAudio.enabled = true;
            StartCoroutine(ChasePlayer());
            isRunning = true;
            isAttacking = false;
            
        }

        if(playerInAttackRange && playerInSightRange && health > 0f){
            AttackPlayer();
            isAttacking = true;
            isRunning = false;
        }

        EnemyAnimation();

    }

    IEnumerator UpdateEnemyHealth(){
        yield return new WaitForSeconds(2f);
        health = (float)GlobalEnemy.enemyHealth;
    }

    private void Patroling(){
        nav.SetDestination(transform.position);
    }
    //     nav.speed = walkSpeed;
    //     if(!walkPointSet){
    //         SearchWalkPoint();
    //     }

    //     if(walkPointSet){
    //         nav.SetDestination(walkPoint);
    //     }

    //     Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //     if(distanceToWalkPoint.magnitude < 1f){
    //         walkPointSet = false;
    //     }
    // }

    // private void SearchWalkPoint(){
    //     float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //     float randomX = Random.Range(-walkPointRange, walkPointRange);

    //     walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //     if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
    //         walkPointSet = true;
    //     }

    // }

    IEnumerator ChasePlayer(){
        if(isTakingDamage){
            nav.SetDestination(transform.position);
            yield return new WaitForSeconds(1f);
            nav.SetDestination(transform.position);
            nav.speed = 0f;
            isTakingDamage = false;
        }
        nav.speed = chaseSpeed;
        nav.SetDestination(player.position);
    }

    public void StopChasing(){
        if(nav == null){
            return;
        }
        isTakingDamage = true;
        StartCoroutine(ChasePlayer());

    }

    private void AttackPlayer(){
        nav.SetDestination(transform.position);
        if(Time.time >= nextTimetoDamage){
            nextTimetoDamage = Time.time + 1f/damageRate;
            playerController.GotHurt(damage);
        }
        //transform.LookAt(player);

        if(!alreadyAttacked){
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack(){
        alreadyAttacked = false;
    }

    public void TakeDamage(float amount){
        health = health - amount;
        if(health > 0){
            animEnemy.SetTrigger("Damage");
        }
    }

    private void EnemyAnimation(){
        animEnemy.SetBool("isRunning", isRunning);
        animEnemy.SetBool("isAttacking", isAttacking);
    }
}
