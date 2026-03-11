using Animation;
using Boss;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public Collider collider;
        public FlashColor flashColor;
        public ParticleSystem particleSystem;

        public float startLife = 10f;
        public bool lookAtPlayer = false;

        [Header("Trigger")]
        public String tagToComparePlayer = "Player";
        public bool enemyStarted = false;

        [SerializeField] private float _currentLife;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;
        public GameObject enemyGraphics;

        [Header("Start Animation")]
        public float startAnimationDuration = .2f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        private Player _player;

        private void Awake()
        {
            enemyGraphics.SetActive(false);
        }

        private void Start()
        {
            _player = GameObject.FindObjectOfType<Player>();
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();
        }

        protected virtual void Kill() 
        {
            OnKill();
        }
        protected virtual void OnKill() 
        {
            if(collider != null) collider.enabled = false;
            Destroy(gameObject, 2f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (particleSystem != null) particleSystem.Emit(15);


            _currentLife -= f;

            if(_currentLife <= 0)
            {
                Kill();
            }
        }

        #region ANIMATION
        private void BornAnimation()
        {
            transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
        }

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimatinByTrigger(animationType);
        }
        #endregion

        public void Damage(float damage)
        {
            Debug.Log("Damage");
            OnDamage(damage);
        }

        public void Damage(float damage, Vector3 dir)
        {
            OnDamage(damage);
            transform.DOMove(transform.position - dir, .1f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player p = collision.transform.GetComponent<Player>();

            if (p != null)
            {
                p.healthBase.Damage(1);
            }
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }
        }


        IEnumerator StartEnemy()
        {
            enemyGraphics.SetActive(true);
            Init();
            if (startWithBornAnimation)
                BornAnimation();
            yield return new WaitForSeconds(1.5f);
        }

        #region TRIGGER
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == tagToComparePlayer && !enemyStarted)
            {
                StartCoroutine(StartEnemy());
                enemyStarted = true;
            }
            if (other.tag == tagToComparePlayer)
            {
                transform.LookAt(_player.transform.position);
            }
        }
        #endregion
    }
}
