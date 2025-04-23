using System;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour, IDialogue
{
    [Header("Dialogue")]
    [SerializeField] public DialogueData DialogueData;
    
    [Header("Class Field")]
    [SerializeField] public TextMeshProUGUI descriptionText;

    [Header("Archetype Field")]
    [SerializeField] public TextMeshProUGUI archetypeButton1Text;
    [SerializeField] public TextMeshProUGUI archetypeButton2Text;
    [SerializeField] public TextMeshProUGUI atkStatText1;
    [SerializeField] public TextMeshProUGUI defStatText1;
    [SerializeField] public TextMeshProUGUI hpStatText1;
    [SerializeField] public TextMeshProUGUI spdStatText1;
    [SerializeField] public TextMeshProUGUI evaStatText1;

    [Header("Name Field")]
    [SerializeField] public TextMeshProUGUI nameTextField;
    [SerializeField] public TextMeshProUGUI[] keyboard;

    [Header("Verification Field")]
    [SerializeField] public TextMeshProUGUI nameText;
    [SerializeField] public TextMeshProUGUI archetypeText;
    [SerializeField] public TextMeshProUGUI bitsText;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI limText;
    [SerializeField] public TextMeshProUGUI currText;
    [SerializeField] public TextMeshProUGUI atkStatText2;
    [SerializeField] public TextMeshProUGUI defStatText2;
    [SerializeField] public TextMeshProUGUI evaStatText2;
    [SerializeField] public TextMeshProUGUI hpStatText2;
    [SerializeField] public TextMeshProUGUI spdStatText2;
    [SerializeField] public Image playerImage;
    [SerializeField] public Sprite maleSprite;
    [SerializeField] public Sprite femaleSprite;

    [Header("Intros")]
    [SerializeField] public IntroObject[] IntroObjects;

    private bool generatedMove = false;

    [Serializable]
    public struct IntroObject
    {
        public GameObject o;
        public int state;
    }

    //Private Fields
    private Story story;
    private int currentState;
    private string saveTheWorld = "";
    private string sex = "";
    private string playerClass = "";
    private string archetype = "";
    private bool shiftOn = true;
    private int letterIndex = 0;
    private string playerName;
    private Player player;

    public void StartDialogue()
    {
        DialogueManager.Instance.DisplayNextDialogue(DialogueData);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(story != null)
            currentState = (int)story.variablesState["currentState"];
        
        if(DialogueManager.Instance.DialogueEnded)
            StartGame();
        
        switch(currentState)
        {
            case 1:
                Intro1();
                break;
            case 2:
                Intro2();
                break;
            case 3:
                Intro3();
                break;
            case 4:
                Intro4();
                break;
            case 5:
                Intro5();
                break;
            case 6:
                Intro6();
                break;
        }
    }

    private void Init()
    {
        ActivateState(0);    
        StartDialogue();
        player = Player.Instance();
        story = DialogueManager.Instance.CurrentStory;
        currentState = (int)story.variablesState["currentState"];
    }
    
    private void Intro1()
    {
        if(story != null && saveTheWorld.Length <= 0)
            saveTheWorld = (string)story.variablesState["saveTheWorld"];

        if(saveTheWorld.Equals("No") && DialogueManager.Instance.DialogueEnded)
            SceneLoader.Instance.LoadScene("Start Scene", TransitionType.FADE_TO_BLACK);
    }

    private void Intro2()
    {
        ActivateState(2); 
        if(story!= null && sex.Length >= 0)
            story.variablesState["sex"] = sex;   
    }

    private void Intro3()
    {
        ActivateState(3); 
        player.SetSex(sex);
        
        if(story!= null && playerClass.Length >= 0)
            story.variablesState["class"] = playerClass; 
    }

    private void Intro4()
    {
        switch(playerClass)
        {
            case "SWORDSMAN":
                archetypeButton1Text.text = "REGULAR SWORDSMAN";
                archetypeButton2Text.text = "DUAL SWORDSMAN";
                break;
            case "DEFENDER":
                archetypeButton1Text.text = "KNIGHT";
                archetypeButton2Text.text = "HEAVY SHIELDER";
                break;
            case "ESOIC":
                archetypeButton1Text.text = "ENERGY MANIPULATOR";
                archetypeButton2Text.text = "NATURE MANIPULATOR";
                break;
            case "BRAWLER":
                archetypeButton1Text.text = "MIXED MARTIAL ARTIST";
                archetypeButton2Text.text = "BERSERKER";
                break;
            case "SPECIALIST":
                archetypeButton1Text.text = "COMBAT SPECIALIST";
                archetypeButton2Text.text = "WEAPON SPECIALIST";
                break;
            default:
                break;
        }
        
        ActivateState(4); 
        
        if(story != null && archetype.Length >= 0)
            story.variablesState["archetype"] = true;
    }

    private void Intro5()
    {
        ActivateState(5); 
        player.SetArchetype(archetype);
        player.SetBaseStats
        (
            player.Archetype.BaseStats.Hp,
            player.Archetype.BaseStats.Atk,
            player.Archetype.BaseStats.Def,
            player.Archetype.BaseStats.Eva,
            player.Archetype.BaseStats.Hp,
            player.Archetype.BaseStats.Spd,
            Units.BASE_ACC,
            Units.BASE_CRT
        );
        if(!generatedMove)
        {
            generatedMove = true;
            string moveName = null;
            moveName ??= MoveMaker.Instance.GetLevelUpMoves(1, player.Archetype.ArchetypeName, player.Archetype.ClassName)[0].Name;
            player.MoveManager.AddMove(moveName);
        }
        
        string newName = nameTextField.text.Replace("_", "").Trim();
        
        if(newName.Length > 0)
            story.variablesState["name"] = true;
        else
            story.variablesState["name"] = false;

        story.variablesState["playerName"] = newName;
        playerName = newName;
    }

    private void Intro6()
    {
        player.SetName(playerName);
        nameText.text = player.Name;
        archetypeText.text = player.Archetype.ArchetypeName;
        bitsText.text = player.Bits.ToString();
        levelText.text = player.Level.ToString();
        limText.text = player.LimXP.ToString();
        currText.text = player.CurrXP.ToString();
        atkStatText2.text = player.BaseStats.Atk.ToString();
        defStatText2.text = player.BaseStats.Def.ToString();
        evaStatText2.text = player.BaseStats.Eva.ToString();
        hpStatText2.text = player.BaseStats.Hp.ToString();
        spdStatText2.text = player.BaseStats.Spd.ToString();
        playerImage.sprite = (player.Sex.Equals("MALE") || (player.Sex.Equals("MALEFE") && GameManager.Instance.Leaning.Equals("MALE"))) ? maleSprite : femaleSprite;

        ActivateState(6); 
    }

    private void StartGame()
    {
        ActivateState(0); 
        ChapterScene.PartName = "Part 1: Grace Land";
        ChapterScene.ChapterName = "Chapter 1 - The Person and the Bear";
        ChapterScene.SceneName = "Forest 1";
        SceneLoader.Instance.LoadScene("Chapter Scene", TransitionType.FADE_TO_BLACK);
    }

    private void ActivateState(int state)
    {
        foreach(IntroObject io in IntroObjects)
        {
            if(io.state == state)
                io.o.SetActive(true);
            else
                io.o.SetActive(false);
        }
    }

    public void SelectSex(string s)
    {
        sex = s;
    }

    public void SelectClass(string c)
    {
        playerClass = c;
        
        switch(c)
        {
            case "SWORDSMAN":
                descriptionText.text = "<b>SWORDSMEN</b> typically channel BINARY energy into their blades. They are known for their strong ATTACK stats.";
                break;
            case "DEFENDER":
                descriptionText.text = "<b>DEFENDERS</b> typically channel BINARY energy into their shield and armor. They are known for their strong DEFENSE stats.";
                break;
            case "ESOIC":
                descriptionText.text = "<b>ESOICS</b> typically channel BINARY energy into their minds. They are known for having great HEALTH stats.";
                break;
            case "BRAWLER":
                descriptionText.text = "<b>BRAWLERS</b> typically channel BINARY energy into their bodies. They are known for having great SPEED stats.";
                break;
            case "SPECIALIST":
                descriptionText.text = "<b>SPECIALISTS</b> typically channel BINARY energy to the world around them. They are known for their sharp EVASION stats.";
                break;
        }
    }

    public void SelectArchetype(int option)
    {
        archetype = option == 1 ? archetypeButton1Text.text : archetypeButton2Text.text;
        Archetype a = Archetype.GetArchetype(archetype);

        atkStatText1.text = a.BaseStats.Atk.ToString();
        defStatText1.text = a.BaseStats.Def.ToString();
        hpStatText1.text = a.BaseStats.Hp.ToString();
        spdStatText1.text = a.BaseStats.Spd.ToString();
        evaStatText1.text = a.BaseStats.Eva.ToString();

        story.variablesState["archetype"] = true;
    }

    public void AddLetter(string letter)
    {
        char[] newName = nameTextField.text.ToCharArray();

        if(letterIndex >= nameTextField.text.Length)
            return;
        if(letter == "SHIFT")
        {
            shiftOn = !shiftOn;
            Shift();
            return;
        }
        
        if(letter == "SPACE")
            letter = " ";
        else if(letter == "BACKSPACE")
        {
            letterIndex--;
            letter = "_";
        }
        else
            letter = shiftOn ? letter.ToUpper() : letter.ToLower();

        newName[Mathf.Clamp(0, letterIndex, 9)] = letter.ToCharArray()[0];
        nameTextField.text = new string(newName);

        letterIndex = Mathf.Clamp(0, !letter.Equals("_") ? letterIndex + 1 : letterIndex, 9);
    }

    private void Shift()
    {
        foreach(TextMeshProUGUI key in keyboard)
            key.text = shiftOn ? key.text.ToUpper() : key.text.ToLower();
    }
}