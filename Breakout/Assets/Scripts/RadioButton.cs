using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour
{
    public ToggleGroup toggleGroup;

    // Default resolution settings
    private string defaultResolution = "ToggleHD";

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        // Load the last saved resolution or default to the default resolution
        string savedResolution = PlayerPrefs.GetString("SelectedResolution", defaultResolution);

        // Set the toggle based on the saved resolution
        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = toggle.name == savedResolution;
        }

        // Apply the saved or default resolution immediately
        ApplyResolution(savedResolution);
    }

    public void Submit()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        // Check if a toggle is selected
        if (toggle != null)
        {
            // Save the selected toggle's name
            PlayerPrefs.SetString("SelectedResolution", toggle.name);

            // Apply the selected resolution
            ApplyResolution(toggle.name);
        }
    }

    private void ApplyResolution(string resolutionName)
    {
        switch (resolutionName)
        {
            case "ToggleHD":
                // Set resolution to 1920x1080 (HD)
                Screen.SetResolution(1920, 1080, true);
                break;

            case "Toggle2K":
                // Set resolution to 2560x1440 (2K)
                Screen.SetResolution(2560, 1440, true);
                break;

            case "Toggle4K":
                // Set resolution to 3840x2160 (4K)
                Screen.SetResolution(3840, 2160, true);
                break;

            default:
                // Default resolution if no match
                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }

    private void OnDisable()
    {
        // Load the previously submitted resolution or default
        string savedResolution = PlayerPrefs.GetString("SelectedResolution", defaultResolution);

        // Reapply the previously saved resolution
        ApplyResolution(savedResolution);

        // Update the active toggle to reflect the applied resolution
        UpdateActiveToggle(savedResolution);
    }

    private void UpdateActiveToggle(string resolutionName)
    {
        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = toggle.name == resolutionName;
        }
    }
}