public class PasswortTest {
    public static void main(String[] args) {
        System.out.println("Hello world!");
    }

    public static boolean passwordTest(String password) {
        if (password.length() >= 5 && password.length() <= 10) {
            return true;
        }
        return false;
    }
}