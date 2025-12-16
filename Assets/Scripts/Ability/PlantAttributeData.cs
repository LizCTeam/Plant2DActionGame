using UnityEngine;

[CreateAssetMenu(menuName = "Ability Object Data")]
public class PlantAttributeData : ScriptableObject
{
    [TooltipAttribute("育成段階が最大になるまでの時間 : \n 10fだと10秒まで待つと最大になる")]
    public float MaxGrowthDuration;
    [TooltipAttribute("野菜の種類 : \n Daikonだと大根のアビリティのデータを作ることになる")]
    public VegetableType VegetableType;
    [TooltipAttribute("第一段階のパーセント : \n [MaxGrowthDuration]の[SeedingPercentage]%が第一段階になる時間 \n 0.3fと入れると30%の時間で第一段階へ移行する")]
    public float SeedingPercentage;
    [TooltipAttribute("第二段階のパーセント : \n [MaxGrowthDuration]の[SproutPercentage]%が第二段階になる時間 \n 0.8fと入れると80%の時間で第二段階へ移行する")]
    public float SproutPercentage;
    [TooltipAttribute("第一段階の野菜の攻撃力")] 
    public int SeedingDamage;
    [TooltipAttribute("第ニ段階の野菜の攻撃力")] 
    public int SproutDamage;
    [TooltipAttribute("第三段階の野菜の攻撃力")] 
    public int MatureDamage;
}