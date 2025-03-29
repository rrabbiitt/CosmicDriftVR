using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyAI : MonoBehaviour, ITakeDamage
{
    // 애니메이션 트리거 이름 상수화
    const string RUN_TRIGGER = "Run";
    const string CROUCH_TRIGGER = "Crouch";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float startingHealth; // 시작 체력
    [SerializeField] private float minTimeUnderCover; // 숨어있는 최소 시간
    [SerializeField] private float maxTimeUnderCover; // 숨어있는 최대 시간
    [SerializeField] private int minShootsToTake; // 최소 발사 횟수
    [SerializeField] private int maxShootsToTake; // 최대 발사 횟수
    [SerializeField] private int rotationSpeed; // 회전 속도
    [SerializeField] private int damage; // 공격 데미지
    [Range(0, 100)]
    [SerializeField] private float shootingAccuracy; // 발사 정확도

    [SerializeField] private Transform shootingPosition; // 발사 위치
    [SerializeField] private ParticleSystem bloodSplatterFX; // 피 특수 효과

    private bool isShooting; // 발사중 여부
    private int currentShotsTaken; // 현재 발사 횟수
    private int currentMaxShotsToTake; // 현재 최대 발사 횟수
    private UnityEngine.AI.NavMeshAgent agent; // NavMesh 에이전트 컴포넌트
    private Player player; // 플레이어 객체
    private Transform occupiedCoverSpot; // 사용중인 숨어있는 장소
    private Animator animator; // 애니메이터 컴포넌트
   
    private float _health; // 적 체력 저장하는 private 변수

    public float health // 적 체력
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, startingHealth); //체력을 시작 체력과 0 사이로 제한
        }
    }

    public void Awake()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 참조
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // NavMesh
        animator.SetTrigger(RUN_TRIGGER); // 시작 시 달리기 애니메이션 재생
        _health = startingHealth; // 시작 체력으로 초기화
    }

    public void Init(Player player, Transform coverSpot)
    {
        occupiedCoverSpot = coverSpot; // 숨을 위치 설정
        this.player = player; // 플레이어 설정
        GetToCover(); // 숨을 위치로 이동
    }

    // 숨을 위치로 이동
    private void GetToCover()
    {
        agent.isStopped = false; // NavMesh 이동 활성화
        agent.SetDestination(occupiedCoverSpot.position); // 숨을 위치로 이동 목표 설정
    }

    private void Update()
    {
        // 움직이고 있고 적이 숨을 위치에 도달했을 경우
        if(agent.isStopped == false && (transform.position - occupiedCoverSpot.position).sqrMagnitude <= 0.1f)
        {
            agent.isStopped = true; // 이동 비활성화
            StartCoroutine(InitializeShootingCO()); // 발사 코루틴 실행
        }
        if (isShooting) // 적이 발사중이면
        {
            RotateTowardsPlayer(); // 플레이어 쪽으로 회전
        }
    }

    // 발사 동작을 초기화하는 코루틴 메서드
    private IEnumerator InitializeShootingCO()
    {
        HideBehindCover(); // 숨기 함수
        // 랜덤한 시간 동안 기다림
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeUnderCover, maxTimeUnderCover));
        StartShooting(); // 발사 동작 시작
    }

    private void HideBehindCover()
    {
        animator.SetTrigger(CROUCH_TRIGGER); // 숙이기 애니메이션 재생
    }

    // 발사 동작 시작 함수
    private void StartShooting()
    {
        isShooting = true; // 발사 중 상태로 변경
        // 최대 발사 횟수 설정
        currentMaxShotsToTake = UnityEngine.Random.Range(minShootsToTake, maxShootsToTake);
        currentShotsTaken = 0; // 현재 발사 횟수 초기화
        animator.SetTrigger(SHOOT_TRIGGER); // 쏘기 애니메이션 재생
    }

    // 실제 발사 함수, 플레이어를 맞췄는지 확인
    public void Shoot()
    {
        /*
        // 랜덤 값을 생성해 발사 정확도 판단
        bool hitPlayer = UnityEngine.Random.Range(0, 100) < shootingAccuracy;

        // 만약 발사가 정확하게 향하면
        if (hitPlayer)
        {
            RaycastHit hit;
            // 적이 플레이어 헤드 방향으로 레이캐스트 발사
            Vector3 direction = player.GetHeadPosition() - shootingPosition.position;
            // 레이캐스트가 플레이어에게 맞았다면
            if (Physics.Raycast(shootingPosition.position, direction, out hit))
            {
                // 레이캐스트가 맞은 오브젝트가 player 컴포넌트를 가진 상위 오브젝트면
                Player player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    // 플레이어에게 데메지를 입힘
                    player.TakeDamage(damage);
                }
            }
        } */
        currentShotsTaken++; // 현재 발사 횟수 증가
        // 최대 발사 횟수에 도달하지 않았으면
        if(currentShotsTaken >-currentMaxShotsToTake)
        {
            StartCoroutine(InitializeShootingCO()); // 다시 발사 준비 코루틴 시작
        }
    }

    // 플레이어 방향으로 회전하는 함수
    private void RotateTowardsPlayer()
    {
        // 플레이어와의 방향 벡터 계산
        Vector3 direction = player.GetHeadPosition() - transform.position;
        // 플레이어의 높이 방향(Y축)은 회전에 영향을 주지 않도록 설정
        direction.y = 0;
        // 적 캐릭터가 플레이어를 바라보는 회전을 계산
        Quaternion rotation = Quaternion.LookRotation(direction);
        // 회전 각도를 rotationSpeed 속도로 회전해 부드럽게 회전하도록 함
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation; // 실제 회전 적용
    }

    public delegate void EnemyDestroyedEventHandler(EnemyAI enemy);
    public event EnemyDestroyedEventHandler OnDestroyEvent;

    public void TakeDamage(Weapon weapon, Projectile projectile, Vector3 contactPoint)
    {
        health -= weapon.GetDamage();
        if (health <= 0)
        {
            onDestroy();
        }
        ParticleSystem effect = Instantiate(bloodSplatterFX, contactPoint, Quaternion.LookRotation(weapon.transform.position - contactPoint));
        effect.Stop();
        effect.Play();
    }

    public void onDestroy()
    {
        if (OnDestroyEvent != null)
        {
            OnDestroyEvent.Invoke(this);
        }
        Destroy(gameObject); // EnemyAI 스크립트가 붙은 GameObject 파괴
    }
}
