public class PlayerCounterAttackState : PlayerState
{
    private bool _canCreateClone;
    public PlayerCounterAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _stateTimer = _player.GetCounterAttackDuration();
        _player.Animator.SetBool("SuccessfulCounterAttack", false);
        _canCreateClone = true;
    }
    public override void Update()
    {
        base.Update();

        _player.SetVelocityZero();

        if (_player.TryToCounterAttack())
        {
            _stateTimer = 10;
            _player.Animator.SetBool("SuccessfulCounterAttack", true);
        }

        if (_stateTimer < 0 || _triggerCalled)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    internal bool GetCanCreateClone()
    {
        return _canCreateClone;
    }
    internal void SetCanCreateClone(bool canCreateClone)
    {
        _canCreateClone = canCreateClone;
    }
}