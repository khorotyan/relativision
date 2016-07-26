using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeleteProject : MonoBehaviour
{
    Button curButton;

	void Start ()
    {
        curButton = gameObject.GetComponent<Button>();
        curButton.onClick.AddListener(delegate () { DeleteButton(); });
    }
	
	void Update ()
    {
        
	}

    public void DeleteButton()
    {
        Destroy(curButton.transform.parent.gameObject);
    }


}
