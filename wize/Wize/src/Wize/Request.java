package Wize;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import com.google.gson.Gson;

public class Request {
	private String _command;
	private String _mac;
	private String _weight;
	private String _tag;
	private byte[] _pic;
	private String _weighingTime;
	public ResponseListener OnResponse = null;

	public Validation getStatus() {
		Validation myValidation = new Validation();
		Boolean isValid = true;
		String message = "";

		message += CheckParameter(_command, "Command");
		message += CheckParameter(_mac, "MAC");
		message += CheckParameter(_weight, "Weight");
		message += CheckParameter(_tag, "Tag");

		// if (_weighingTime.getYear() < 1970) {
		// isValid = false;
		// message += "Weighing clock is not correct. ";
		// }

		isValid = message.length()==0 ? true : false;

		myValidation.IsValid(isValid);
		myValidation.Message(message);

		return myValidation;
	}

	private String CheckParameter(String myParameter, String parameterName) {
		String result = "";
		if (myParameter.length() == 0) {
			result = parameterName +" is empty. ";
		}
		return result;
	}

	public String toJson() {
		Gson gson = new Gson();
		return gson.toJson(this);
	}

	public void fromJson(String json) {
		Gson gson = new Gson();
		Request c = gson.fromJson(json, Request.class);
		this._command = c._command;
		this._mac = c._mac;
		this._weight = c._weight;
		this._tag = c._tag;
		this._pic = c._pic;
	}

	public void send(String url) {
		try {
			/* Thread t = new Thread(() -> { */
			try {
				String urlParameters = "request=" + this.toJson();

				URL obj = new URL(url);
				HttpURLConnection con = (HttpURLConnection) obj.openConnection();

				// add reuqest header
				con.setRequestMethod("POST");
				con.setRequestProperty("User-Agent", "Mozilla/5.0");
				con.setRequestProperty("Accept-Language", "en-US,en;q=0.5");
				// con.setRequestProperty("Content-Type", "application/json; charset=utf-8");
				// con.setRequestProperty("Content-Length",
				// String.valueOf(this.toJson().length()));

				// String urlParameters = this.toJson();

				// Send post request
				con.setDoOutput(true);
				DataOutputStream wr = new DataOutputStream(con.getOutputStream());
				wr.writeBytes(urlParameters);
				wr.flush();
				wr.close();

				int responseCode = con.getResponseCode();
				System.out.println("\nSending 'POST' request to URL : " + url);
				// System.out.println("Post parameters : " + urlParameters);
				System.out.println("Response Code : " + responseCode);

				BufferedReader in = new BufferedReader(new InputStreamReader(con.getInputStream()));
				String inputLine;
				StringBuffer response = new StringBuffer();

				while ((inputLine = in.readLine()) != null) {
					response.append(inputLine);
				}
				in.close();

				Response(response.toString());
			} catch (Exception ex) {
				Response(ex.getMessage());
			}
		} catch (Exception ex) {
			System.out.println(ex.getMessage());
		}
	}

	private void Response(String data) {
		if (OnResponse != null)
			OnResponse.Reponse(data);
	}

	public String Command() {
		return this._command;
	}

	public void Command(String command) {
		this._command = command;
	}

	public String Mac() {
		return this._mac;
	}

	public void Mac(String mac) {
		this._mac = mac;
	}

	public String Weight() {
		return this._weight;
	}

	public void Weight(String weight) {
		this._weight = weight;
	}

	public String Tag() {
		return this._tag;
	}

	public void Tag(String tag) {
		this._tag = tag;
	}

	public byte[] Pic() {
		return this._pic;
	}

	public void Pic(byte[] arrayList) {
		this._pic = arrayList;
	}

	public String WeighingTime() {
		return this._weighingTime;
	}

	public void WeighingTime(String weighingTime) {
		this._weighingTime = weighingTime;
	}
}
