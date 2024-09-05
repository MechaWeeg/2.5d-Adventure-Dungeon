using Godot;
using System;
using System.Linq;

public abstract partial class Character : CharacterBody3D
{

    [Export] private StatResource[] stats;

    [ExportGroup("Required Nodes")] 
    [Export] public AnimationPlayer AnimPlayerNode { get; private set; }
    [Export] public Sprite3D Sprite3DNode { get; private set; }
    [Export] public StateMachine StateMachineNode { get; private set; }
    [Export] public Area3D HurtboxNode { get; private set; }
    [Export] public Area3D HitboxNode { get; private set; }
    [Export] public CollisionShape3D HitboxShapeNode { get; private set; }
    [Export] public Timer ShaderTimer { get; private set; }

    [ExportGroup("AI Nodes")]
    [Export] public Path3D PathNode { get; private set; }
    [Export] public NavigationAgent3D AgentNode { get; private set; }
    [Export] public Area3D ChaseAreaNode { get; private set; }
    [Export] public Area3D AttackAreaNode { get; private set; }


    public Vector2 direction = new();
    public ShaderMaterial shader;

    public override void _Ready()
    {
        shader = (ShaderMaterial)Sprite3DNode.MaterialOverlay;

        HurtboxNode.AreaEntered += HandleHurtboxAreaEntered;
        Sprite3DNode.TextureChanged += HandleTextureChanged;
        ShaderTimer.Timeout += HandleShaderTimerTimeout;

    }

    private void HandleTextureChanged()
    {
        shader.SetShaderParameter(
            "tex", Sprite3DNode.Texture
        );
    }

    private void HandleHurtboxAreaEntered(Area3D area)
    {
        if (area is not IHitbox hitbox) { return; }

        StatResource health = GetStatResource(Stat.Health);
        float damage = hitbox.GetDamage();

        health.StatValue -= damage;

        shader.SetShaderParameter(
            "active", true
        );

        ShaderTimer.Start();
    }

    public StatResource GetStatResource(Stat stat) 
    {
        return stats.Where((element) => element.StatType == stat)
        .FirstOrDefault();
    }

    public void Flip()
    {
        bool isNotMovingHorizontally = Velocity.X == 0;

        if (isNotMovingHorizontally) { return; }

        bool isMovingLeft = Velocity.X < 0;
        Sprite3DNode.FlipH = isMovingLeft;
    }

    public void ToggleHitbox(bool flag)
    {
        HitboxShapeNode.Disabled = flag;
    }

    private void HandleShaderTimerTimeout()
    {
        shader.SetShaderParameter(
            "active", false
        );
    }
}
