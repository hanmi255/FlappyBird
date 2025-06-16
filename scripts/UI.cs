using System;
using Godot;

public partial class UI : CanvasLayer
{
    private static readonly Texture2D MEDAL_GOLD = GD.Load<Texture2D>("res://assets/sprites/UI/medalGold.png");
    private static readonly Texture2D MEDAL_SILVER = GD.Load<Texture2D>("res://assets/sprites/UI/medalSilver.png");
    private static readonly Texture2D MEDAL_BRONZE = GD.Load<Texture2D>("res://assets/sprites/UI/medalBronze.png");

    private Label ScoreLabel;
    public Control StartMenu { get; private set; }
    private VBoxContainer GameOverMenu;
    private TextureButton OKButton;
    private TextureRect MedalImg;
    private Label CurrentScore;
    private Label BestScore;
    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("%ScoreLabel");
        StartMenu = GetNode<Control>("%StartMenu");
        GameOverMenu = GetNode<VBoxContainer>("%GameOverMenu");
        OKButton = GetNode<TextureButton>("%OKButton");
        MedalImg = GetNode<TextureRect>("%MedalImg");
        CurrentScore = GetNode<Label>("%CurrentScore");
        BestScore = GetNode<Label>("%BestScore");

        OKButton.Pressed += () =>
        {
            GetTree().ReloadCurrentScene();
        };
    }

    public void UpdateScore(int score)
    {
        ScoreLabel.Text = score.ToString();
    }

    public void GameOver()
    {
        GameOverMenu.Show();
        ScoreLabel.Hide();
    }

    public void CalculateScore(int score, int bestScore)
    {
        CurrentScore.Text = score.ToString();

        MedalImg.Texture = score switch
        {
            >= 20 => MEDAL_GOLD,
            >= 10 => MEDAL_SILVER,
            _ => MEDAL_BRONZE
        };
        BestScore.Text = bestScore.ToString();
    }
}
