using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;

public class QuitButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<XButton>().Clicked(() =>
        {
            Application.Quit();
        });
    }

#if UNITY_EDITOR
    public void Build(Canvas main)
    {
        XButton b = XButton.New("Quit", null);
        b.Get().Size(200f, 40f).Left().Down().SetScene(main);
        b.gameObject.AddComponent<QuitButton>();
    }
#endif
}
