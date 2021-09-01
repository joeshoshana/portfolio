package UnitTests;

import java.awt.Color;

import org.junit.jupiter.api.Test;

import Wize.WeightDisplay;

class DisplayTest {

	@Test
	void Display_BadMessage() {
		WeightDisplay wd = new WeightDisplay();
		wd.setVisible(true);
		wd.ChangeBackground(Color.getHSBColor(Color.RGBtoHSB(37, 187, 70, null)[0],
				Color.RGBtoHSB(37, 187, 70, null)[1], Color.RGBtoHSB(37, 187, 70, null)[2]));
		wd.setMessage("good", true);
		while (true)
			;
	}

}
