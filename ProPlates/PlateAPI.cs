using ProPlates.Components;
using TMPro;
using UnityEngine;
using VRC;
using Object = UnityEngine.Object;

// developers: feel free to use this API in your mods, just be sure to credit
// me when using it
// note: this requires the OpacityListener component as well, so be sure to
// include that if you are using this API

namespace ProPlates;

public class Plate
{
    private const string PlateParent = "Contents/Status Line";
    // backing fields hrffff
    private Player _player;
    private string _text;
    private Color _color;
    private Vector3 _position;

    /// <summary>
    /// Plate constructor.
    /// </summary>
    /// <param name="player">The player to create a Plate for</param>
    /// <param name="text">Text to display</param>
    /// <param name="color">Text color</param>
    /// <param name="position">Local position. Default is (0, 0, 0)</param>
    /// <param name="containerPrefix">GameObject name prefix. Formatted as "{containerName} Container". Default is "PlateAPI".</param>
    public Plate(Player player, string text, Color color, Vector3 position = default, string containerPrefix = "PlateAPI")
    {
        this.Player = player;
        this.Text = text;
        this.Color = color;
        this.Position = position;

        PlayerNameplate nameplate = this.Player._vrcplayer.field_Public_PlayerNameplate_0;

        Transform newPlate = Object.Instantiate(nameplate.transform.Find("Contents/Quick Stats"),
            nameplate.transform.Find(PlateParent), false);

        newPlate.name = $"{containerPrefix} Container";
        newPlate.localPosition = this.Position;
        newPlate.gameObject.active = true;

        // this component matches the plate opacity to the nameplate's opacity
        OpacityListener opacityListener = newPlate.gameObject.AddComponent<OpacityListener>();
        opacityListener.reference = nameplate.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>();
        opacityListener.target = newPlate.gameObject.GetComponent<ImageThreeSlice>();

        // remove unnecessary gameobjects and set text
        for (int i = newPlate.childCount; i > 0; i--)
        {
            Transform c = newPlate.GetChild(i - 1);
            if (c.name != "Trust Text")
            {
                Object.DestroyImmediate(c.gameObject);
                continue;
            }
            c.name = "Text";
            c.GetComponent<TextMeshProUGUI>().text = this.Text;
            c.GetComponent<TextMeshProUGUI>().color = this.Color;
        }

        this.GameObject = newPlate.gameObject;
    }

    /// <summary>
    /// GameObject representing the Plate.
    /// </summary>
    public GameObject GameObject { get; private set; }

    public Player Player
    {
        get => this._player;
        set
        {
            this._player = value;
            if (this.GameObject == null) return;
            PlayerNameplate newPlayerNameplate = value._vrcplayer.field_Public_PlayerNameplate_0;
            this.GameObject.transform.SetParent(newPlayerNameplate.transform.Find(PlateParent), false);
            this.GameObject = newPlayerNameplate.transform.Find(PlateParent).gameObject;
        }
    }

    /// <summary>
    /// Displayed text.
    /// </summary>
    public string Text
    {
        get => this._text;
        set
        {
            this._text = value;
            if (this.GameObject == null) return;
            this.GameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = value;
        }
    }

    /// <summary>
    /// Text color.
    /// </summary>
    public Color Color
    {
        get => this._color;
        set
        {
            this._color = value;
            if (this.GameObject == null) return;
            this.GameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().color = value;
        }
    }

    /// <summary>
    /// Plate position.
    /// </summary>
    public Vector3 Position
    {
        get => this._position;
        set
        {
            this._position = value;
            if (this.GameObject == null) return;
            this.GameObject.transform.localPosition = value;
        }
    }


}