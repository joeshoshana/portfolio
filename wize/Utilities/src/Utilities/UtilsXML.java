package Utilities;

import java.io.File;
import java.io.StringWriter;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NodeList;

public class UtilsXML {

	private static Document doc;

	// Root "ORDER"
	public static Element addRoot(String rootName) {
		Element root = doc.createElement(rootName);
		doc.appendChild(root);
		return root;
	}

	// Elements inside ROOT like INFO, OTHERS
	public static Element addElement(String rootName, Element root) {
		Element e = doc.createElement(rootName);
		root.appendChild(e);
		return e;
	}

	// Elements inside other elements INFO -> Id, Value
	public static void addNode(Element e, String tagName, String content) {
		Element employ = doc.createElement(tagName);
		employ.appendChild(doc.createTextNode(content));
		e.appendChild(employ);
	}

	// Document
	public static void makeDoc() throws ParserConfigurationException {
		DocumentBuilderFactory dF = DocumentBuilderFactory.newInstance();
		DocumentBuilder docB = dF.newDocumentBuilder();
		doc = docB.newDocument();
	}

	// Save the xml file
	public static void makeFile(String filePath) throws TransformerException {
		TransformerFactory tF = TransformerFactory.newInstance();
		Transformer t = tF.newTransformer();
		DOMSource source = new DOMSource(doc);
		File xml = new File(filePath);
		StreamResult r = new StreamResult(xml);
		t.transform(source, r);
	}

	public static String getTextValue(Element doc, String tag) {
		String value = "";
		NodeList nl;
		nl = doc.getElementsByTagName(tag);
		if (nl.getLength() > 0 && nl.item(0).hasChildNodes()) {
			value = nl.item(0).getFirstChild().getNodeValue();
		}
		return value;
	}

	public static String getXML() {
		DOMSource domSource = new DOMSource(doc);
		StringWriter writer = new StringWriter();
		StreamResult result = new StreamResult(writer);
		TransformerFactory tf = TransformerFactory.newInstance();
		Transformer transformer;
		try {
			transformer = tf.newTransformer();
			transformer.transform(domSource, result);
		} catch (TransformerException e) {
			e.printStackTrace();
		}

		return writer.toString();
	}
}