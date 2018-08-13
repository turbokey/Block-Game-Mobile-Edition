using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

        private AsyncOperation async = null;
        private bool isLoading = false;
        public string levelName = "";
    public Texture backgroundBar;
   public Texture lineBar;
   public Texture GUITextureBackground;

        private IEnumerator _Start(){
                Debug.Log( "Loading... " );
                async = Application.LoadLevelAsync( levelName );
                while( !async.isDone ){
                        Debug.Log( string.Format( "Loading {0}%", async.progress*100 ) );
                        yield return null;
                }
                Debug.Log( "Loading complete" );
                isLoading = false;
                yield return async;
        }

        private void OnGUI(){
      if(GUITextureBackground != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), GUITextureBackground);
                if( !isLoading ){
                        {
                                isLoading = true;
                                StartCoroutine("_Start"); 
                        }
                } else{
         GUI.Label(new Rect( (Screen.width / 6), (Screen.height / 2) , Screen.width*2/3, Screen.height/30),"");
			GUI.DrawTexture( new Rect( (Screen.width / 6), (Screen.height / 2), Screen.width*2/3, Screen.height/40 ), backgroundBar, ScaleMode.StretchToFill, true, 1F);
			GUI.DrawTexture( new Rect( (Screen.width / 6), (Screen.height / 2), async.progress*Screen.width*2/3, Screen.height/40 ), lineBar, ScaleMode.StretchToFill, true, 1F);
                }
        }
}