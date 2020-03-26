using Godot;
using System;

public class ScreenShake : Node
{
    public int amplitude = 0;
    Tween shakeTween;
    Camera2D camera2D;
    Timer frequency;
    Timer duration;

    public float offset_x;
    public float offset_y;

    public override void _Ready()
    {
        base._Ready();
        shakeTween = this.GetNode("ShakeTween") as Tween;
        camera2D = this.GetParent() as Camera2D;
        this.offset_x = this.camera2D.Offset.x;
        this.offset_y = this.camera2D.Offset.y;
        frequency = this.GetNode("Frequency") as Timer;
        duration = this.GetNode("Duration") as Timer;
    }

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
        randVector.x = random.Next(-amplitude, amplitude) + this.offset_x;
        randVector.y = random.Next(-amplitude, amplitude) + this.offset_y;

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
            new Vector2(offset_x, offset_y),
            frequency.WaitTime,
            Tween.TransitionType.Sine,
            Tween.EaseType.InOut
        );
        shakeTween.Start();
    }

    public virtual void OnFrequencyTimeout()
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
