using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GifCtrl : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private Sprite[] m_SpriteArray;
    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private int m_IndexSprite = 0;
    private bool isDone = true;
    private void Awake()
    {
        m_Image = GetComponent<Image>();
        // StartCoroutine(Func_PlayAnim());
    }


    private void Update()
    {
        if (m_IndexSprite >= m_SpriteArray.Length) m_IndexSprite = 0;
          
        m_Image.sprite = m_SpriteArray[m_IndexSprite++];               

    }
   

}
