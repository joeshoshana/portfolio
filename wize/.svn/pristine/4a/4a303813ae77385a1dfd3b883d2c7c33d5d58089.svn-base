package Utilities.OS;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class Mac {
    public static String Get() {
        try {
            if (OSValidator.isWindows())
                return getWindowsMac();
            else if (OSValidator.isUnix())
                return getMacRaspberry();
            else
                return null;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }
    }

    private static String getWindowsMac() {
        try {
            String command = "wmic csproduct get UUID";

            Process proc = Runtime.getRuntime().exec(command);

            BufferedReader reader = new BufferedReader(new InputStreamReader(proc.getInputStream()));

            String mac = "";
            String line = "";
            while ((line = reader.readLine()) != null) {
                if (line.toLowerCase().contains("uuid")) {
                    line = reader.readLine();
                    line = reader.readLine();
                    mac = line.replaceAll("\\s+", "");
                    System.out.print(mac + "\n");
                }
            }

            proc.waitFor();

            System.out.println(mac);
            return mac;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }

    }

    private static String getMacRaspberry() {
        try {
            String command = "cat /proc/cpuinfo";

            Process proc = Runtime.getRuntime().exec(command);

            // Read the output

            BufferedReader reader = new BufferedReader(new InputStreamReader(proc.getInputStream()));

            String mac = "";
            String line = "";
            while ((line = reader.readLine()) != null) {
                if (line.toLowerCase().contains("serial")) {
                    mac = line.split(":")[1].trim();
                    System.out.print(mac + "\n");
                }
            }

            proc.waitFor();

            System.out.println(mac);
            return mac;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }

    }
}