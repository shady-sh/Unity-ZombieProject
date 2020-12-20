using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeath : LivingEntity
{
    public Slider healthSlider;
    public AudioClip deathClip, hitClip, itemPickupClip;
    AudioSource playerAudioPlayer;
    Animator playerAnimator;
    PlayerMovement playerMovement;
    PlayerShooter playerShooter;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = health;
    }

    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                item.Use(gameObject);
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.enabled == true)
        {
            if (Enemy.currentKill >= 6)
            {
                healthSlider.gameObject.SetActive(false);
                playerAnimator.SetTrigger("Die");
                playerMovement.enabled = false;
                playerShooter.enabled = false;
            }
        }
    }
}