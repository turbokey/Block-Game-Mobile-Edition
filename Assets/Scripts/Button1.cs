using UnityEngine;
using System.Collections;

public class Button1 : MonoBehaviour {
	Vector3 temp_pos1 = new Vector3(0,0,0);
	Vector3 temp_pos2 = new Vector3(0,0,0);
	public GameObject Button11;
	public GameObject Button22;
	void Start () {
		temp_pos1 = new Vector3(Screen.width*10/11,Screen.height*2/3,0); 
		temp_pos2 = new Vector3(Screen.width*9/11,Screen.height/2,0); 
		Button11.transform.position=temp_pos1;
		Button22.transform.position=temp_pos2;
	}

}
