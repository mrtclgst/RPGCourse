using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform _sword;

    public PlayerCatchSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _sword = _player.GetSword().transform;
        _player.PlayerFX.PlayDustFX();
        _player.PlayerFX.ScreenShake(_player.PlayerFX._shakeCatchSword);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (_sword.position.x > _player.transform.position.x && _player.GetFacingDirection() != 1
            || _sword.position.x < _player.transform.position.x && _player.GetFacingDirection() != -1)
        {
            _player.Flip();
        }
        _playerRb.velocity = new Vector2(-1 * _player.GetSwordReturnForce() * _player.GetFacingDirection(), _playerRb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        _player.StartCoroutine(_player.IE_BusyFor(0.1f));
    }

    public override void Update()
    {
        base.Update();
        if (_triggerCalled)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}