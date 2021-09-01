package UnitTests;

import java.io.IOException;

import org.junit.jupiter.api.Test;

import Offline.OfflineHandler;
import Wize.Request;

class OfflineHandlerTest {

	@Test
	void saveRecords() {
		OfflineHandler oh = new OfflineHandler(null);
		Request req = new Request();
		req.Command("Test");
		req.Mac("123456");
		req.Tag("1111");
		req.Weight("1250.36");
		oh.Save(req.toJson());
		try {
			oh.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	@Test
	void loadRecords() {
		/*
		 * OfflineHandler oh = new OfflineHandler(); ArrayList<String> records =
		 * oh.Load();
		 */
	}

}
