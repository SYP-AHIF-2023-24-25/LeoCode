import org.junit.Test;
import org.junit.jupiter.api.Assertions;

public class PasswortTestTests {
    @Test
    public void testPasswordLength() {
        boolean result = PasswortTest.passwordTest("1234567");
        Assertions.assertTrue(result);
    }
    @Test
    public void testPasswordLength_notOk() {
        boolean result = PasswortTest.passwordTest("1");
        Assertions.assertFalse(result);
    }
    @Test
    public void testPasswordLength_not_Ok() {
        boolean result = PasswortTest.passwordTest("12");
        Assertions.assertFalse(result);
    }
}
