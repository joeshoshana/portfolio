package Wize.Configurations;

import javax.xml.parsers.*;
import org.w3c.dom.*;

import Utilities.Parsers;
import Utilities.UtilsXML;

public class DisplayConfiguration {
    private String _type;
    private boolean _isDisplay = false;
    private boolean _isShowUserInput = false;

    public DisplayConfiguration() {
    }

    public void LoadConfig(String xmlPath) throws Exception {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);
            NodeList nl = dom.getElementsByTagName("Display");
            if (nl.getLength() > 0 && nl.item(0).getNodeType() == Node.ELEMENT_NODE) {
                Element n = (Element) nl.item(0);
                _type = UtilsXML.getTextValue(n, "Type");
                _isShowUserInput = Parsers.TryParseBool(UtilsXML.getTextValue(n, "IsShowUserInput"), false);
                _isDisplay = Parsers.TryParseBool(UtilsXML.getTextValue(n, "IsDisplay"), false);
            }
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String Type() {
        return _type;
    }

    public Boolean IsDisplay() {
        return _isDisplay;
    }

    public Boolean IsShowUserInput() {
        return _isShowUserInput;
    }
}