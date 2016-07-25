using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorController : MonoBehaviour {

    public Button applyColorButton;
    public InputField rText;
    public InputField gText;
    public InputField bText;
    public static Color32 sharedEditorColor;

    private Color32 defAddEventButtonCol;

    void Awake ()
    {
        defAddEventButtonCol = new Color32(200, 200, 200, 200);
        sharedEditorColor = defAddEventButtonCol;

        rText.contentType = InputField.ContentType.IntegerNumber;
        rText.characterLimit = 3;

        gText.contentType = InputField.ContentType.IntegerNumber;
        gText.characterLimit = 3;

        bText.contentType = InputField.ContentType.IntegerNumber;
        bText.characterLimit = 3;
    }
	
	void Update ()
    {
	
	}

    // If the inputed values are valid, set the universal edit mode colors to the given RGBA color
    public void SetEditorColor()
    {
        int n;
        if (int.TryParse(rText.text, out n) && int.TryParse(gText.text, out n) && int.TryParse(bText.text, out n) && 
            (Int16.Parse(rText.text) >= 0 && Int16.Parse(rText.text) <= 255) && (Int16.Parse(gText.text) >= 0 && Int16.Parse(gText.text) <= 255) && 
            (Int16.Parse(bText.text) >= 0 && Int16.Parse(bText.text) <= 255) && rText.text.Length != 0 && gText.text.Length != 0 && bText.text.Length != 0)
        {
            sharedEditorColor = new Color32(Byte.Parse(rText.text), Byte.Parse(gText.text), Byte.Parse(bText.text), 255);
        }    
    }
}
