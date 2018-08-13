using UnityEngine;
using System.Collections;

public class Menumanager : MonoBehaviour {
	public void GenerateWorld(){
		Application.LoadLevel("World2");
	}

	public void Quit(){
		Application.Quit();
	}
}
