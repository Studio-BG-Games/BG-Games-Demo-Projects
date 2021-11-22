using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.UISystem;
using UnityEngine;
using Scripts;

public class ShopQuestionWindow : WindowController
{
   public event Action OnExitButtonClick;
   public event Action OnContinueShoppingButtonClock;
   
   public void Exit()
   {
      OnExitButtonClick?.Invoke();
      CloseWindow();
   }

   public void ContinueShopping()
   {
      OnContinueShoppingButtonClock?.Invoke();
      CloseWindow();
   }
}
