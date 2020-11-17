using Godot;
using System;

public class Main : Control
{
    private Label storyLabel;
    private GameManager gameManager;
    private Button quitButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        storyLabel = GetNode<Label>("StoryLabel");
        gameManager = GetNode<GameManager>("/root/GameManager");
        quitButton = GetNode<Button>("CenterContainer/VBoxContainer/HBoxContainer/QuitButton");
        quitButton.Visible = false;
        if (OS.GetName() == "HTML5")
        {
            // no quitting in HTML web browser mode
            quitButton.Visible = false;
        }

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_start"))
        {
            this._on_PlayButton_button_up();
        }
        else if (@event.IsActionPressed("ui_select"))
        {
            this._on_CreditsButton_button_up();
        }
    }

    private void _on_CreditsButton_button_up()
    {
//        GD.Print("credits selected from main menu");
        this.GetTree().ChangeScene("res://src/Scenes/Credits.tscn");
    }

    private void _on_PlayButton_button_up()
    {
//        GD.Print("new game from main menu");
        gameManager.newGame();
    }

    private void _on_QuitButton_button_up()
    {
//        GD.Print("player wants to quit from main menu");
        // TODO: prompt "are you sure?"
        // TODO: quit game completely
        GetTree().Quit();
    }
}

