using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour
{
    public ToggleGroup toggleGroup;

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void Submit()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        // Check if a toggle is selected
        if (toggle != null)
        {
            if (toggle.name == "ToggleHD")
            {
                // Set resolution to 1920x1080 (HD)
                Screen.SetResolution(1920, 1080, true);
            }
            if (toggle.name == "Toggle2K")
            {
                // Set resolution to 2560x1440 (2K)
                Screen.SetResolution(2560, 1440, true);
            }
            if (toggle.name == "Toggle4K")
            {
                // Set resolution to 3840x2160 (4K)
                Screen.SetResolution(3840, 2160, true);
            }
        }
    }
}
