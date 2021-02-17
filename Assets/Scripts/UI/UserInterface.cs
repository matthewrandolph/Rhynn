using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Rhynn.UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private Screen startScreen;
        
        public Screen CurrentScreen
        {
            get
            {
                if (_screens.Count == 0) return null;

                return _screens.Peek();
            }
        }

        private void Start()
        {
            // Set up InputSystem control action listeners for MenuControl.Up, MenuControl.Down, MenuControl.Left,
            // and MenuControl.Right as well as maybe a listener for the Quit event
        }

        public void Run()
        {
            SetScreen(startScreen);
        }

        public void SetScreen(NotNull<Screen> screen)
        {
            // Remove the current screen (if any)
            if (_screens.Count > 0)
            {
                //if (_screens.Count == 0) throw new InvalidOperationException("Cannot pop when there are no screens on the stack.");
                
                // Unbind the screen
                IUserInterfaceScreen oldScreen = (IUserInterfaceScreen) _screens.Pop();
                oldScreen.Detach(this);
            }
            
            _screens.Push(screen);

            IUserInterfaceScreen uiScreen = (IUserInterfaceScreen) screen.Value;
            uiScreen.Attach(this);
            
            Repaint();
        }

        public void PushScreen(NotNull<Screen> screen)
        {
            if (CurrentScreen != null)
            {
                IUserInterfaceScreen oldScreen = CurrentScreen;
                oldScreen.Deactivate();
            }
            
            _screens.Push(screen);

            IUserInterfaceScreen uiScreen = (IUserInterfaceScreen) screen.Value;
            uiScreen.Attach(this);
            
            Repaint();
        }

        public void PopScreen()
        {
            if (_screens.Count == 0) throw new InvalidOperationException("Cannot pop when there are no screens on the stack.");
            
            // Unbind the screen
            Screen screen = _screens.Pop();

            IUserInterfaceScreen uiScreen = (IUserInterfaceScreen) screen;
            uiScreen.Detach(this);

            if (CurrentScreen != null)
            {
                IUserInterfaceScreen newScreen = CurrentScreen;
                newScreen.Activate();
            }
            
            Repaint();
        }

        public void Repaint()
        {
            int i = 0;
            foreach (Screen screen in _screens.Reverse())
            {
                screen.Canvas.sortingLayerID = i;
                i++;
            }
        }

        private readonly Stack<Screen> _screens = new Stack<Screen>();
    }
}