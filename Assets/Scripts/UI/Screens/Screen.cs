using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Rhynn.UI
{
    [RequireComponent(typeof(Canvas))]
    public class Screen : Control, IUserInterfaceScreen
    {
        public UserInterface UI => _UI;
        public Canvas Canvas => _canvas;

        protected override void Awake()
        {
            gameObject.SetActive(false);
            _canvas = GetComponent<Canvas>();
        }
        
        private void Start()
        {
            _controls.AddRange(GetComponentsInChildren<Control>(true));
            _controls.Remove(this);
        }

        protected virtual void OnActivate()
        {
            // do nothing
        }

        protected virtual void OnDeactivate()
        {
            // do nothing
        }

        public void Attach(NotNull<UserInterface> ui)
        {
            if (_UI != null) throw new InvalidOperationException("Can only attach a Screen to a UI once.");
            
            _UI = ui;
            gameObject.SetActive(true);

            Init();
            
            // send to child controls
            foreach (Control child in _controls)
            {
                child.Init();
            }
        }

        public void Detach(NotNull<UserInterface> ui)
        {
            if (_UI == null) throw new InvalidOperationException("Cannot detach an unattached Screen.");

            _UI = null;

            End();
            
            foreach (Control child in _controls)
            {
                child.End();
            }

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            OnActivate();
        }

        public void Deactivate()
        {
            OnDeactivate();
        }

        private UserInterface _UI;
        private Canvas _canvas;
        private List<Control> _controls = new List<Control>();
    }
}