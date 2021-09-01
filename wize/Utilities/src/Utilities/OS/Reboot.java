package Utilities.OS;

public class Reboot {
    public static void Start() {
        try {
            if (OSValidator.isWindows())
                rebootWindows();
            else if (OSValidator.isUnix())
                rebootRaspberry();
            else
                return;
        } catch (Exception ex) {
            ex.printStackTrace();
            return;
        }
    }

    private static void rebootRaspberry() {
        try {
            String command = "reboot";

            Runtime.getRuntime().exec(command);
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

    private static void rebootWindows() {
        try {
            String command = "shutdown /r";

            Runtime.getRuntime().exec(command);
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}