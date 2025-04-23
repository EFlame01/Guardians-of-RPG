using UnityEngine;

/// <summary>
/// TransitionTexture is a class
/// that stores the information 
/// needed to transition from one
/// scene to another scene.
/// </summary>
[System.Serializable]
public class TransitionTexture
{
    public TransitionType Type;
    public Texture2D Texture;
}