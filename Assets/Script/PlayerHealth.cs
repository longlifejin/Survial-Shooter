using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingGO
{
    public AudioClip deathClip;
    public AudioClip hitClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    public event Action onRespawn;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        UpdateHealth(startHealth, false);
    }

    private void OnEnable()
    {
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        Debug.Log(health);
        Debug.Log(GameMgr.Instance.isGameOver);

    }

    private void Update()
    {
        
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead)
            return;
        base.OnDamage(damage, hitPoint, hitNormal);
        playerAudioPlayer.PlayOneShot(hitClip);
    }

    public override void OnDie()
    {
        base.OnDie();
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Death");
        GameMgr.Instance.GameOver();

        playerMovement.enabled = false;
        playerShooter.enabled = false;

    }



    public void RestartLevel()
    {
        if(onRespawn != null)
        {
            onRespawn();
        }
        Debug.Log("Restart");
    }

}
