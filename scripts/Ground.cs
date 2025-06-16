using System;
using System.Linq;
using Godot;

public partial class Ground : Node2D
{
    [Export] public float Speed = -150.0f;
    [Signal] public delegate void OnPlayerCrashedEventHandler();

    private int Width { get; set; }
    private Sprite2D sprite2D;
    private Area2D[] grounds;
    private AudioStreamPlayer2D DieSound;

    public override void _Ready()
    {
        var ground1 = GetNode<Area2D>("Ground_1");
        var ground2 = GetNode<Area2D>("Ground_2");
        grounds = [ground1, ground2];

        foreach (var ground in grounds)
            ground.BodyEntered += OnGroundBodyEntered;

        sprite2D = GetNode<Sprite2D>("%Sprite2D");
        DieSound = GetNode<AudioStreamPlayer2D>("DieSound");

        Width = sprite2D.Texture.GetWidth();
    }

    public override void _Process(double delta)
    {
        // 移动逻辑
        Vector2 movement = new(Speed * (float)delta, 0);
        foreach (var ground in grounds)
            ground.GlobalPosition += movement;

        // 复位逻辑
        foreach (var ground in grounds)
        {
            if (ground.GlobalPosition.X < -Width / 2)
            {
                // 找到相邻地面并计算新位置
                Vector2 newPos = grounds.First(g => g != ground).GlobalPosition;
                ground.GlobalPosition = new Vector2(newPos.X + Width, ground.GlobalPosition.Y);
            }
        }
    }

    private void OnGroundBodyEntered(Node2D body)
    {
        EmitSignal(SignalName.OnPlayerCrashed);
        Speed = 0;
        DieSound.Play();

        var playerRef = body as Player;
        playerRef.StopMoveMent();
        playerRef.StopGravity();
    }
}
