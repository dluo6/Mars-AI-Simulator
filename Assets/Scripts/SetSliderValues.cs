using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetSliderValues : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TMP_InputField curText;


    public void Start() {
        minText.text = ((int)slider.minValue).ToString();
        maxText.text = ((int)slider.maxValue).ToString();
        curText.text = ((int)slider.value).ToString();

        slider.onValueChanged.AddListener(delegate{updateValue();});
        curText.onEndEdit.AddListener(delegate{updateSlider();});
    }

    public void updateValue() {
        curText.text = ((int)slider.value).ToString();
    }

    public void updateSlider() {
        slider.value = float.Parse(curText.text);
    }
}
