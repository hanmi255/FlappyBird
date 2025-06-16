using System;
using Godot;

public partial class Obstacle : Node2D
{
    [Export] public float MoveSpeed = 150.0f;
    [Signal] public delegate void OnPlaneCrashedEventHandler();
    [Signal] public delegate void OnPlayerScoredEventHandler();

    private AudioStreamPlayer2D HitSound;
    private AudioStreamPlayer2D ScoreSound;

    public override void _Ready()
    {
        var topArea = GetNode<Area2D>("TopArea");
        var bottomArea = GetNode<Area2D>("BottomArea");
        var score = GetNode<Area2D>("Score");
        var visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        HitSound = GetNode<AudioStreamPlayer2D>("HitSound");
        ScoreSound = GetNode<AudioStreamPlayer2D>("ScoreSound");

        topArea.BodyEntered += OnMountainBodyEntered;
        bottomArea.BodyEntered += OnMountainBodyEntered;
        score.BodyEntered += OnScoreBodyEntered;
        visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
    }

    public override void _Process(double delta)
    {
        Position = new Vector2(Position.X - MoveSpeed * (float)delta, Position.Y);
    }

    public void SetSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    private void OnMountainBodyEntered(object sender)
    {
        if (sender is Player)
        {
            EmitSignal(SignalName.OnPlaneCrashed);
            HitSound.Play();
        }
    }

    private void OnScoreBodyEntered(object sender)
    {
        if (sender is Player)
        {
            EmitSignal(SignalName.OnPlayerScored);
            ScoreSound.Play();
        }
    }

    private void OnScreenExited()
    {
        QueueFree();
    }
}
