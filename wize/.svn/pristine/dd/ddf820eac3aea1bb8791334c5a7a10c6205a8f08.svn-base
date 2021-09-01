package Offline;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Timer;

public class OfflineHandler implements IOffline {
	public int OfflineInterval = 0;
	private static String m_activeDirectory = System.getProperty("user.dir").replace("\\", "/");
	private static String m_recordsFile = m_activeDirectory + "/record.txt";
	private Timer m_timer = null;
	private OfflineArgs m_args = null;
	private boolean m_isUpload = false;
	private boolean m_isRunning = false;

	public OfflineHandler(OfflineArgs args) {
		m_args = args;
	}

	public void Save(ArrayList<String> records) {
		for (int i = 0; i < records.size(); i++)
			Save(records.get(i));
	}

	public void Save(String record) {
		try (BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(m_recordsFile, true))) {
			bufferedWriter.write(record + "\n");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public ArrayList<String> Load() {
		ArrayList<String> records = new ArrayList<>();
		File temp = new File(m_recordsFile);

		if (temp.exists()) {
			try (BufferedReader bufferedReader = new BufferedReader(new FileReader(m_recordsFile))) {
				String line = bufferedReader.readLine();
				while (line != null) {
					records.add(line);
					line = bufferedReader.readLine();
				}
			} catch (FileNotFoundException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
		return records;
	}

	public void Run(boolean isRun) {
		m_isRunning = isRun;
		if (m_isRunning)
			run();
		else
			Disconnect();
	}

	public void Delete() {
		try {
			File f = new File(m_recordsFile);
			if (f.exists())
				f.delete();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	@Override
	public void run() {
		if (m_args.Interval > 0 && m_args.Task != null) {
			m_timer = new Timer("Timer");

			long delay = 30000L;
			long period = m_args.Interval * 60 * 1000;
			m_timer.scheduleAtFixedRate(m_args.Task, delay, period);
		}

	}

	@Override
	public void close() throws IOException {
		Disconnect();
	}

	public void Disconnect() {
		if (m_timer != null)
			m_timer.cancel();
		m_timer = null;
	}

	@Override
	public void SetIsUpload(boolean isUpload) {
		m_isUpload = isUpload;

	}

	@Override
	public boolean IsUpload() {
		return m_isUpload;
	}
}
