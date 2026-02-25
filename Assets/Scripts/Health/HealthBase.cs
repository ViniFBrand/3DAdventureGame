using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;
    public bool destroyOnKill = false;
    [SerializeField] private float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public List<UIFillUpdater> uiHealthUpdater;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        ResetLife();
    }

    public void ResetLife()
    {
        _currentLife = startLife;
    }

    protected virtual void Kill()
    {
        if(destroyOnKill)
            Destroy(gameObject, 2f);

        OnKill?.Invoke(this);
    }

    #region DEBUG
    [NaughtyAttributes.Button]
    public void Damage()
    {
        Damage(5f);
    }
    #endregion

    public void Damage(float f)
    {
        _currentLife -= f;

        if (_currentLife <= 0)
        {
            Kill();
        }
        UpdateUI();
        OnDamage?.Invoke(this);
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }

    public void UpdateUI()
    {
        if(uiHealthUpdater != null)
        {
            uiHealthUpdater.ForEach(i => i.UpdateValue((float) _currentLife/startLife));
        }
    }
}
