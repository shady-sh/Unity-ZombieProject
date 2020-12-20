using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State {
        Ready,
        Empty,
        Reloading
    }
    
    public State state { get; private set; }
    public Transform fireTransform;
    public ParticleSystem muzzleFlashEffect, shellEjectEffect;
    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    public AudioClip shotClip, reloadClip;
    public float damage = 25f;
    private float fireDistance = 50f;
    public int ammoRemain = 100, magCapacity = 25, magAmmo;
    public float timeBetFire = 0.12f;
    public float reloadTime = 1f;
    private float lastFireTime;

    private void Awake() {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire() {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire) {
            lastFireTime = Time.time;
            Shot();
        }
    }

    void Shot() {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance)) {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null) {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo <= 0) {
            state = State.Empty;
            Reload();
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }

    public bool Reload() {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity) {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }
    
    private IEnumerator ReloadRoutine() {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);
        yield return new WaitForSeconds(reloadTime);
        int ammoToFill = magCapacity - magAmmo;
        if(ammoRemain < ammoToFill) {
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;
        state = State.Ready;
    }
}
