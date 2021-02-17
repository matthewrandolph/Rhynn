using System.Collections.Generic;
using UnityEngine;
using Util.PostOffice;

namespace Rhynn.UI
{
    public abstract class Control : MonoBehaviour
    {
        protected virtual void Awake()
        {
            gameObject.SetActive(true);
        }

        public virtual void Init()
        {
            // Nothing to do here
        }

        public virtual void End()
        {
            // raise OnControlEnd message
            PostOffice<Control>.SendEvent(Message.OnControlEnd,this);
        }
    }
}