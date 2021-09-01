package Wize.Modules;

import Wize.Requests.Request;
import Wize.Configurations.ModuleConfiguration;
import Wize.Requests.HTTPSend;
import Wize.Requests.RequestArgs;
import Wize.Requests.WeightRequestArgs;

public class Weight extends Module {
    public Weight(ModuleConfiguration config) {
        super(config);
    }

    @Override
    public void Run(boolean isRun) {
        try {
            super.Run(isRun);

            while (_isRunning) {
                if (_scale.IsDataRecieved()) {
                    if (_config.DisplayConfig().IsDisplay())
                        _display.Weight(_scale.Data(), 5000);
                    RequestArgs args = new WeightRequestArgs();
                    args._mac = _mac;
                    args._weight = _scale.Data();
                    Request req = new Request(new HTTPSend(_config.Web(), null), args);
                    req.Send();
                }
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}