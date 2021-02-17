using Rhynn.UI;
using UnityEngine;

namespace Rhynn.App
{
    public class Program : MonoBehaviour
    {
        [SerializeField] private GameObject userInterfacePrefab;
        
        // The main entry point for the program
        void Start()
        {
            // Set up starting user interface (for character selection and generation, loading save, etc)
            _userInterface = Instantiate(userInterfacePrefab).GetComponent<UserInterface>();
            _userInterface.Run();
        }

        private UserInterface _userInterface;
    }
}
