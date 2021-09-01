package Wize.Configurations;

import javax.xml.parsers.*;
import org.w3c.dom.*;
import java.util.ArrayList;

import IO.GPIO;
import IO.IOs;
import IO.State;
import IO.Transput;
import Utilities.UtilsXML;

public class IOConfiguration {
    private String _type;
    private static ArrayList<GPIO> _gpioList = new ArrayList<GPIO>();

    public IOConfiguration() {
    }

    public void LoadConfig(String xmlPath) throws Exception {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);
            NodeList nl = dom.getElementsByTagName("IO");
            if (nl.getLength() > 0 && nl.item(0).getNodeType() == Node.ELEMENT_NODE) {
                Element n = (Element) nl.item(0);
                _type = UtilsXML.getTextValue(n, "Type");
                getGpios(n, "GPIO");
            }
        } catch (Exception ex) {
            throw ex;
        }
    }

    private void getGpios(Element doc, String tag) {
        NodeList nl;
        nl = doc.getElementsByTagName(tag);
        if (nl.getLength() == 0)
            return;

        for (int i = 0; i < nl.getLength(); i++) {
            GPIO gpio = new GPIO();
            if (nl.item(i).getNodeType() == Node.ELEMENT_NODE) {
                String dt = UtilsXML.getTextValue((Element) nl.item(i), "IO");
                if (dt.length() == 0)
                    dt = "None";
                gpio.IO = IOs.valueOf(dt);
                dt = UtilsXML.getTextValue((Element) nl.item(i), "State");
                if (dt.length() == 0)
                    dt = "None";
                gpio.State = State.valueOf(dt);
                dt = UtilsXML.getTextValue((Element) nl.item(i), "Transput");
                if (dt.length() == 0)
                    dt = "None";
                gpio.Transput = Transput.valueOf(dt);
            }

            _gpioList.add(gpio);
        }
    }

    public void AddIO(GPIO io) {
        _gpioList.add(io);
    }

    public String Type() {
        return _type;
    }

    public ArrayList<GPIO> IO() {
        return _gpioList;
    }
}