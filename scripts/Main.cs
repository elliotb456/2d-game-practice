using Godot;
using System;

public partial class Main : Node2D
{

	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
	private AudioStreamPlayer2D _music;
	private AudioStreamPlayer2D _deathSound;
	private Timer _mobTimer;
	private Timer _scoreTimer;
	private Timer _startTimer;
	private Hud _hud;
	private PathFollow2D _mobSpawnLocation;
	private Player _player;
	private Marker2D _startPosition;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_music = GetNode<AudioStreamPlayer2D>("Music");
		_deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
		_mobTimer = GetNode<Timer>("MobTimer");
		_scoreTimer = GetNode<Timer>("ScoreTimer");
		_startTimer = GetNode<Timer>("StartTimer");
		_hud = GetNode<Hud>("HUD");
		_mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		_player = GetNode<Player>("Player");
		_startPosition = GetNode<Marker2D>("StartPosition");
	}


	public void GameOver()
	{
		_mobTimer.Stop();
		_scoreTimer.Stop();
		_music.Stop();
		_deathSound.Play();

		_hud.ShowGameOver();
	
	}


	public void NewGame()
	{
		_score = 0;
		_music.Play();

		_player.Start(_startPosition.Position);

		_hud.UpdateScore(_score);
		_hud.ShowMessage("Get Ready!");

		// Note that for calling Godot-provided methods with strings, we have to use the original Godot snake_case name.
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

		_startTimer.Start();
	}


	private void OnScoreTimerTimeout()
	{
    _score++;
	_hud.UpdateScore(_score);
	}


	private void OnStartTimerTimeout()
	{
    	_mobTimer.Start();
    	_scoreTimer.Start();
	}


	private void OnMobTimerTimeout()
	{
    // Create a new instance of the Mob scene.
    Mob mob = MobScene.Instantiate<Mob>();

    // Choose a random location on Path2D.
    _mobSpawnLocation.ProgressRatio = GD.Randf();

    // Set the mob's direction perpendicular to the path direction.
    float direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;

    // Set the mob's position to a random location.
    mob.Position = _mobSpawnLocation.Position;

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
	