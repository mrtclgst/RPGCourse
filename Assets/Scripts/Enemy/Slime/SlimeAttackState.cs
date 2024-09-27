public class SlimeAttackState : EnemyState
{
    protected EnemySlime _enemySlime;

    public SlimeAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySlime enemySlime) : base(enemy, stateMachine, animBoolName)
    {
        _enemySlime = enemySlime;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _enemySlime.SetLastTimeAttacked();
    }

    public override void Update()
    {
        base.Update();
        _enemySlime.SetVelocityZero();

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(_enemySlime.BattleState);
        }
    }
}