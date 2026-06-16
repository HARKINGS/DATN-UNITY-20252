
using UnityEngine;

public interface IHealth {
    void ChangeHealth(DamageData damageData);
}

public interface IMovable
{
    void Move(Vector2 moveDirection);
    void Stop();
}

public interface IAnalyzable
{
    void TrackAttack();
    void TrackDodge();
}

public interface IState
{
    void Enter();
    void Exit();
    void Tick();
}