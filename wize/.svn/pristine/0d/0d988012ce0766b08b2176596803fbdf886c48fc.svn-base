package Wize.Requests;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import Wize.Responses.IResponse;
import Utilities.Json;

public class HTTPSend implements ISend {

    private String _url;
    private IResponse _response = null;

    public HTTPSend(String url, IResponse response) {
        _url = url;
        _response = response;
    }

    @Override
    public void Send(RequestArgs args) {
        try {
            try {
                String urlParameters = "request=" + new Json<RequestArgs>().toJson(args);

                URL obj = new URL(_url);
                HttpURLConnection con = (HttpURLConnection) obj.openConnection();

                con.setRequestMethod("POST");
                con.setRequestProperty("User-Agent", "Mozilla/5.0");
                con.setRequestProperty("Accept-Language", "en-US,en;q=0.5");
                con.setDoOutput(true);
                DataOutputStream wr = new DataOutputStream(con.getOutputStream());
                wr.writeBytes(urlParameters);
                wr.flush();
                wr.close();

                int responseCode = con.getResponseCode();
                System.out.println("\nSending 'POST' request to URL : " + _url);
                System.out.println("Response Code : " + responseCode);

                BufferedReader in = new BufferedReader(new InputStreamReader(con.getInputStream()));
                String inputLine;
                StringBuffer response = new StringBuffer();

                while ((inputLine = in.readLine()) != null) {
                    response.append(inputLine);
                }
                in.close();

                if (_response != null)
                    _response.Response(response.toString());
            } catch (Exception ex) {
                ex.printStackTrace();
                if (_response != null)
                    _response.Response(ex.getMessage());
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

}