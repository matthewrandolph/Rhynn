using Rhynn.Engine;
using UnityEngine;
using Util;
using Util.PostOffice;

namespace UI.Graphics
{
    public class ActorGfx : MonoBehaviour
    {
        private void Start()
        {
            _actor.Moved += OnMoved;
        }

        public void Init(Actor actor)
        {
            _actor = actor;
        }
        
        public void OnMoved(object sender, ValueChangeEventArgs<Vec2> eventArgs)
        {
            Vector3 worldPosition = new Vector3(eventArgs.New.x, 0, eventArgs.New.y);
            gameObject.transform.position = worldPosition;
        }

        private Actor _actor;
    }
}