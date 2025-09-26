using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TextBoxCharacter is a class that extends the 
/// TextBox class. This class is responsible for 
/// displaying the text box when an NPC is speaking,
/// as well as the sprite associated with it.
/// </summary>
public class TextBoxCharacter : TextBox
{
    public Canvas canvas;
    public string CharacterName;
    public Sprite Sprite;
    [SerializeField] private TextMeshProUGUI _nameTag;
    [SerializeField] private Image _characterImage;
    public Sprite male_sprite;
    public Sprite female_sprite;

    public override void Start()
    {
        base.Start();
        SetUpCharacterTextBox();
    }

    /// <summary>
    /// Sets up text box information
    /// needed to display
    /// character name.
    /// </summary>
    public void SetUpCharacterTextBox()
    {
        if (_nameTag != null)
        {
            if (CharacterName.Length > 0)
                _nameTag.text = CharacterName;
            else
                _nameTag.text = Player.Instance().Name;
        }
        if (Sprite != null)
            _characterImage.sprite = Sprite;
        else
            _characterImage.sprite = (Player.Instance().Sex != null && (Player.Instance().Sex.Equals("MALE") || (Player.Instance().Sex.Equals("MALEFE") && GameManager.Instance.Leaning.Equals("MALE")))) ? male_sprite : female_sprite;

        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 100;
        }
    }

    // public override void EndNarration()
    // {
    //     if(canvas != null)
    //         canvas.sortingOrder = 0;
    //     base.EndNarration();
    // }

    // public override void StartNarration(DialogueData dialogueData)
    // {
    //     if(canvas != null)
    //         canvas.sortingOrder = 100;
    //     base.StartNarration(dialogueData);
    // }

    public override IEnumerator StartTextBoxAnimation()
    {
        if (canvas != null)
            canvas.sortingOrder = 100;
        return base.StartTextBoxAnimation();
    }

    public override IEnumerator EndTextBoxAnimation()
    {
        if (canvas != null)
            canvas.sortingOrder = 0;
        return base.EndTextBoxAnimation();
    }
}