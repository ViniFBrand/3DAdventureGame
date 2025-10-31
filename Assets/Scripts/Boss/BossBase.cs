using DG.Tweening;
using Ebac.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss
{
    public enum BossAction
    {
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH
    }

    public class BossBase : MonoBehaviour
    {
        [Header("Animation")]
        public float startAnimationDuration = .5f;
        public Ease startAnimationEase = Ease.OutBack;
        public GameObject bossGraphics;

        [Header("Attack")]
        public int attackAmount = 5;
        public float timeBetweenAttacks = .5f;

        [Header("Trigger")]
        public String tagToComparePlayer = "Player";
        public bool bossStarted = false;

        public float speed = 5f;
        public List<Transform> waypoints;
        public int waypointsRandom;

        public HealthBase healthBase;

        private StateMachine<BossAction> stateMachine;
        private Player _player;

        private void Awake()
        {
            Init();
            healthBase.OnKill += OnBossKill;
            bossGraphics.SetActive(false);
            _player = GameObject.FindObjectOfType<Player>();
        }

        private void Init()
        {
            stateMachine = new StateMachine<BossAction>();
            stateMachine.Init();

            stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
            stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
            stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
        }

        private void OnBossKill(HealthBase h)
        {
            SwitchState(BossAction.DEATH);
        }

        #region ATTACK
        public void StartAttack(Action endCallback = null)
        {
            StartCoroutine(AttackCoroutine(endCallback));
        }

        IEnumerator AttackCoroutine(Action endCallback)
        {
            int attack = 0;
            while(attack < attackAmount)
            {
                attack++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            endCallback?.Invoke();
        }
        #endregion

        #region WALK
        public void GoToRandomPoint(Action onArrive = null)
        {
            waypointsRandom = UnityEngine.Random.Range(0, waypoints.Count);
            StartCoroutine(GoToPointCoroutine(waypoints[waypointsRandom], onArrive));
        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while(Vector3.Distance(transform.position, t.position) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            onArrive?.Invoke();
        }
        #endregion

        #region ANIMATION
        public void StartInitAnimation()
        {
            transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
        }
        #endregion


        #region DEBUG
        [NaughtyAttributes.Button]
        private void SwitchInit()
        {
            SwitchState(BossAction.INIT);
        }
        [NaughtyAttributes.Button]
        private void SwitchWalk()
        {
            SwitchState(BossAction.WALK);
        }
        [NaughtyAttributes.Button]
        private void SwitchAttack()
        {
            SwitchState(BossAction.ATTACK);
        }
        #endregion


        #region STATE MACHINE
        public void SwitchState(BossAction state)
        {
            stateMachine.SwitchState(state, this);
        }

        #endregion


        IEnumerator StartBoss()
        {
            bossGraphics.SetActive(true);
            SwitchState(BossAction.INIT);
            yield return new WaitForSeconds(1.5f);
            SwitchState(BossAction.WALK);
        }

        #region TRIGGER
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == tagToComparePlayer && !bossStarted)
            {
                StartCoroutine(StartBoss());
                bossStarted = true;
            }
            if(other.tag == tagToComparePlayer)
            {
                transform.LookAt(_player.transform.position);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == tagToComparePlayer)
            {
                transform.LookAt(waypoints[waypointsRandom].transform.position);
            }
        }
        #endregion

    }

}
