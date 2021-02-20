using Rhynn.Input;

namespace Rhynn.UI
{
    /// <summary>
    /// This state means some animation is active and the game should ignore input that relates to issuing commands.
    /// (Menus and the like should still work fine). This is active when the player is animating as well as when AI
    /// (or possibly other players?) are taking their turns.
    /// </summary>
    public class AnimatingState : GameUIState
    {
        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }
        
        public AnimatingState(GameScreen screen) : base(screen) { }
    }
}