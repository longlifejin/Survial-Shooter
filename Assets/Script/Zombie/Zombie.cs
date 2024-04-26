using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Zombie : LivingGO
{
    public int typeNumber;

    public LayerMask whatIsTarget;
    private LivingGO targetGo;
    private NavMeshAgent navMesh;

    public float damage;
    public float probability;
    public float speed;
    public GameObject zombiePrefab;

    private Animator zombieAnimator;
    private AudioSource zombieAudioPlayer;
    public Renderer zombieRenderer;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public ParticleSystem hitEffect;
    private ObjectPool objectPool;

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
        navMesh.enabled = true;


    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
        base.speed = speed;
        navMesh.enabled = true;
        objectPool = GameMgr.Instance.objectPool;
    }

    private void Update()
    {
        zombieAnimator.SetBool("HasTarget", hasTarget);

        AnimatorStateInfo stateInfo = zombieAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 0.99f)
        {
            StartSinking();
        }
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
                Collider[] cols = Physics.OverlapSphere(transform.position, 100f, whatIsTarget);
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

        zombieAnimator.SetTrigger("Death");
        zombieAudioPlayer.PlayOneShot(deathSound);
    }

    public void StartSinking()
    {
        navMesh.enabled = false;

        float sinkSpeed = 1f;
        float sinkDepth = -5f;
        transform.position += Vector3.down * sinkSpeed * Time.deltaTime;

        if(transform.position.y <= sinkDepth)
        {
            // Destroy(gameObject);
            //TO-DO : 오브젝트 풀링 적용하기
            objectPool.ReturnToPool(this.gameObject, this.typeNumber);
        }
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
