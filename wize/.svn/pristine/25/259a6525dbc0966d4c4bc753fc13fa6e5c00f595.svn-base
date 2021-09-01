package Utilities;

import com.google.gson.Gson;

public class Json<T> {
    public String toJson(T type) {
        Gson gson = new Gson();
        return gson.toJson(type);
    }

    public T fromJson(String json, Class<T> type) {
        Gson gson = new Gson();
        return gson.fromJson(json, type);
    }
}