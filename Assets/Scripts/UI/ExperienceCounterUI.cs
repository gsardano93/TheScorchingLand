
using TMPro;
using UnityEngine;

public class ExperienceCounterUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textBox;
    private int expStorage;
    private int expVisualReference=0;
    private float timer;
    private float timerMax = 0.01f;
    void Start()
    {
        textBox.text = expVisualReference.ToString();
        Player.Instance.OnExpChanged += Player_OnExpChanged;
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        Player.Instance.OnExpChanged -= Player_OnExpChanged;
    }


    void Update()
    {
        if (expStorage > 0)
            AddExp();
        if (expStorage < 0)
            SubtractExp();
    }
    private void AddExp()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            expStorage--;
            expVisualReference++;
            textBox.text = expVisualReference.ToString();
            timer = timerMax;
        }
    }
    private void SubtractExp()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            expStorage++;
            expVisualReference--;
            textBox.text = expVisualReference.ToString();
            timer = timerMax;
        }
    }

    private void Player_OnExpChanged(object sender, Player.OnExpChangedEventArgs e)
    {
        expStorage += e.amountChanged;
    }
    public int GetExperienceVisual(){
        return expVisualReference;
    }
    public void RefreshTextBox(int i){
        expVisualReference = i;
        textBox.text = i.ToString();
    }
}
