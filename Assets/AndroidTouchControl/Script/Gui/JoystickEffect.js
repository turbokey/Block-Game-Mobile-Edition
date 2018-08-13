//This script create visible effect when you touch Joystick

var gui : GUITexture;

function Start(){
   gui.color.a =.10;
}
function Update () {			
 
   var count = Input.touchCount;
   for(var i : int = 0;i < count; i++)
   {	
    	var firstTouch = Input.GetTouch(i);
        if (firstTouch.phase == TouchPhase.Began) 
        {
           if (gui.HitTest(firstTouch.position))
           {	
           	    gui.color.a =.50;  
           }
        }
        if (firstTouch.phase == TouchPhase.Ended) 
        {
           		gui.color.a =.10;    
        }
    }
}
