package org.example;

import junit.framework.Test;
import junit.framework.TestCase;
import junit.framework.TestSuite;

/**
 * Unit test for simple App.
 */
public class AppTest 
    extends TestCase
{
    /**
     * Create the test case
     *
     * @param testName name of the test case
     */
    public AppTest( String testName )
    {
        super( testName );
    }

    /**
     * @return the suite of tests being tested
     */
    public static Test suite()
    {
        return new TestSuite( AppTest.class );
    }

    /**
     * Rigourous Test :-)
     */
    public void testApp()
    {
        assertTrue( true );
    }

    public void testCalculateParkingFee() {
        	App app = new App();
        	int parkingHours = 2;
        	int pricePerHour = 10;
        	int expected = 20;
        	int actual = app.CalculateParkingFee(parkingHours, pricePerHour);
        	assertEquals(expected, actual);
    }

    public void testCalculateParkingFee_True() {
        	App app = new App();
        	int parkingHours = 2;
        	int pricePerHour = 20;
        	int expected = 40;
        	int actual = app.CalculateParkingFee(parkingHours, pricePerHour);
        	assertEquals(expected, actual);
    }
}
