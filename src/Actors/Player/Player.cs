using Godot;
using System;

public class Player : Actor
{
    [Signal]
    delegate void PlayerDied(string howPlayerDied);

    private static string name = string.Empty;
    private static string prefix = string.Empty;
    private static string suffix = string.Empty;
    private string[] namePrefixes = {"artemis", "strimzi", "dispatch", "proton", "rhea", "qpid", "jboss", "fuse", "infinispan", "camel", "activemq", "drools", "fis"};
    private static int count = 0;

    private GameManager gameManager;
    private Label playerNameLabel;
    public StateMachine StateMachine;
    public AnimationPlayer AnimationPlayer;
    public Vector2 LookDirection = new Vector2(1, 1);
    public CPUParticles2D PlayerTrail;
    public CPUParticles2D PlayerBurst;


    public override void _Ready()
    {
        GD.Print("Player::_Ready()");
        //GD.Print(this.GetPath());
        this.StateMachine = this.GetNode("StateMachine") as StateMachine;
        this.AnimationPlayer = this.GetNode("AnimationPlayer") as AnimationPlayer;
        this.PlayerTrail = this.GetNode("PlayerTrail") as CPUParticles2D;
        this.PlayerBurst = this.GetNode("PlayerBurst") as CPUParticles2D;
        this.gameManager = GetNode<GameManager>("/root/GameManager");
        this.playerNameLabel = GetNode<Label>("BodyPivot/PlayerNameLabel");

        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");
        this.Connect("PlayerDied", this.gameManager, "_on_PlayerDied");

        this.SetPlayerName();
        this.playerNameLabel.Text = name;
        this.playerNameLabel.Visible = true;

        count++;
    }

    private void _on_GracePeriodExpired()
    {
        GD.Print("Player::_on_GracePeriodExpired()");
        this.StateMachine.TransitionTo("Die");
    }

    public void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GD.Print("Player::_on_AnimationPlayer_animation_finished() anim_name is " + anim_name);

        if (0 == this.gameManager.GracePeriod)
        {
            EmitSignal(nameof(PlayerDied), "expired");
        }
        else if ("Fall" == anim_name)
        {
            EmitSignal(nameof(PlayerDied), "collision");
            if (this.playerNameLabel.Visible)
            {
                this.playerNameLabel.Visible = false;
            }
        }

    }

    private void SetPlayerName()
    {
        if (prefix.Empty())
        {
            Random random = new Random();
            int prefixIndex = random.Next(0, this.namePrefixes.Length);
            prefix = this.namePrefixes[prefixIndex];
        }

        if (suffix.Empty())
        {
            this.GenerateSuffix();
        }

        name = prefix + "-" + count.ToString() + "-" + suffix;
    }

    private void GenerateSuffix()
    {
        Random random = new Random();
        byte[] arrSuffixChars = new byte[6];
        int asciiIndex = 0;

        arrSuffixChars[5] = 0;
        for (int i = 0; i < 5; ++i)
        {
            if (random.Next(0,3) > 0)
            {
                // choose letter
                asciiIndex = 97 + random.Next(0, 25);
            }
            else
            {
                // choose digit
                asciiIndex = 48 + random.Next(0, 9);
            }
            arrSuffixChars[i] = (byte)asciiIndex;
        }

        suffix = System.Text.Encoding.ASCII.GetString(arrSuffixChars);
    }

    public void _on_PlayerNameTimer_timeout()
    {
        this.playerNameLabel.Visible = false;
    }

}
