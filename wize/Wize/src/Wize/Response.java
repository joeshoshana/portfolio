package Wize;
import com.google.gson.Gson;

public class Response {

	public String msg;
	public Boolean isSucceded;
	public static Response  fromJson(String json)
	{
		try
		{
			Gson gson = new Gson();
			Response c = gson.fromJson(json,Response.class);
			return c;	
		}
		catch(Exception ex)
		{
			return null;
		}
		
	}
}
