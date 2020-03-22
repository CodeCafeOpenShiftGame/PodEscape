using Godot;
using System;

public class ScreenShake : Node
{
    public int amplitude = 0;
    Tween shakeTween;
    Camera2D camera2D;
    Timer frequency;
    Timer duration;

    public const int OFFSET_X = 768;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        shakeTween = this.GetNode("ShakeTween") as Tween;
        camera2D = this.GetParent() as Camera2D;
        frequency = this.GetNode("Frequency") as Timer;
        duration = this.GetNode("Duration") as Timer;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Start(float duration = 0.2f, float frequency = 15f, int amplitude = 16)
    {
        this.amplitude = amplitude;
        this.duration.WaitTime = duration;
        this.frequency.WaitTime = 1 / frequency;

        this.duration.Start();
        this.frequency.Start();

        NewShake();
    }

    public void NewShake()
    {
        Vector2 randVector = new Vector2(0f, 0f);
        Random random = new Random();
        randVector.x = random.Next(-amplitude, amplitude) + OFFSET_X;
        randVector.y = random.Next(-amplitude, amplitude);

        shakeTween.InterpolateProperty(
            camera2D,
            "offset",
            camera2D.Offset,
            randVector,
            frequency.WaitTime,
            Tween.TransitionType.Sine,
            Tween.EaseType.InOut
        );
        shakeTween.Start();
    }

    public void Reset()
    {
        shakeTween.InterpolateProperty(
            camera2D,
            "offset",
            camera2D.Offset,
            new Vector2(OFFSET_X, 0),
            frequency.WaitTime,
            Tween.TransitionType.Sine,
            Tween.EaseType.InOut
        );
        shakeTween.Start();
    }

    public virtual void OnFrenquencyTimeout()
    {
        this.NewShake();
    }

    public virtual void OnDurationTimeout()
    {
        this.Reset();
        this.frequency.Stop();
    }

    public virtual void _on_Dash_DashSignal()
    {
        this.Start();
    }

}
