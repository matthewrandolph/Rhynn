using Util;

namespace Rhynn.UI
{
    public interface IUserInterfaceScreen
    {
        void Attach(NotNull<UserInterface> ui);
        void Detach(NotNull<UserInterface> ui);

        /// <summary>
        /// Called when the Screen has become the current screen after having a screen above it removed.
        /// </summary>
        void Activate();

        /// <summary>
        /// Called when the Screen is no longer the current screen after having a new one added above it.
        /// </summary>
        void Deactivate();
    }
}