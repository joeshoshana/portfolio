package Wize;

import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;

import javax.xml.parsers.*;
import javax.xml.transform.*;
import javax.xml.transform.dom.*;
import javax.xml.transform.stream.*;
import org.w3c.dom.*;

import IO.GPIO;
import IO.IOs;
import IO.State;
import IO.Transput;

public class Configuration {
    public static String Scale;
    public static String Web;
    public static String Com;
    public static String IP;
    public static String Port;
    public static String Error = "";
    public static String TagModule = "";
    public static String Display = "";
    public static String TagCom = "";
    // public static String SiemensActive = "";
    public static String SiemensIP = "";
    public static String SiemensPort = "";
    public static String LPRIP = "";
    public static String LPRPort = "";
    public static String Module = "";
    public static String ResetTime = "";
    public static String OfflineInterval = "";
    public static String IsShowUserInput = "";

    public static ArrayList<GPIO> GPIOs = null;

    public static boolean LoadConfiguration(String xmlPath) {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);
            Element doc = dom.getDocumentElement();

            Scale = getTextValue(doc, "Scale");
            Web = getTextValue(doc, "Web");
            Com = getTextValue(doc, "Com");
            IP = getTextValue(doc, "IP");
            Port = getTextValue(doc, "Port");
            TagModule = getTextValue(doc, "TagModule");
            Display = getTextValue(doc, "Display");
            TagCom = getTextValue(doc, "TagCom");
            // SiemensActive = getTextValue( doc, "SiemensActive");
            SiemensIP = getTextValue(doc, "SiemensIP");
            SiemensPort = getTextValue(doc, "SiemensPort");
            LPRIP = getTextValue(doc, "LPRIP");
            LPRPort = getTextValue(doc, "LPRPort");
            Module = getTextValue(doc, "Module");
            ResetTime = getTextValue(doc, "ResetTime");
            OfflineInterval = getTextValue(doc, "OfflineInterval");
            IsShowUserInput = getTextValue(doc, "IsShowUserInput");
            GPIOs = getGpios(doc, "GPIO");

            return true;
        } catch (Exception ex) {
            Error = ex.getMessage();
            return false;
        }
    }

    public static boolean SaveConfiguration(String xmlPath) {
        Document dom;
        Element e = null;

        DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
        try {
            DocumentBuilder db = dbf.newDocumentBuilder();

            dom = db.newDocument();

            Element rootEle = dom.createElement("Configuration");

            e = dom.createElement("Scale");
            e.appendChild(dom.createTextNode(Scale));
            rootEle.appendChild(e);

            e = dom.createElement("Web");
            e.appendChild(dom.createTextNode(Web));
            rootEle.appendChild(e);

            e = dom.createElement("Com");
            e.appendChild(dom.createTextNode(Com));
            rootEle.appendChild(e);

            e = dom.createElement("IP");
            e.appendChild(dom.createTextNode(IP));
            rootEle.appendChild(e);

            e = dom.createElement("Port");
            e.appendChild(dom.createTextNode(Port));
            rootEle.appendChild(e);

            e = dom.createElement("TagModule");
            e.appendChild(dom.createTextNode(TagModule));
            rootEle.appendChild(e);

            e = dom.createElement("Display");
            e.appendChild(dom.createTextNode(Display));
            rootEle.appendChild(e);

            e = dom.createElement("TagCom");
            e.appendChild(dom.createTextNode(TagCom));
            rootEle.appendChild(e);

            /*
             * e = dom.createElement("SiemensActive");
             * e.appendChild(dom.createTextNode(SiemensActive)); rootEle.appendChild(e);
             */

            e = dom.createElement("SiemensIP");
            e.appendChild(dom.createTextNode(SiemensIP));
            rootEle.appendChild(e);

            e = dom.createElement("SiemensPort");
            e.appendChild(dom.createTextNode(SiemensPort));
            rootEle.appendChild(e);

            e = dom.createElement("LPRIP");
            e.appendChild(dom.createTextNode(LPRIP));
            rootEle.appendChild(e);

            e = dom.createElement("LPRPort");
            e.appendChild(dom.createTextNode(LPRPort));
            rootEle.appendChild(e);

            e = dom.createElement("Module");
            e.appendChild(dom.createTextNode(Module));
            rootEle.appendChild(e);

            e = dom.createElement("ResetTime");
            e.appendChild(dom.createTextNode(ResetTime));
            rootEle.appendChild(e);

            e = dom.createElement("OfflineInterval");
            e.appendChild(dom.createTextNode(OfflineInterval));
            rootEle.appendChild(e);

            dom.appendChild(rootEle);
            try {
                Transformer tr = TransformerFactory.newInstance().newTransformer();

                tr.transform(new DOMSource(dom), new StreamResult(new FileOutputStream(xmlPath)));

            } catch (TransformerException te) {
                Error = te.getMessage();
                return false;
            } catch (IOException ioe) {
                Error = ioe.getMessage();
                return false;
            }
        } catch (ParserConfigurationException pce) {
            Error = pce.getMessage();
            return false;
        }
        return true;
    }

    private static ArrayList<GPIO> getGpios(Element doc, String tag) {
        NodeList nl;
        nl = doc.getElementsByTagName(tag);
        if (nl.getLength() == 0)
            return null;

        ArrayList<GPIO> gpios = new ArrayList<>();
        for (int i = 0; i < nl.getLength(); i++) {
            GPIO gpio = new GPIO();
            if (nl.item(i).getNodeType() == Node.ELEMENT_NODE) {
                String dt = getTextValue((Element) nl.item(i), "IO");
                if (dt.length() == 0)
                    dt = "None";
                gpio.IO = IOs.valueOf(dt);
                dt = getTextValue((Element) nl.item(i), "State");
                if (dt.length() == 0)
                    dt = "None";
                gpio.State = State.valueOf(dt);
                dt = getTextValue((Element) nl.item(i), "Transput");
                if (dt.length() == 0)
                    dt = "None";
                gpio.Transput = Transput.valueOf(dt);
            }

            gpios.add(gpio);
        }

        return gpios;
    }

    private static String getTextValue(Element doc, String tag) {
        String value = "";
        NodeList nl;
        nl = doc.getElementsByTagName(tag);
        if (nl.getLength() > 0 && nl.item(0).hasChildNodes()) {
            value = nl.item(0).getFirstChild().getNodeValue();
        }
        return value;
    }
}
