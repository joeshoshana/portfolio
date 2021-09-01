package Wize.Configurations;

import javax.xml.parsers.*;
import org.w3c.dom.*;

import Utilities.Parsers;
import Utilities.UtilsXML;

public class ModuleConfiguration {
    private String _type;
    private String _web;
    private int _resetTime;
    private int _offlineInterval;
    private ControllerConfiguration _controllerConfig = null;
    private TagConfiguration _tagConfig = null;
    private DisplayConfiguration _displayConfig = null;
    private IOConfiguration _ioConfig = null;
    private LPRConfiguration _lprConfig = null;
    private ScaleConfiguration _scaleConfig = null;

    public ModuleConfiguration() {
        _controllerConfig = new ControllerConfiguration();
        _tagConfig = new TagConfiguration();
        _displayConfig = new DisplayConfiguration();
        _ioConfig = new IOConfiguration();
        _lprConfig = new LPRConfiguration();
        _scaleConfig = new ScaleConfiguration();
    }

    public void LoadModuleConfiguration(String xmlPath) throws Exception {
        try {
            Document dom;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            dom = db.parse(xmlPath);

            NodeList nl = dom.getElementsByTagName("Module");
            if (nl.getLength() > 0 && nl.item(0).getNodeType() == Node.ELEMENT_NODE) {
                Element n = (Element) nl.item(0);
                _type = UtilsXML.getTextValue(n, "Type");
                _web = UtilsXML.getTextValue(n, "Web");
                _resetTime = Parsers.TryParseInt(UtilsXML.getTextValue(n, "ResetTime"), 0);
                _offlineInterval = Parsers.TryParseInt(UtilsXML.getTextValue(n, "OfflineInterval"), 0);
            }

            _controllerConfig.LoadConfig(xmlPath);
            _tagConfig.LoadConfig(xmlPath);
            _displayConfig.LoadConfig(xmlPath);
            _ioConfig.LoadConfig(xmlPath);
            _lprConfig.LoadConfig(xmlPath);
            _scaleConfig.LoadConfig(xmlPath);

        } catch (Exception ex) {
            throw ex;
        }
    }

    public String Type() {
        return _type;
    }

    public String Web() {
        return _web;
    }

    public int ResetTime() {
        return _resetTime;
    }

    public int OfflineInterval() {
        return _offlineInterval;
    }

    public ControllerConfiguration ControllerConfig() {
        return _controllerConfig;
    }

    public TagConfiguration TagConfig() {
        return _tagConfig;
    }

    public DisplayConfiguration DisplayConfig() {
        return _displayConfig;
    }

    public IOConfiguration IOConfig() {
        return _ioConfig;
    }

    public LPRConfiguration LPRConfig() {
        return _lprConfig;
    }

    public ScaleConfiguration ScaleConfig() {
        return _scaleConfig;
    }

}