using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitBase;
using static SpellBase;
using Spine.Unity;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SocialPlatforms;

public class PlayerUnit : UnitBase
{
    [Header("# Unit Effect")]
    public List<GameObject> buffEffect = new List<GameObject>();

    [Header("# Unit Setting")]
    public Scanner scanner;
    public UnitData unitData;
    MeshRenderer bodySprite;
    protected bool startMoveFinish = false;
    protected LayerMask targetLayer;
    protected Vector3 moveVec; // 거리
    [SerializeField] protected Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    [SerializeField] protected Vector2 attackRaySize;
    GameObject hpBar; // 체력바

    [Header("# Unit Activity")]
    Collider2D col;
    protected RaycastHit2D[] attackTargets; // 스캔 결과 배열
    [SerializeField] protected Transform nearestAttackTarget; // 가장 가까운 목표
    [SerializeField] protected Transform[] multipleAttackTargets; // 다수 공격 목표
    [SerializeField] Vector3 firstPos;
    protected Coroutine smash;
    protected Coroutine arrow;
    protected Coroutine lerp;

    [Header("# Spine")]
    SkeletonAnimation skeletonAnimation;
    string CurrentAnimation; //현재 어떤 애니메이션이 재생되고 있는지에 대한 변수

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        bodySprite = GetComponent<MeshRenderer>();

