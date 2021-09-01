package Wize.Requests;

public class Request {
    protected ISend _send = null;
    protected RequestArgs _args = null;

    public Request(ISend send, RequestArgs args) {
        _send = send;
        _args = args;
    }

    public void Send() {
        _send.Send(_args);
    }
}