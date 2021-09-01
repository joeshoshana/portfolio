package Utilities;

public class Parsers {
    public static int TryParseInt(String value, int fallbackValue) {
        try {
            return Integer.parseInt(value);
        } catch (NumberFormatException e) {
            return fallbackValue;
        }
    }

    public static Boolean TryParseBool(String value, Boolean fallbackValue) {
        try {
            return Boolean.parseBoolean(value);
        } catch (Exception e) {
            return fallbackValue;
        }
    }
}