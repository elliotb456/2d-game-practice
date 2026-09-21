using Godot;
using System;

public partial class Hud : CanvasLayer
{

	[Signal]
	public delegate void StartGameEventHandler();
	private Label _message;
	private Timer _messageTimer;
	private Button _startButton;
	private Label _score;


	public override void _Ready()
	{
		_message = GetNode<Label>("Message");
		_messageTimer = GetNode<Timer>("MessageTimer");
		_startButton = GetNode<Button>("StartButton");
		_score = GetNode<Label>("ScoreLabel");
	}

	public void ShowMessage(string text)
	{
		_message.Text = text; 
		_message.Show();

		_messageTimer.Start();
	}


	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		await ToSignal(_messageTimer, Timer.SignalName.Timeout);

		_message.Text = "Dodge the Aliens!";
		_message.Show();

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		_startButton.Show();
	}


	public void UpdateScore(int score)
	{
		_score.Text = score.ToString();
	}


	private void OnStartButtonPressed()
	{
    	_startButton.Hide();
    	EmitSignal(SignalName.StartGame);
	}


	private void OnMessageTimerTimeout()
	{
    	_message.Hide();
	}
}
