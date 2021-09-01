package Wize.Builders;

import java.util.ArrayList;
import java.util.TimerTask;

import Offline.Offline;
import Offline.OfflineArgs;
import Offline.OfflineHandler;
import ScaleReaders.ConnectionArgs;
import ScaleReaders.ConnectionType;
import ScaleReaders.ScaleHeaders;
import Scales.Scale;
import Scales.Shkila;
import Utilities.Json;
import Utilities.Network;
import Wize.Requests.HTTPSend;
import Wize.Requests.Request;
import Wize.Requests.RequestArgs;
import Wize.Responses.IResponse;
import Wize.Responses.ResponseArgs;
import Wize.Configurations.ModuleConfiguration;
import Wize.Modules.Module;
import Wize.Modules.Weight;

public class WeightBuilder extends ModuleBuilder {

    public WeightBuilder(ModuleConfiguration config) {
        _config = config;
        _module = new Weight(_config);
    }

    @Override
    public Scale BuildScale() {
        ConnectionArgs connArgs = new ConnectionArgs();
        connArgs.BaudRate = 9600; //
        connArgs.Com = _config.ScaleConfig().Com();
        connArgs.DataBits = 8;
        connArgs.Dtr = false;
        connArgs.Parity = 0;
        connArgs.Rts = false;
        connArgs.StopBits = 1;
        connArgs.Type = _config.ScaleConfig().Com() == "" || _config.ScaleConfig().Com().length() == 0
                ? ConnectionType.Tcp
                : ConnectionType.Serial;
        return new Scale(new Shkila(ScaleHeaders.valueOf(_config.ScaleConfig().Type()), connArgs));
    }

    @Override
    public Offline BuildOffline() {
        OfflineArgs args = new OfflineArgs();
        if (_config.OfflineInterval() > 0) {

            args.Interval = _config.OfflineInterval();
            args.Task = new TimerTask() {
                public void run() {
                    try {
                        if (Network.IsPing()) {

                            ArrayList<String> records = _module.LoadOffline();
                            for (int i = 0; i < records.size(); i++) {
                                RequestArgs args = (new Json<RequestArgs>()).fromJson(records.get(i),
                                        RequestArgs.class);
                                Request req = new Request(new HTTPSend(_config.Web(), new IResponse() {

                                    @Override
                                    public void Response(String rsp) {
                                        try {
                                            ResponseArgs r = (new Json<ResponseArgs>()).fromJson(rsp,
                                                    ResponseArgs.class);
                                            if (r != null) {
                                                if (!r.isSucceded) {
                                                    _module.SetIsUpload(false);
                                                } else if (r.isSucceded) {
                                                    _module.SetIsUpload(true);
                                                }
                                            } else {
                                                System.out.println(rsp);
                                                _module.SetIsUpload(false);
                                            }
                                        } catch (Exception e) {
                                            e.printStackTrace();
                                        }

                                    }
                                }), args);
                                req.Send();
                                if (_module.IsUpload())
                                    records.remove(i--);

                                _module.SetIsUpload(false);
                            }

                            _module.DeleteOffline();
                            _module.SaveOffline(records);
                        }
                    } catch (Exception ex) {
                        ex.printStackTrace();
                    } finally {
                        _module.SetIsUpload(false);
                    }

                }
            };
        }
        return new Offline(new OfflineHandler(args));

    }

    @Override
    public Module GetModule() {
        return _module;
    }

}