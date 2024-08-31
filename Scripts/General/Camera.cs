using Godot;
using System;

public partial class Camera : Camera3D
{
    [Export] private Node target;
    [Export] private Vector3 positionFromTargert;

    public override void _Ready()
    {
        GameEvents.OnStartGame += HandleStartGame;
        GameEvents.OnEndGame += HandleEndGame;
    }


    void HandleStartGame()
    {
        Reparent(target);

        Position = positionFromTargert;
    }

    private void HandleEndGame()
    {
        Reparent(GetTree().CurrentScene);
    }
}