//This script create visible effect when you touch X Button

var xButton: GUITexture;

function Start () {
   xButton.pixelInset = Rect (-Screen.height/3,Screen.height/2,Screen.height/7, Screen.height/7);
   xButton.color.a =.50;	
}
function Update () {

   var count = Input.touchCount;
   for(var i : int = 0;i < count; i++)
    {	
    	var firstTouch = Input.GetTouch(i);
        if (firstTouch.phase == TouchPhase.Began) 
        {
           if (xButton.HitTest(firstTouch.position))
           {	
           		xButton.color.a =.10;           	   
           }
        }
        if (firstTouch.phase == TouchPhase.Ended) 
        {
           		xButton.color.a =.50;                   
        }
    }
}