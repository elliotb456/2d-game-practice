using Godot;
using System;

public partial class Main : Node2D
{

	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
	private AudioStreamPlayer2D _music;
	private AudioStreamPlayer2D _deathSound;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_music = GetNode<AudioStreamPlayer2D>("Music");
		_deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
	}


	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		_music.Stop();
		_deathSound.Play();

		GetNode<Hud>("HUD").ShowGameOver();
	
	}


	public void NewGame()
	{
		_score = 0;
		_music.Play();

		var player = GetNode<Player>("Player");
		var startPostition = GetNode<Marker2D>("StartPosition");
		player.Start(startPostition.Position);

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		// Note that for calling Godot-provided methods with strings, we have to use the original Godot snake_case name.
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

		GetNode<Timer>("StartTimer").Start();
	}


	private void OnScoreTimerTimeout()
	{
    _score++;
	GetNode<Hud>("HUD").UpdateScore(_score);
	}


	private void OnStartTimerTimeout()
	{
    	GetNode<Timer>("MobTimer").Start();
    	GetNode<Timer>("ScoreTimer").Start();
	}


	private void OnMobTimerTimeout()
	{
    // Create a new instance of the Mob scene.
    Mob mob = MobScene.Instantiate<Mob>();

    // Choose a random location on Path2D.
    var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    mobSpawnLocation.ProgressRatio = GD.Randf();

    // Set the mob's direction perpendicular to the path direction.
    float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

    // Set the mob's position to a random location.
    mob.Position = mobSpawnLocation.Position;

    // Add some randomness to the direction.
    direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
    mob.Rotation = direction;

    // Choose the velocity.
    var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
    mob.LinearVelocity = velocity.Rotated(direction);

    // Spawn the mob by adding it to the Main scene.
    AddChild(mob);
	}
}
	