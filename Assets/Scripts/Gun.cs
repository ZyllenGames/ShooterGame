using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int AmmuNum;

    [Header("Projectile Spawn")]
    public Projectile Projectile;
    public Transform Muzzle;
    Transform m_ShootAt;
    public float ProjectileSpeed = 10f;
    public int MiTimeBetweenShots = 200;

    [Header("Shell Spawn")]
    public Shell Shell;
    public Transform ShellSpawn;

    public float RecoilForce = 0.5f;
    public float RecoilRotation = -15f;

    float m_NextShotTime = 0f;

    public AudioClip ShootSound;
    public AudioClip ReloadSound;

    bool m_IsPlayerShot;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Crossfire") != null)
            m_ShootAt = GameObject.FindGameObjectWithTag("Crossfire").transform;
        m_IsPlayerShot = true;
    }

    public void SetShootAt(Transform transform)
    {
        m_ShootAt = transform;
        m_IsPlayerShot = false;
    }

    public void ShotProjectiles()
    {
        if (Time.time > m_NextShotTime)
        {
            m_NextShotTime = Time.time + (float)MiTimeBetweenShots / 1000f;

            //Projecttile Spawn
            Muzzle.LookAt(new Vector3(m_ShootAt.position.x, Muzzle.position.y, m_ShootAt.position.z));
            Projectile newprojectile = Instantiate(Projectile, Muzzle.position, Muzzle.rotation);
            newprojectile.Speed = ProjectileSpeed;
            if (m_IsPlayerShot)
                newprojectile.IsPlayerShot = true;
            else
            {
                newprojectile.IsPlayerShot = false;
                newprojectile.Speed = 4f;
            }

            //Shell Spawn
            Shell newshell = Instantiate(Shell, ShellSpawn.position, ShellSpawn.rotation);

            //Recoil
            Transform gungraphics = transform.GetChild(0);
            gungraphics.Translate(Vector3.back * RecoilForce);
            gungraphics.Rotate(Vector3.right * RecoilRotation);

            if(AudioManager.Instance != null)
                AudioManager.Instance.PlaySound(ShootSound, transform.position);

            AmmuNum--;
            if (AmmuNum <= 0)
                Destroy(gameObject);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound("Impact", transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("Get Gun");
            other.gameObject.GetComponent<GunController>().EquiptGun(this);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound(ReloadSound, transform.position);
            Destroy(gameObject);
        }
    }

    public void StartAnimation()
    {
        StartCoroutine(Zoom());
    }

    IEnumerator Zoom()
    {
        Transform t = transform.GetChild(0);

        while(true)
        {
            float tartime = 0.7f;
            float curtime = 0;
            float target = 1.2f;
            float curScale = 1f;
            while(curtime < tartime)
            {
                curtime += Time.deltaTime;
                float ratio = curtime / tartime;
                curScale = Mathf.Lerp(1f, target, ratio);
                t.localScale = Vector3.one * curScale;
                yield return null;
            }
            curtime = 0;
            while (curtime < tartime)
            {
                curtime += Time.deltaTime;
                float ratio = curtime / tartime;
                curScale = Mathf.Lerp(target, 1f, ratio);
                t.localScale = Vector3.one * curScale;
                yield return null;
            }
        }
    }
}
