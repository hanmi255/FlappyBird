using System;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Gravity { get; set; } = 1000.0f;
	[Export] public float JumpVelocity { get; set; } = -400.0f;
	[Export] public float MaxSpeed { get; set; } = 400.0f;
	[Export] public float RotationSpeed { get; set; } = 2.0f;
	[Signal] public delegate void OnPlayerGameStartEventHandler();

	private bool isStarted = false;
	private bool shouldProcessInput = true;
	private AudioStreamPlayer2D JumpSound;
	public override void _Ready()
	{
		JumpSound = GetNode<AudioStreamPlayer2D>("JumpSound");
	}


	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		if (Input.IsActionJustPressed("jump") && shouldProcessInput)
		{
			velocity.Y = JumpVelocity;
			Rotation = Mathf.DegToRad(-30f);
			JumpSound.Play();

			if (!isStarted)
			{
				isStarted = true;
				EmitSignal(SignalName.OnPlayerGameStart);
			}
		}

		if (!isStarted)
			return;

		velocity.Y += Gravity * (float)delta;

		Velocity = velocity;
		MoveAndSlide();
		RotatePlayer(delta);
	}

	private void RotatePlayer(double delta)
	{
		if (Velocity.Y > 0 && Rotation < Mathf.DegToRad(90f))
		{
			Rotation += RotationSpeed * (float)delta;
		}
	}

	public void StopMoveMent()
	{
		shouldProcessInput = false;
	}

	public void StopGravity()
	{
		Gravity = 0;
		Velocity = Vector2.Zero;
	}
}
