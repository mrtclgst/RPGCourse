public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _player.GetDashDuration();
        SkillManager.Instance.GetSkillDash().CloneOnDash();
        _player.Stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetVelocity(0, _playerRb.velocity.y);
        SkillManager.Instance.GetSkillDash().CloneOnArrival();
        _player.Stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!_player.IsGroundDetected() && _player.IsWallDetected())
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }

        _player.SetVelocity(_player.GetDashSpeed() * _player.GetDashDirection(), 0);
        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }

        _player.EntityFX.CreateAfterImage();
    }

}