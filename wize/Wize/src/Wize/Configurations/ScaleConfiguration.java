package Wize.Configurations;

import javax.xml.parsers.*;
import org.w3c.dom.*;

import Utilities.UtilsXML;

public class ScaleConfiguration {
    private String _type;
    private String _com;

    public ScaleConfiguration() {
    }

    public void LoadConfig(String xmlPath) throws Exception {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);
            NodeList nl = dom.getElementsByTagName("Scale");
            if (nl.getLength() > 0 && nl.item(0).getNodeType() == Node.ELEMENT_NODE) {
                Element n = (Element) nl.item(0);
                _type = UtilsXML.getTextValue(n, "Type");
                _com = UtilsXML.getTextValue(n, "Com");
            }

        } catch (Exception ex) {
            throw ex;
        }
    }

    public String Type() {
        return _type;
    }

    public String Com() {
        return _com;
    }

}