using UnityEngine;

[CreateAssetMenu(menuName = "Ability Object Data")]
public class PlantAttributeData : ScriptableObject
{
    [TooltipAttribute("育成段階が最大になるまでの時間 : \n 10fだと10秒まで待つと最大になる")]
    public float MaxGrowthDuration;
    [TooltipAttribute("野菜の種類 : \n Daikonだと大根のアビリティのデータを作ることになる")]
    public VegetableType VegetableType;
    [Header("成長の設定項目")]
    [TooltipAttribute("第一段階のパーセント : \n [MaxGrowthDuration]の[SeedingPercentage]%が第一段階になる時間 \n 0.3fと入れると30%の時間で第一段階へ移行する")]
    public float SeedingPercentage;
    [TooltipAttribute("第二段階のパーセント : \n [MaxGrowthDuration]の[SproutPercentage]%が第二段階になる時間 \n 0.8fと入れると80%の時間で第二段階へ移行する")]
    public float SproutPercentage;
    
    [Header("強さの設定項目")]
    [TooltipAttribute("第一段階の野菜の攻撃力")] 
    public int SeedingDamage;
    [TooltipAttribute("第ニ段階の野菜の攻撃力")] 
    public int SproutDamage;
    [TooltipAttribute("第三段階の野菜の攻撃力")] 
    public int MatureDamage;

    [Header("[Corn Only]大きさの設定項目")]
    [TooltipAttribute("第一段階の野菜の大きさ")]
    public float SeedingScale = 1f;
    [TooltipAttribute("第ニ段階の野菜の大きさ")]
    public float SproutScale = 1f;
    [TooltipAttribute("第三段階の野菜の大きさ")]
    public float MatureScale = 1f;
    
    [Header("[Daikon Only]振る回数")]
    [TooltipAttribute("第一段階で振れる回数")]
    public int SeedingSwingCount;
    [TooltipAttribute("第ニ段階で振れる回数")]
    public int SproutSwingCount;
    [TooltipAttribute("第三段階で振れる回数")]
    public int MatureSwingCount;
}