using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine.Rendering;
using System.Linq;

// 날짜 : 2021-04-26 PM 3:12:50
// 작성자 : Rito

namespace Rito.RadialMenu_v3.Test
{
    public class Test_RadialMenu : MonoBehaviour
    {
        public RadialMenu radialMenu;
        public KeyCode key = KeyCode.G;
        MyController myController;
        [Space]
        public Sprite[] sprites;
        public PlayerController playerController;     
        private int secili;
        public GameObject PauseP;
        Vector2 mover;
        private void Awake()
        {
            myController=new MyController();
            myController.MyGameplay.WeaponSelect.performed += wpn => Choose();
            myController.MyGameplay.WeaponSelect.canceled += wpn => SecimA();
            myController.MyGameplay.Rotate.performed += wpn => mover = wpn.ReadValue<Vector2>(); 
            myController.MyGameplay.Rotate.canceled += wpn => mover = Vector2.zero;
          
        }
        private void OnEnable()
        {
            myController.Enable();
           
        }
        private void OnDisable()
        {
            myController.Disable();
        }
        private void Start()
        {
            radialMenu.SetPieceImageSprites(sprites);
            
        }
         public void Choose()
        {
            if (PauseP.activeInHierarchy == false)
            {
                if (radialMenu.isActiveAndEnabled)
                {

                    SecimA();
                }
                else
                {
                    radialMenu.gameObject.SetActive(true);
                    radialMenu.Show();
                }
            }
   
        }
        private void Update()
        {
            Gamepad gamepado = Gamepad.current;
            if (PauseP.activeInHierarchy == false)
            {
                if (gamepado != null)
                {
                    if (radialMenu.isActiveAndEnabled && gamepado.rightStick.IsActuated(0.1f))
                    {
                        ClockwisePolarCoord gamepad = ClockwisePolarCoord.FromVector2(mover);
                        radialMenu._arrowRotationZ2 = -gamepad.Angle;
                        float fIndex = (gamepad.Angle / 360f) * radialMenu._pieceCount;
                        secili = Mathf.RoundToInt(fIndex) % radialMenu._pieceCount;

                    }
                    if (Input.GetButtonDown("Submit"))
                    {
                        SecimA();
                    }
                    for (int i = 0; i < radialMenu._pieceRects.Length; i++)
                    {
                        if (i != secili)
                        {
                            radialMenu._pieceRects[i].localScale = new Vector2(1f, 1f);
                        }
                        else
                            radialMenu._pieceRects[secili].localScale = new Vector2(1.2f, 1.2f);
                    }

                }

                else if (gamepado == null)
                {
                    if (Input.GetKeyDown(key))
                    {
                        radialMenu.gameObject.SetActive(true);
                        radialMenu.Show();

                    }

                    else if (Input.GetKeyUp(key))
                    {

                        int selected;
                        selected = radialMenu.Hide();
                        //Debug.Log($"Selected : {selected}");
                        playerController.Spell(selected);
                        //SecimA();
                    }

                }
            }
            else if(PauseP.activeInHierarchy == true)
            {
                radialMenu.gameObject.SetActive(false);
            }
                
        }
      void SecimA()
        {
            if (PauseP.activeInHierarchy==false)
            {
                radialMenu._selectedIndex = secili;
                secili = radialMenu.Hide();
                playerController.Spell(secili);
            }
          
        }
     
    }
}