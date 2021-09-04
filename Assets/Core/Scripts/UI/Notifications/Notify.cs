namespace Game
{
    public class Notify
    {
        public static UI.UINotifications list => UI.UINotifications.list;
        public static void SomethingWentWrong() => list.Add("Something went wrong, please try again", "حدث خطأ ما, برجاء المحاولة مرة اخري");
        public static void AlreadyMarried() => list.Add("You are already married", "انت متزوح بالفعل");
        public static void AlreadyFriends() => list.Add("You are already friends", "انت متصادقون بالفعل");
        public static void DontHaveEnoughGold() => list.Add("You don't have enough gold", "ليس لديك ذهب كاف");
        public static void DontHaveEnoughDiamonds() => list.Add("You don't have enough diamonds", "ليس لديك الماس كاف");
        public static void DontHaveEnoughBDiamonds() => list.Add("You don't have enough bound diamonds", "ليس لديك الماس مجاني كاف");
        public static void InvalidName() => list.Add("Invalid Name", "الاسم غير متوافق");
        public static void SelectItemFirst() => list.Add("Select an item first", "اختر ايتم اولا");
    }
}