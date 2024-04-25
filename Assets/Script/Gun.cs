using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform; //총알 발사 위치

    public ParticleSystem fireEffect;
    public LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer; //총 사운드
    public AudioClip shotClip;
    private float fireDistance = 200f;

    private float lastFireTime; // 마지막으로 총 쏜 시점
    private float fireInterval = 0.2f;
    private float damage = 30f;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;

        lastFireTime = 0;
    }

    public void Fire()
    {
        if(Time.time > lastFireTime + fireInterval)
        {
            lastFireTime = Time.time;

            var hitPoint = Vector3.zero;
            var ray = new Ray(fireTransform.position, fireTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance))
            {
                hitPoint = hitInfo.point; //충돌 지점
                var damagable = hitInfo.collider.GetComponent<IDamageable>();
                damagable?.OnDamage(damage, hitPoint, hitInfo.normal);
            }
            else
            {
                hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
            }

            StartCoroutine(ShotEffect(fireTransform.position + fireTransform.forward * 10f));
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        fireEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);

        yield return new WaitForSeconds(0.02f);

        bulletLineRenderer.enabled = false;
    }

}
