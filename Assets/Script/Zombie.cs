using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingGO
{
    public LayerMask whatIsTarget;
    private LivingGO targetGo;
    private NavMeshAgent navMesh;

    public float damage = 5f;

    public ParticleSystem hitEffect;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private Animator zombieAnimator;
    private AudioSource zombieAudioPlayer;
    public Renderer zombieRenderer;

    private float attackInterval = 1f;
    private float lastAttackTime;

    private bool hasTarget
    {
        get
        {
            if(targetGo != null && !targetGo.dead )
            {
                return true;
            }
            else { return false; }
        }
    }

    private void Awake()
    {
        zombieAnimator = GetComponent<Animator>();  
        navMesh = GetComponent<NavMeshAgent>();
        zombieRenderer = GetComponent<Renderer>();
        zombieAudioPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if(hasTarget)
            {
                navMesh.isStopped = false;
                navMesh.SetDestination(targetGo.transform.position);
            }
            else
            {
                navMesh.isStopped =true;
                Collider[] cols = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                foreach(Collider col in cols)
                {
                    LivingGO livingGo = col.GetComponent<LivingGO>();

                    if(livingGo != null && !livingGo.dead)
                    {
                        targetGo = livingGo;
                        break;
                    }
                }

            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        zombieAudioPlayer.PlayOneShot(hitSound);

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void OnDie()
    {
        base.OnDie();

        var cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }

        navMesh.isStopped = true;
        navMesh.enabled = false;

        zombieAnimator.SetTrigger("Die");
        zombieAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
       if(!dead && Time.time > lastAttackTime + attackInterval)
        {
            var entity = other.GetComponent<LivingGO>();
            if (entity != null && entity == targetGo)
            {
                var pos = transform.position;
                pos.y += 1f;
                var hitPoint = other.ClosestPoint(pos);
                var hitNormal = transform.position - targetGo.transform.position;
                entity.OnDamage(damage, hitPoint, hitNormal.normalized);
                lastAttackTime = Time.time;
            }
        }
    }
}
