public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySlime enemySlime)
        : base(enemy, stateMachine, animBoolName, enemySlime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.GetIdleTimer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0f)
        {
            _stateMachine.ChangeState(_enemySlime.MoveState);
        }
    }
}