        targetLayer = scanner.targetLayer;
    }

    void OnEnable()
    {
        StateSetting();
        MakeHpBar();

        BattleData.Instance.units.Add(gameObject);

        Vector3 startPos = BattleManager.Instance.unitSpawnPoint[0].position;
        Vector3 targetPos = BattleManager.Instance.point;
        float xPos = startPos.x + targetPos.x * (targetPos.x < 0 ? -0.4f : 0.4f);

        // 클릭 지점으로 이동 -> 나머지는 Scanner 함수 에서 실행 (y 축만 먼저 빠르게 이동)
        lerp = StartCoroutine(lerpCoroutine(startPos,new Vector3((xPos > targetPos.x ? targetPos.x : xPos), targetPos.y, 0), moveSpeed)); // y 축 먼저 이동
    }

    private void Start()
    {
        // 초기 데이터 저장
        initialHealth = unitData.Health;
        initialMoveSpeed = unitData.MoveSpeed;
        initialPower = unitData.Power;
        initialAttackTime = unitData.AttackTime;
        

    }

    void Update()
    {
        if (unitState != UnitState.Die)
        {
            // 체력 실시간 적용
            HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
            hpBarLogic.nowHp = health;

            if (health <= 0 || BattleManager.Instance.battleState == BattleManager.BattleState.Lose) // hp 가 0 이 되거나 게임에서 졌을 경우
            {
                StartCoroutine(Die());
            } 
            else if(BattleManager.Instance.battleState == BattleManager.BattleState.Win) // 승리 시
            {
                Win();
            }
            else
            {
                AttackRay();
            }
        }

        // Order Layer 조정
        // SpriteRenderer 가 있을 경우에는 본체의 y 축 값의 소수점을 제외한 값을 Order Layer 에 적용
        if (bodySprite != null)
        {
            float yPos = transform.position.y * 100 - 400; // 넓게 분배하기 위해 * 100 음수/양수 처리를 위해 -400;
            int orderLayer = Mathf.FloorToInt(yPos); // 소수점 제외
            bodySprite.sortingOrder = Mathf.Abs(orderLayer); // 절대값으로 변경 후 적용

            // 체력바 OrderLayer
            HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
            hpBarLogic.realHpSprite.sortingOrder = Mathf.Abs(orderLayer) - 1;
            hpBarLogic.hpFrameSprite.sortingOrder = Mathf.Abs(orderLayer);
        }
    }

    void OnDisable()
    {
        transform.position = new Vector3(-10, 0, 0); // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
    }

    // 기본 설정 초기화 함수
    void StateSetting()
    {
        // 수치값
        unitID = unitData.UnitID;
        health = unitData.Health;
        moveSpeed = unitData.MoveSpeed;
        power = unitData.Power;
        attackTime = unitData.AttackTime;

        if (PlayerData.instance != null)//진열대 추가 수치값
        {
            Debug.Log("변경전 : " + power);
            float atk = PlayerData.instance.atk_Stu;
            float spd = PlayerData.instance.speed_Stu;
            float hp = PlayerData.instance.unitHp_Stu;
            if (atk >= 1) { power *= atk; }
            if (spd >= 1) { moveSpeed *= spd; }
            if (hp >= 1) { health *= hp; }
            Debug.Log("변경후 : " + power);
        }

        // 설정값
        col.enabled = true;
        unitState = UnitState.Move;
        moveVec = Vector3.right;
        firstPos = BattleManager.Instance.point;
        scanner.unitType = unitID / 10000;
        nearestAttackTarget = null;
        multipleAttackTargets = new Transform[5];
    }

    // 체력바
    void MakeHpBar()
    {
        // 체력바
        hpBar = PoolManager.Instance.Get(1, 3);
        HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
        hpBarLogic.owner = this.gameObject.transform; // 주인 설정
        hpBarLogic.nowHp = health;
        hpBarLogic.maxHp = health;
    }

    // 가까운 적을 찾는 Scanner 함수 (이동)
    protected virtual void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이(방향) = 타겟 위치 - 나의 위치
            moveVec = scanner.nearestTarget.position - transform.position;
            if(transform.position.y != moveVec.y) { moveVec.x *= 0.5f; moveVec.y *= 2f; } // y 축 먼저 빠르게 이동
            else { moveVec.x *= 1f; moveVec.y *= 1f; } // 정상화

            // 이동
            transform.position += moveVec.normalized * moveSpeed * Time.deltaTime;
            unitState = UnitState.Move;

            // 애니메이션
            StartAnimation("Walk", true, 1f);

            // 가는 방향에 따라 Sprite 방향 변경
            SpriteDir(moveVec, Vector3.zero);
        }
        else
        {
            if (startMoveFinish)
            {
                // 유닛의 처음 위치로 귀환
                lerp = StartCoroutine(lerpCoroutine(transform.position, firstPos, moveSpeed));

                if (transform.position == firstPos) {

                    moveVec = Vector3.zero;
                    unitState = UnitState.Idle;
                    transform.localScale = new Vector3(1f, 1f, 1f);

                    // 애니메이션
                    StartAnimation("Idle", true, 1.5f);
                }
            }
        }
    }

    // 가까운 공격 목표를 찾는 Ray 함수 (공격)
    protected virtual void AttackRay()
    {
        // BoxCastAll(시작 위치, 크기, 회전, 방향, 길이, 대상 레이어) : 사각형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, targetLayer);

        nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // 단일 공격
        multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // 다수 공격

        if (nearestAttackTarget != null)
        {
            if(!startMoveFinish)
            {
                StopCoroutine(lerp);
                startMoveFinish = true;
            }

            // 적이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            // 적 상태 변경
            if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
            {
                if (multipleAttackTargets == null) 
                    return;

                foreach (Transform enemy in multipleAttackTargets)
                {
                    if (enemy == null) return;
                    UnitBase enemyState = enemy.gameObject.GetComponent<UnitBase>();
                    enemyState.unitState = UnitState.Fight;
                }
            }
            else
            {
                if (nearestAttackTarget == null) return;
                UnitBase enemyState = nearestAttackTarget.gameObject.GetComponent<UnitBase>();
                enemyState.unitState = UnitState.Fight;
            }         

            // 공격
            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;

                // 유닛 별로 각각의 공격 함수 실행
                if (gameObject.CompareTag("Archer"))
                {
                    arrow = StartCoroutine(Arrow());
                }
                else
                {
                    smash = StartCoroutine(Attack());
                }
            }

            // 적의 위치에 따라 Sprite 방향 변경 (Attary Ray 영역이 큰 Unit 변수 제거 용도)
            SpriteDir(nearestAttackTarget.transform.position, transform.position);
        }
        else
        {
            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();

            // 다음에 attackRay 에 적 인식시, 바로 공격 가능하게 attackTime 초기화
            attackTime = unitData.AttackTime - 0.2f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize);
    }

    // 일반 근접 공격 함수
    protected virtual IEnumerator Attack()
    {
        if (nearestAttackTarget == null) {
            if (smash != null) { StopCoroutine(smash); smash = null; }
        }

        // 애니메이션
        StartAnimation("Attack", false, 1f);

        yield return new WaitForSeconds(0.6f); // 애니메이션 시간

        if ((unitID % 10000) / 1000 == 2) // 탱커, Croirang -> 다수 공격
        {
            foreach (Transform enemy in multipleAttackTargets)
            {
                SetEnemyState(enemy);
            }
        }
        else // 단일 공격
        {
            SetEnemyState(nearestAttackTarget);
        }

        yield return new WaitForSeconds(0.4f); // 애니메이션 시간

        // 애니메이션
        StartAnimation("Idle", true, 1.5f);
    }

    protected void SetEnemyState(Transform target)
    {
        if(target == null)
            return;

        EnemyUnit enemyLogic = target.gameObject.GetComponent<EnemyUnit>();

        enemyLogic.health -= power;
    }

    // 화살 공격 함수
    IEnumerator Arrow()
    {
        if (nearestAttackTarget == null) {
            if (arrow != null) { StopCoroutine(arrow); arrow = null; }
        }

        // 애니메이션
        StartAnimation("Attack", false, 1f);

        yield return new WaitForSeconds(0.6f); // 애니메이션 시간

        // 맞고 있는 적 유닛 상태 변경
        if (nearestAttackTarget != null)
        {
            EnemyUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<EnemyUnit>();
        }

        // 화살 가져오기
        GameObject arrowObj = PoolManager.Instance.Get(3, 1, transform.position + new Vector3(0, 1f, 0));
        Arrow arrowLogic = arrowObj.GetComponent<Arrow>();
        arrowLogic.unitType = unitID / 10000;
        arrowLogic.arrowPower = power;

        // 화살 목표 오브젝트 설정
        if(nearestAttackTarget != null)
        {
            arrowLogic.target = nearestAttackTarget.gameObject;
            arrowLogic.playerUnit = this.gameObject;
        }

        yield return new WaitForSeconds(0.4f); // 애니메이션 시간

        // 애니메이션
        StartAnimation("Idle", true, 1.5f);
    }

    void Win()
    {
        unitState = UnitState.Win;
        startMoveFinish = true;
        nearestAttackTarget = null;
        scanner.nearestTarget = null;

        firstPos = transform.position;
        moveVec = Vector3.zero;
        moveSpeed = 0;
        StopCoroutine(lerp); // 이동 멈춤
        lerp = StartCoroutine(lerpCoroutine(transform.position, firstPos, moveSpeed));
        StopCoroutine(lerp); // 이동 멈춤
        StartAnimation("Win", true, 1);
    }

    IEnumerator Die()
    {
        unitState = UnitState.Die;
        moveVec = Vector2.zero;
        col.enabled = false;
        hpBar.SetActive(false);

        if (nearestAttackTarget != null)
        {
            EnemyUnit enemyLogic = nearestAttackTarget.GetComponent<EnemyUnit>();
            enemyLogic.unitState = UnitState.Move;

            nearestAttackTarget = null;
        }

        moveSpeed = 0;
        attackTime = 0;

        BattleData.Instance.units.Remove(gameObject);

        // 진행중인 코루틴 함수 모두 중지
        if (smash != null) { StopCoroutine(smash); smash = null; }
        if (arrow != null) { StopCoroutine(arrow); arrow = null; }

        // 애니메이션
        StartAnimation("Die", true, 1f);

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    // 맨 처음 시작 이동 lerpCoroutine
    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {

        float distance = Vector3.Distance(current, target); // 거리(길이) 구하기
        float time = distance / speed; // 거리(길이) 에 따라 이동하는 시간 설정

        float elapsedTime = 0.0f;

        this.transform.position = current;

        while (elapsedTime < time && !scanner.nearestTarget && current != target && BattleManager.Instance.battleState != BattleManager.BattleState.Win)
        {
            elapsedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(current, target, elapsedTime / time);

            yield return null;

            unitState = UnitState.Move;
            Debug.Log(gameObject.name + "  Move");

            // 가는 방향에 따라 Sprite 방향 변경
            SpriteDir(target, current);

            // 애니메이션
            StartAnimation("Walk", true, 1.2f);
        }

        startMoveFinish = true;

        yield return null;
    }

    // 스파인 애니메이션 함수
    protected void StartAnimation(string animName, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if (animName.Equals(CurrentAnimation))
        {
            return;
        }

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animName, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animName;
    }
    /*  이펙트 프리펩
    22001 - B_Hp_Spell hp최대치 30초간 증가
    22002 - B_Pw_Spell 파워 증가 20초
    22004 - B_Hp_HSpell 하이hp최대치 증가 30초간
    22005 - B_Mix_Spell 파워, 이속, 공속 30초간 증가
    22006 - B_Mix_HSpell 하이 파워,이속,공속 10초간 증가
    22007 - B_HS_HSpell 하이스피드 이동속도 초 증가 10초간
*/
    public void Buff_Effect(SpellTypes spellType, bool isBuff, int id)
    {
        switch (spellType)
        {
            case SpellTypes.Attack:
                buffEffect[0].SetActive(isBuff); // isBuff가 true면 활성화, false면 비활성화
                break;
            case SpellTypes.Buff:
                if(id == 22001)
                    {
                    buffEffect[0].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[0].name);
                }
                    else if (id == 22002)
                {
                    buffEffect[1].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[1].name);
                }
                else if (id == 22004)
                {
                    buffEffect[2].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[2].name);
                }
                else if (id == 22005)
                {
                    buffEffect[3].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[3].name);
                }
                else if (id == 22006)
                {
                    buffEffect[4].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[4].name);
                }
                else if (id == 22007)
                {
                    buffEffect[5].SetActive(isBuff);  // 버프 적용
                    Debug.Log(buffEffect[5].name);
                }
                
                break;
            case SpellTypes.Debuff:
                buffEffect[2].SetActive(isBuff); // isBuff가 true면 활성화, false면 비활성화
                break;
        }
    }


}
