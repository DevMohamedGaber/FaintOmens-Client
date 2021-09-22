namespace Game
{
    public class Notify
    {
        public static UI.Notifications list => UI.Notifications.list;
        public static void SomethingWentWrong() => list.Add("Something went wrong, please try again", "حدث خطأ ما, برجاء المحاولة مرة اخري");
        public static void AlreadyMarried() => list.Add("You are already married", "انت متزوح بالفعل");
        public static void AlreadyFriends() => list.Add("You are already friends", "انت متصادقون بالفعل");
        public static void TargetNotFriend() => list.Add("Target Not a Friend", "الهدف ليس صديق");
        public static void TargetOffline() => list.Add("Target is offline", "الهدف غير متصل");
        public static void DontHaveEnoughGold() => list.Add("You don't have enough gold", "ليس لديك ذهب كاف");
        public static void DontHaveEnoughDiamonds() => list.Add("You don't have enough diamonds", "ليس لديك الماس كاف");
        public static void DontHaveEnoughBDiamonds() => list.Add("You don't have enough bound diamonds", "ليس لديك الماس مجاني كاف");
        public static void InvalidName() => list.Add("Invalid Name", "الاسم غير متوافق");
        public static void SelectItemFirst() => list.Add("Select an item first", "اختر ايتم اولا");
        public static void InvalidTargetId() => list.Add("Invalid target id", "الرقم التعريفي للهدف غير صحيح");
        public static void NotInGuild() => list.Add("You aren't in a guild", "انت لست مشترك في نقابة");
        public static void NotInTeam() => list.Add("You're not in team, create or join one first", "لست مشترك بفريق, انشا او اشترك بفريق");
    }
}