using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Collections.Specialized;

namespace Service_Windows
{
    public class Request
    {
        public String Command;
        public String MAC;
        public String Weight;

        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

        public void FromJson(string json)
        {
            if (json == null)
                return;

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType());
            Request obj = (Request)serializer.ReadObject(ms);
            ms.Close();
            this.Command = obj.Command;
            this.MAC = obj.MAC;
            this.Weight = obj.Weight;
        }

        public void Send(string url, Action<String> Callback)
	    {
	    	try
	    	{
                String request =  this.ToJson();

                try
                {
                    string method = "POST";
                    string UserAgent = "Mozilla/5.0";
                    string AcceptLanguage = "en-US,en;q=0.5";
                    string ContentType = "application/json; charset=utf-8";
                    using(WebClient client = new WebClient())
                    {
                        var parameters = new NameValueCollection();
                        parameters.Add("request", request);
                        var response_data = client.UploadValues(url, method, parameters);
                        var responseString = UnicodeEncoding.UTF8.GetString(response_data);
                        Callback(responseString);
                    }

                  /*  HttpWebRequest.DefaultMaximumErrorResponseLength = 200000;
                    HttpWebRequest client = (HttpWebRequest)WebRequest.Create(url);
                    client.Method = method;
                    client.Timeout = 1000 * 60 * 10; //10 minutes
                    client.KeepAlive = true;
                    client.AllowWriteStreamBuffering = true;
                    client.ContentType = ContentType;
                    client.UserAgent = UserAgent;
                    client.Accept = AcceptLanguage;
                    using (var streamWriter = new StreamWriter(client.GetRequestStream()))
                    {
                        streamWriter.Write(request);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    Stream ms = client.GetResponse().GetResponseStream();
                    string reply;
                    using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        string a = reader.ReadToEnd();
                        if(!string.IsNullOrEmpty(a))
                            Callback(a);
                    }*/

                }
                catch (WebException wex)
                {
                    if (wex.Message != null)
                    {
                        Callback(wex.Message);
                    }

                    if (wex.InnerException != null)
                    {
                    }
                    if (wex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)wex.Response)
                        {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                string dt = reader.ReadToEnd();
                                Callback(dt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Callback(ex.Message);
                }

	    		/*URL obj = new URL(url);
	    		HttpURLConnection con = (HttpURLConnection) obj.openConnection();

	    		//add reuqest header
	    		con.setRequestMethod("POST");
	    		con.setRequestProperty("User-Agent", "Mozilla/5.0");
	    		con.setRequestProperty("Accept-Language", "en-US,en;q=0.5");
	    		//con.setRequestProperty("Content-Type", "application/json; charset=utf-8");
	    		//con.setRequestProperty("Content-Length", String.valueOf(this.toJson().length()));

	    		//String urlParameters = this.toJson();
	    		
	    		// Send post request
	    		con.setDoOutput(true);
	    		DataOutputStream wr = new DataOutputStream(con.getOutputStream());
	    		wr.writeBytes(urlParameters);
	    		wr.flush();
	    		wr.close();
	    		
	    		int responseCode = con.getResponseCode();
	    		System.out.println("\nSending 'POST' request to URL : " + url);
	    		System.out.println("Post parameters : " + urlParameters);
	    		System.out.println("Response Code : " + responseCode);

	    		BufferedReader in = new BufferedReader(
	    		        new InputStreamReader(con.getInputStream()));
	    		String inputLine;
	    		StringBuffer response = new StringBuffer();

	    		while ((inputLine = in.readLine()) != null) {
	    			response.append(inputLine);
	    		}
	    		in.close();
	    		
	    		//print result
	    		System.out.println(response.toString());*/
	    
	    	}
	    	catch(Exception ex)
	    	{
                throw ex;	
	    	}
	    	
	    }
    }
}
