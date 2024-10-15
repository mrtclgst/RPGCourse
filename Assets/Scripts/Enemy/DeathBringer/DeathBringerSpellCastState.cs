using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;

    private int _amountOfSpells;
    private float _spellTimer;



    public DeathBringerSpellCastState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer) : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }
    public override void Enter()
    {
        base.Enter();
        _amountOfSpells = _enemyDeathBringer.GetAmountOfSpells();
        _spellTimer = 0.5f;
    }

    public override void Update()
    {
        base.Update();

        _spellTimer -= Time.deltaTime;

        if (CanCastSpell())
        {
            _enemyDeathBringer.CastSpell();
            _amountOfSpells--;
        }
        else
        {
            if (_amountOfSpells <= 0)
                _stateMachine.ChangeState(_enemyDeathBringer.TeleportState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _enemyDeathBringer.SetLastTimeCast(Time.time);
    }
    private bool CanCastSpell()
    {
        if (_amountOfSpells > 0 && _spellTimer < 0)
        {
            _spellTimer = _enemyDeathBringer.GetSpellCooldown();
            return true;
        }

        return false;
    }
}