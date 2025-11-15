

using UnityEngine;

public class TextBoxBattle : TextBox
{
    public volatile static bool KeepTextBoxOpened;
    public volatile static bool EndNarrationNow;

    public override void Update()
    {
        base.Update();

        if (EndNarrationNow)
        {
            EndNarrationNow = false;
            KeepTextBoxOpened = false;
            EndNarration();
        }
    }

    public override void EndNarration()
    {
        if (KeepTextBoxOpened)
            return;
        else
            base.EndNarration();
    }
}