
public class PasswordChecker
{
    public static void Main()
    {

    }
    public static bool CheckPassword(string password)
    {
        if(password.Length >= 6 && password.Length <= 10)
        {
            return true;
        }
        return false;
    }
}