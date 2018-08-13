//This script create visible effect when you touch Jump Button

var jumpButton: GUITexture;

function Start () {
   jumpButton.pixelInset = Rect (-Screen.height/6,Screen.height/2,Screen.height/7, Screen.height/7);
   jumpButton.color.a =.50;	
}
function Update () {

   var count = Input.touchCount;
   for(var i : int = 0;i < count; i++)
    {	
    	var firstTouch = Input.GetTouch(i);
        if (firstTouch.phase == TouchPhase.Began) 
        {
           if (jumpButton.HitTest(firstTouch.position))
           {	
           		jumpButton.color.a =.10;           	   
           }
        }
        if (firstTouch.phase == TouchPhase.Ended) 
        {
           		jumpButton.color.a =.50;                   
        }
    }
}