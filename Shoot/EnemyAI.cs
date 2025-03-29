using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyAI : MonoBehaviour, ITakeDamage
{
    // �ִϸ��̼� Ʈ���� �̸� ���ȭ
    const string RUN_TRIGGER = "Run";
    const string CROUCH_TRIGGER = "Crouch";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float startingHealth; // ���� ü��
    [SerializeField] private float minTimeUnderCover; // �����ִ� �ּ� �ð�
    [SerializeField] private float maxTimeUnderCover; // �����ִ� �ִ� �ð�
    [SerializeField] private int minShootsToTake; // �ּ� �߻� Ƚ��
    [SerializeField] private int maxShootsToTake; // �ִ� �߻� Ƚ��
    [SerializeField] private int rotationSpeed; // ȸ�� �ӵ�
    [SerializeField] private int damage; // ���� ������
    [Range(0, 100)]
    [SerializeField] private float shootingAccuracy; // �߻� ��Ȯ��

    [SerializeField] private Transform shootingPosition; // �߻� ��ġ
    [SerializeField] private ParticleSystem bloodSplatterFX; // �� Ư�� ȿ��

    private bool isShooting; // �߻��� ����
    private int currentShotsTaken; // ���� �߻� Ƚ��
    private int currentMaxShotsToTake; // ���� �ִ� �߻� Ƚ��
    private UnityEngine.AI.NavMeshAgent agent; // NavMesh ������Ʈ ������Ʈ
    private Player player; // �÷��̾� ��ü
    private Transform occupiedCoverSpot; // ������� �����ִ� ���
    private Animator animator; // �ִϸ����� ������Ʈ
   
    private float _health; // �� ü�� �����ϴ� private ����

    public float health // �� ü��
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, startingHealth); //ü���� ���� ü�°� 0 ���̷� ����
        }
    }

    public void Awake()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ����
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // NavMesh
        animator.SetTrigger(RUN_TRIGGER); // ���� �� �޸��� �ִϸ��̼� ���
        _health = startingHealth; // ���� ü������ �ʱ�ȭ
    }

    public void Init(Player player, Transform coverSpot)
    {
        occupiedCoverSpot = coverSpot; // ���� ��ġ ����
        this.player = player; // �÷��̾� ����
        GetToCover(); // ���� ��ġ�� �̵�
    }

    // ���� ��ġ�� �̵�
    private void GetToCover()
    {
        agent.isStopped = false; // NavMesh �̵� Ȱ��ȭ
        agent.SetDestination(occupiedCoverSpot.position); // ���� ��ġ�� �̵� ��ǥ ����
    }

    private void Update()
    {
        // �����̰� �ְ� ���� ���� ��ġ�� �������� ���
        if(agent.isStopped == false && (transform.position - occupiedCoverSpot.position).sqrMagnitude <= 0.1f)
        {
            agent.isStopped = true; // �̵� ��Ȱ��ȭ
            StartCoroutine(InitializeShootingCO()); // �߻� �ڷ�ƾ ����
        }
        if (isShooting) // ���� �߻����̸�
        {
            RotateTowardsPlayer(); // �÷��̾� ������ ȸ��
        }
    }

    // �߻� ������ �ʱ�ȭ�ϴ� �ڷ�ƾ �޼���
    private IEnumerator InitializeShootingCO()
    {
        HideBehindCover(); // ���� �Լ�
        // ������ �ð� ���� ��ٸ�
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeUnderCover, maxTimeUnderCover));
        StartShooting(); // �߻� ���� ����
    }

    private void HideBehindCover()
    {
        animator.SetTrigger(CROUCH_TRIGGER); // ���̱� �ִϸ��̼� ���
    }

    // �߻� ���� ���� �Լ�
    private void StartShooting()
    {
        isShooting = true; // �߻� �� ���·� ����
        // �ִ� �߻� Ƚ�� ����
        currentMaxShotsToTake = UnityEngine.Random.Range(minShootsToTake, maxShootsToTake);
        currentShotsTaken = 0; // ���� �߻� Ƚ�� �ʱ�ȭ
        animator.SetTrigger(SHOOT_TRIGGER); // ��� �ִϸ��̼� ���
    }

    // ���� �߻� �Լ�, �÷��̾ ������� Ȯ��
    public void Shoot()
    {
        /*
        // ���� ���� ������ �߻� ��Ȯ�� �Ǵ�
        bool hitPlayer = UnityEngine.Random.Range(0, 100) < shootingAccuracy;

        // ���� �߻簡 ��Ȯ�ϰ� ���ϸ�
        if (hitPlayer)
        {
            RaycastHit hit;
            // ���� �÷��̾� ��� �������� ����ĳ��Ʈ �߻�
            Vector3 direction = player.GetHeadPosition() - shootingPosition.position;
            // ����ĳ��Ʈ�� �÷��̾�� �¾Ҵٸ�
            if (Physics.Raycast(shootingPosition.position, direction, out hit))
            {
                // ����ĳ��Ʈ�� ���� ������Ʈ�� player ������Ʈ�� ���� ���� ������Ʈ��
                Player player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    // �÷��̾�� �������� ����
                    player.TakeDamage(damage);
                }
            }
        } */
        currentShotsTaken++; // ���� �߻� Ƚ�� ����
        // �ִ� �߻� Ƚ���� �������� �ʾ�����
        if(currentShotsTaken >-currentMaxShotsToTake)
        {
            StartCoroutine(InitializeShootingCO()); // �ٽ� �߻� �غ� �ڷ�ƾ ����
        }
    }

    // �÷��̾� �������� ȸ���ϴ� �Լ�
    private void RotateTowardsPlayer()
    {
        // �÷��̾���� ���� ���� ���
        Vector3 direction = player.GetHeadPosition() - transform.position;
        // �÷��̾��� ���� ����(Y��)�� ȸ���� ������ ���� �ʵ��� ����
        direction.y = 0;
        // �� ĳ���Ͱ� �÷��̾ �ٶ󺸴� ȸ���� ���
        Quaternion rotation = Quaternion.LookRotation(direction);
        // ȸ�� ������ rotationSpeed �ӵ��� ȸ���� �ε巴�� ȸ���ϵ��� ��
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation; // ���� ȸ�� ����
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
        Destroy(gameObject); // EnemyAI ��ũ��Ʈ�� ���� GameObject �ı�
    }
}
