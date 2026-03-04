using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAbilityShoot : PlayerAbilityBase
{
    public GunBase[] gunBase;
    public Transform gunPosition;

    private GunBase _currentGun;

    protected override void Init()
    {
        base.Init();

        CreateGun(0);

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();

        inputs.Gameplay.ChangeGun.performed += ctx => ChangeGun(ctx);

    }


    /*private void CreateGun()
    {
        _currentGun = Instantiate(gunBase, gunPosition);

        _currentGun.transform.localPosition = _currentGun.transform.localEulerAngles = Vector3.zero;
    }*/

    public void CreateGun(int i)
    {
        _currentGun = Instantiate(gunBase[i], gunPosition);

        _currentGun.transform.localPosition = _currentGun.transform.localEulerAngles = Vector3.zero;
    }

    private void ChangeGun(InputAction.CallbackContext ctx)
    {
        var control = ctx.control;
        if (control.displayName == "1")
        {
            CreateGun(0);
        }
        else if (control.displayName == "2")
        {
            CreateGun(1);
        }

    }

    private void StartShoot()
    {
        _currentGun.StartShoot();
        ShakeCamera.Instance.Shake();
        //Debug.Log("Start Shoot");
    }
    private void CancelShoot()
    {
        //Debug.Log("Cancel Shoot");
        _currentGun.StopShoot();
    }
}
