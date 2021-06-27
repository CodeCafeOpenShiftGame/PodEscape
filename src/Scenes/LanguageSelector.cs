using Godot;
using System;

public class LanguageSelector : Control
{
	private OptionButton button;
	private Godot.Collections.Array languages;
	
	public override void _Ready()
	{
		button = GetNode<OptionButton>("OptionButton");

		languages = TranslationServer.GetLoadedLocales();
		foreach (string lang in languages) {
			button.AddItem(lang.ToUpper());
		}
	}

	private void _on_OptionButton_item_selected(int index)
	{
		string lang = (string)languages[index];
		TranslationServer.SetLocale(lang);
	}

}
