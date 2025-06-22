using System;
using System.Linq;
using Godot;

public partial class Spawner : Node2D
{
    private static readonly PackedScene OBSTACLE = GD.Load<PackedScene>("scenes/obstacle.tscn");

    public Timer timer { get; private set; }

    [Signal] public delegate void OnObstacleCrashEventHandler();
    [Signal] public delegate void OnScoreAreaEnteredEventHandler();

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        CreateObstacle();
    }

    private void CreateObstacle()
    {
        var obstacle = OBSTACLE.Instantiate<Obstacle>();

        var viewPort = GetViewportRect();
        var halfY = viewPort.Size.Y / 2;
        var randY = GD.RandRange(halfY + 240, halfY - 50);

        obstacle.Position = new Vector2(viewPort.End.X + 150, (float)randY);
        obstacle.OnPlaneCrashed += OnPlaneCrashed;
        obstacle.OnPlayerScored += OnPlayerScored;

        AddChild(obstacle);
    }

    public void StopObstacles()
    {
        timer.Stop();
        foreach (Obstacle obstacle in GetChildren().OfType<Obstacle>())
        {
            timer.Stop();
            obstacle.SetSpeed(0);
        }
    }

    private void OnPlaneCrashed()
    {
        EmitSignal(SignalName.OnObstacleCrash);
        StopObstacles();
    }

    private void OnPlayerScored()
    {
        EmitSignal(SignalName.OnScoreAreaEntered);
    }
}
