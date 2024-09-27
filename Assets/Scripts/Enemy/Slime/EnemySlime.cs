using UnityEngine;

public class EnemySlime : Enemy
{
    #region States
    public SlimeIdleState IdleState { get; private set; }
    public SlimeMoveState MoveState { get; private set; }
    public SlimeBattleState BattleState { get; private set; }
    public SlimeAttackState AttackState { get; private set; }
    public SlimeStunnedState StunnedState { get; private set; }
    public SlimeDeadState DeadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new SlimeIdleState(this, StateMachine, "Idle", this);
        MoveState = new SlimeMoveState(this, StateMachine, "Move", this);
        BattleState = new SlimeBattleState(this, StateMachine, "Move", this);
        AttackState = new SlimeAttackState(this, StateMachine, "Attack", this);
        StunnedState = new SlimeStunnedState(this, StateMachine, "Stunned", this);
        DeadState = new SlimeDeadState(this, StateMachine, "Idle", this);
    }
    protected override void Start()
    {
        base.Start();
        SetupDefaultDirection(-1);
        StateMachine.Initialize(IdleState);
    }

    internal override bool CanBeStunned()
    {
        return base.CanBeStunned();
    }


    protected override void Update()
    {
        Debug.Log(StateMachine.CurrentState.ToString());
        base.Update();
    }
    internal override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);
    }
}