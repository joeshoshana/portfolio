package Wize.Configurations;

import javax.xml.parsers.*;
import org.w3c.dom.*;

import Utilities.Parsers;
import Utilities.UtilsXML;

public class LPRConfiguration {
    private String _type;
    private String _ip;
    private int _port;

    public LPRConfiguration() {
    }

    public void LoadConfig(String xmlPath) throws Exception {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);
            NodeList nl = dom.getElementsByTagName("LPR");
            if (nl.getLength() > 0 && nl.item(0).getNodeType() == Node.ELEMENT_NODE) {
                Element n = (Element) nl.item(0);
                _type = UtilsXML.getTextValue(n, "Type");
                _ip = UtilsXML.getTextValue(n, "IP");
                _port = Parsers.TryParseInt(UtilsXML.getTextValue(n, "Port"), 0);
            }
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String Type() {
        return _type;
    }

    public String IP() {
        return _ip;
    }

    public int Port() {
        return _port;
    }
}