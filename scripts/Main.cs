using System;
using Godot;

public partial class Main : Node2D
{
    const string SAVE_FILE = "user://score.data";

    private Player player;
    private Spawner spawner;
    private Ground ground;
    private UI ui;
    private int score = 0;
    private int bestScore = 0;

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        spawner = GetNode<Spawner>("Spawner");
        ground = GetNode<Ground>("Ground");
        ui = GetNode<UI>("UI");

        LoadBestScore();

        player.OnPlayerGameStart += () =>
        {
            spawner.timer.Start();
            ui.StartMenu.Hide();
        };

        spawner.OnObstacleCrash += () =>
        {
            player.StopMoveMent();
        };

        ground.OnPlayerCrashed += () =>
        {
            spawner.StopObstacles();
            LoadBestScore();
            ui.GameOver();
            ui.CalculateScore(score, bestScore);
        };

        spawner.OnScoreAreaEntered += () =>
        {
            score += 1;
            ui.UpdateScore(score);
            SaveScore();
        };
    }

    private void LoadBestScore()
    {
        var file = FileAccess.Open(SAVE_FILE, FileAccess.ModeFlags.Read);
        if (file != null && file.IsOpen())
        {
            if (file.GetLength() > 0)
                bestScore = (int)file.Get32();
            file.Close();
        }
    }

    private void SaveScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            var file = FileAccess.Open(SAVE_FILE, FileAccess.ModeFlags.Write);
            if (file != null && file.IsOpen())
            {
                file.Store32((uint)bestScore);
                file.Close();
            }
        }
    }
}
