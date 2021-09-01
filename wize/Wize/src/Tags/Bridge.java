package Tags;

public class Bridge extends TagReader {

	public Bridge(TagArgs args) {
		super(args);
	}
	/*
	 * @Override public void Process() { try { InputStream in =
	 * m_SerialPort.getInputStream(); while (IsRunning) {
	 * 
	 * try { byte[] buffer = new byte[1024]; int len = -1; if(in.available() > 0) {
	 * Thread.sleep(160); len = in.read(buffer); String data = new
	 * String(buffer,0,len); if(!data.contains("\n") ) { tag +=
	 * (data.getBytes()[0]); Tag(tag,false); continue; } else { tag +=
	 * data.replaceAll("(\\r|\\n)", ""); }
	 * 
	 * Log(LogType.Info, tag); Tag(tag,true);
	 * 
	 * tag = ""; } } catch ( IOException e ) { e.printStackTrace(); } catch (
	 * Exception e ) { e.printStackTrace(); }
	 * 
	 * 
	 * 
	 * } in.close(); } catch (Exception ex) { Log(LogType.Error, ex.getMessage()); }
	 * }
	 */
}
