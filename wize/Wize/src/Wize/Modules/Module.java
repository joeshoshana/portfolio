package Wize.Modules;

import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

import IO.IO;
import LPR.LPR;
import Offline.Offline;
import Wize.Responses.IResponse;
import Wize.Responses.ResponseArgs;
import Scales.Scale;
import Siemens.Controller;
import Tags.Tag;
import Utilities.Json;
import Utilities.Network;
import Utilities.OS.Mac;
import Utilities.OS.Reboot;
import Wize.Configurations.ModuleConfiguration;
import Displays.Display;

public class Module {
    protected Controller _controller = null;
    protected Scale _scale = null;
    protected Tag _tag = null;
    protected LPR _lpr = null;
    protected Offline _offline = null;
    protected IO _io = null;
    protected Display _display = null;
    protected String _mac = null;
    protected boolean _isRunning = false;
    protected ModuleConfiguration _config = null;
    protected IResponse _response = null;

    protected TimerTask _rebootTask = new TimerTask() {
        public void run() {
            if (!Network.IsPing()) {
                Reboot.Start();
            }
        }
    };

    public Module(ModuleConfiguration config) {
        _mac = Mac.Get();
        _config = config;
        if (_config.ResetTime() > 0)
            setRebootTimer();
    }

    protected void setRebootTimer() {
        Timer timer = new Timer("Timer");

        long delay = 30000L;
        long period = _config.ResetTime() * 60 * 1000;
        timer.scheduleAtFixedRate(_rebootTask, delay, period);
    }

    public void IO(IO io) {
        _io = io;
    }

    public void Display(Display display) {
        _display = display;
    }

    public void Offline(Offline offline) {
        _offline = offline;
    }

    public void LPR(LPR lpr) {
        _lpr = lpr;
    }

    public void Tag(Tag tag) {
        _tag = tag;
    }

    public void Scale(Scale scale) {
        _scale = scale;
    }

    public void Controller(Controller controller) {
        _controller = controller;
    }

    public ArrayList<String> LoadOffline() {
        return _offline.Load();
    }

    public void SaveOffline(ArrayList<String> records) {
        _offline.Save(records);
    }

    public void DeleteOffline() {
        _offline.Delete();
    }

    public boolean IsUpload() {
        return _offline.IsUpload();
    }

    public void SetIsUpload(boolean isUpload) {
        _offline.SetIsUpload(isUpload);
    }

    public void Connect() {
        if (_controller != null)
            _controller.Connect();

        if (_tag != null)
            _tag.Connect();

        if (_io != null)
            _io.Connect();

        if (_scale != null)
            _scale.Connect();

        if (_lpr != null)
            _lpr.Connect();
    }

    public void Run(boolean isRun) {
        _isRunning = isRun;
        if (_controller != null)
            _controller.Run(_isRunning);

        if (_tag != null)
            _tag.Run(_isRunning);

        if (_io != null)
            _io.Run(_isRunning);

        if (_scale != null)
            _scale.Run(_isRunning);

        if (_lpr != null)
            _lpr.Run(_isRunning);

        if (_offline != null)
            _offline.Run(_isRunning);

        if (_display != null)
            _display.SetVisible(true);
    }

    public void Acknowledge(String rsp) {
        ResponseArgs data = (new Json<ResponseArgs>()).fromJson(rsp, ResponseArgs.class);
        if (_display != null) {
            if (!data.isSucceded) {
                _display.Message(data.msg, 5000);
                _display.ErrorBackground(5000);
            } else {
                _display.Message("Have a great day! :-)", 5000);
                _display.SuccessBackground(5000);
            }
        }

    }
}