using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YESorNO_Btn : MonoBehaviour
{
    #region No 버튼을 눌렸을 때 다시 Scene[4_ChooseHero]을 불려오기
    public void NO_AgainScene_4_ChooseHero()
    {
        //다른 씬에 오브젝트 불려오기(GetRootGameObjects()[2]는 Canva가 3번째에 있어서)
        GameObject canvas = SceneManager.GetSceneByName("4_ChooseHero").GetRootGameObjects()[2]; 
        Button[] button = canvas.GetComponentsInChildren<Button>(); //버튼이 여러개가 있어서 자식여러개를 배열로 불려옴
        for(int i = 0; i<button.Length; ++i)
        {
            if (!button[i].interactable)
            {
                button[i].interactable = true;
            }
        }
        SceneManager.UnloadSceneAsync("4-1_HeroChooseYESorNO");
    }
    #endregion

    public void Yes_NestScene_5_HeroScreen()
    {
        SceneManager.LoadScene("5_HeroScreen"); //다음 씬으로
    }
}
