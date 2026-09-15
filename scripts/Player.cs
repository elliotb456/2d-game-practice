using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

    public Vector2 ScreenSize; // Size of the game window.


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector

		// Movement logic checks to determine the players velocity 
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1; 
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}


		// Getting the AnimatedSprite2D node and assigning it to a variable to make it easier to call 
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play(); // If velocity value greater than 0 (ie. moving) then play the animation
		}
		else
		{
			animatedSprite2D.Stop(); // else stop the animation
		}


		// Update the players position and lock it to the screen
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);


		// Ensuring the correct animations play, and are flipped as needed
		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up"; 
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}
}
