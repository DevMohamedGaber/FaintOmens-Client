using UnityEngine;
public class ColorHelper {
    public static Color Compare(uint amount, uint required) => amount >= required ? Color.white : Color.red;
}