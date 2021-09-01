package Wize.Modules;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.Timer;
import Wize.Requests.HTTPSend;
import Wize.Requests.Request;
import Wize.Requests.RequestArgs;
import Wize.Requests.WeightRequestArgs;
import Wize.Responses.IResponse;
import Wize.Responses.ResponseArgs;
import Siemens.SiemensCommands;
import Utilities.Json;
import Wize.Configurations.ModuleConfiguration;

public class Binyamina extends Module {
    private boolean isFirstUp = false;
    private boolean isSecondUp = false;

    public Binyamina(ModuleConfiguration config) {
        super(config);
    }

    @Override
    public void Run(boolean isRun) {
        try {
            super.Run(isRun);

            while (_isRunning) {
                if (_controller.IsDataRecieved()) {
                    switch (_controller.Data()) {
                        case "I001":
                            isFirstUp = true;
                            break;
                        case "I000":
                            isFirstUp = false;
                            break;
                        case "I011":
                            isSecondUp = true;
                            break;
                        case "I010":
                            isSecondUp = false;
                            break;
                    }
                    _controller.ClearData();
                }

                // WriteToLog("Info", WeightData);
                if (_scale.IsDataRecieved()) {
                    RequestArgs args = new WeightRequestArgs();
                    args._mac = _mac;
                    args._weight = _scale.Data();
                    Request req = new Request(new HTTPSend(_config.Web(), null), args);
                    req.Send();
                }

                if (isFirstUp || isSecondUp)
                    _tag.ClearData();
                /*
                 * if(IsFirstUp && !IsSecondUp && IsWeight) {
                 */
                if (_tag.IsDataRecieved()) {
                    RequestArgs args = new RequestArgs();
                    args._mac = _mac;
                    args._tag = _tag.Data();
                    args._weight = _scale.Data();
                    Request req = new Request(new HTTPSend(_config.Web(), new IResponse() {

                        @Override
                        public void Response(String rsp) {
                            try {
                                Acknowledge(rsp);
                            } catch (Exception ex) {
                                System.out.println(ex.getMessage());
                            }
                        }
                    }), args);
                    req.Send();
                    isFirstUp = false;
                    isSecondUp = false;
                    _tag.ClearData();
                }
                // }

                // IsWeight = false;
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

    public void Acknowledge(String rsp) {

        super.Acknowledge(rsp);

        ResponseArgs data = (new Json<ResponseArgs>()).fromJson(rsp, ResponseArgs.class);
        if (_controller != null && data.isSucceded) {
            _controller.Send(SiemensCommands.Q031);
            Timer t = new Timer(5000, new ActionListener() {

                @Override
                public void actionPerformed(ActionEvent e) {
                    _controller.Send(SiemensCommands.Q030);
                }
            });
            t.setRepeats(false);
            t.start();
        }
    }

}