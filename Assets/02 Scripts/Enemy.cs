using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget;
    LivingEntity targetEntity;
    NavMeshAgent pathFinder;
    public ParticleSystem hitEffect;
    public AudioClip deathSound, hitSound;
    Animator enemyAnimator;
    AudioSource enemyAudioPlayer;
    Renderer enemyRenderer;
    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    float lastAttackTime;
    public static int currentKill = 0;

    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        startingHealth = newHealth;
        health = newHealth;
        damage = newDamage;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null & !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    void Update()
    {
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            enemyAudioPlayer.PlayOneShot(hitSound);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
        currentKill = currentKill+1;
    }


    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}