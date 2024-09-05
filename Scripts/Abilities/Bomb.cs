using Godot;
using System;

public partial class Bomb : Node3D
{
    [Export] public float Damage { get; private set; } = 10;
    [Export] private AnimationPlayer playerNode;

    public override void _Ready()
    {
       playerNode.AnimationFinished += HandleExpandAnimationFinished;
    }

    private void HandleExpandAnimationFinished(StringName animName)
    {
        if (animName == GameConstants.ANIM_EXPAND)
        {
            playerNode.Play(GameConstants.ANIM_EXPLOSION);
        }
        else
        {
            QueueFree();
        }
    }

}
