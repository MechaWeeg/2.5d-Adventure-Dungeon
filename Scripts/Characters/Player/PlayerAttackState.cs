using Godot;
using System;

public partial class PlayerAttackState : PlayerState
{
    [Export] private Timer comboTimerNode;
    private int comboCounter = 1;
    private int maxComboCount = 2;

    public override void _Ready()
    {
        base._Ready();
        comboTimerNode.Timeout += () => comboCounter = 1;

    }
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK + comboCounter, -1, 1.8f);

        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
        comboTimerNode.Start();
    }



    protected override void ExitState()
    {
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
    }

    private void HandleAnimationFinished(StringName animName)
    {
        comboCounter++;
        comboCounter = Mathf.Wrap(comboCounter, 1, maxComboCount + 1);

        characterNode.ToggleHitbox(true);
        
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }

    private void PerformHit()
    {
        Vector3 newPosition = characterNode.Sprite3DNode.FlipH ?
            Vector3.Left :
            Vector3.Right;
        float distanceMultiplier = 0.75f;

            characterNode.HitboxNode.Position = newPosition * distanceMultiplier;
        
        characterNode.ToggleHitbox(false);

    }
}
