using Godot;
using System;

public class LanguageSelector : Control
{
	private OptionButton button;
	
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<OptionButton>("OptionButton");

		Godot.Collections.Array languages = TranslationServer.GetLoadedLocales();
		foreach (string lang in languages) {
			button.AddItem(lang);
		}
	}


	private void _on_OptionButton_item_selected(int index)
	{
		string lang = button.GetItemText(index);
		TranslationServer.SetLocale(lang);
	}

}
