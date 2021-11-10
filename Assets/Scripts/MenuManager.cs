using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenSceneWithHeart() => SceneManager.LoadScene("Heart");
    public void OpenSceneWithTikTok() => SceneManager.LoadScene("TikTok");
    public void OpenSceneWithTogepi() => SceneManager.LoadScene("Togepi");
}
