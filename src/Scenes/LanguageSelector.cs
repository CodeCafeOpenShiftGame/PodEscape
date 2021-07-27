using Godot;
using System;

public class LanguageSelector : Control
{
	private OptionButton button;
	private Godot.Collections.Array languages;
	
	public override void _Ready()
	{
		button = GetNode<OptionButton>("OptionButton");

		string current = TranslationServer.GetLocale();
		languages = TranslationServer.GetLoadedLocales();
		for (int i = 0; i < languages.Count; i++) {
			string code = (string)languages[i];
			button.AddItem(TranslationServer.GetLocaleName(code), i);
			if (code == current) {
				button.Select(i);
			}
		}
	}

	private void _on_OptionButton_item_selected(int index)
	{
		string lang = (string)languages[index];
		GD.Print("Select: " + index);
		TranslationServer.SetLocale(lang);
	}
}
