package org.example;

/**
 * Hello world!
 *
 */
public class App 
{
    public static void main( String[] args )
    {
        System.out.println( "Hello World!" );
    }

    public int CalculateParkingFee(int parkingHours, int pricePerHour) {
    	return parkingHours * pricePerHour;
    }
}
