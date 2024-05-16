using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image image;

    // Start is called before the first frame update
    public void setHealth(int health)
    {
        slider.value = health;
        image.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        image.color = gradient.Evaluate(1f);
    }
}
