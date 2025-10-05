using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectiles;

    public Transform positionToShoot;
    public float timeBetweenShoot = .3f;
    public float speed = 50f;
    public KeyCode shootKey = KeyCode.Q;

    private Coroutine _currentCoroutine;


    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShoot);
        }
    }


    public virtual void Shoot()
    {
        //if(randomShoot != null) randomShoot.PlayRandom();

        var projectile = Instantiate(prefabProjectiles);
        projectile.transform.position = positionToShoot.position;
        projectile.transform.rotation = positionToShoot.rotation;
        projectile.speed = speed;
    }

    public void StartShoot()
    {
        StopShoot();
        _currentCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopShoot()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }
}
