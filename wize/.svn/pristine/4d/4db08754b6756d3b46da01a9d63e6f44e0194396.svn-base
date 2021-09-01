package Scales;

import java.io.IOException;

import ScaleReaders.ConnectionArgs;
import ScaleReaders.ErrorListener;
import ScaleReaders.ScaleHeaders;
import ScaleReaders.ScaleReader;
import ScaleReaders.WeightArgs;
import ScaleReaders.WeightListener;

public class Shkila implements IScale {
    private ConnectionArgs m_args = null;
    private ScaleReader m_sr = null;
    private ScaleHeaders m_scaleHeader;
    private String m_data = null;
    private boolean m_isDataRecieved = false;

    public Shkila(ScaleHeaders scaleHeader, ConnectionArgs args) {
        m_args = args;
        m_scaleHeader = scaleHeader;
    }

    @Override
    public void run() {
        m_sr.run();

    }

    @Override
    public void close() throws IOException {
        Disconnect();
    }

    @Override
    public boolean Connect() {
        m_sr = ScaleReader.Factory(m_scaleHeader, m_args);
        // m_sr = ScaleReader.Factory(ScaleHeaders.valueOf(Configuration.Scale),
        // m_args);
        // m_sr = ScaleReader.Factory(ScaleHeaders.Test, connArgs);
        m_sr.OnErrorListener = new ErrorListener() {

            @Override
            public void Error(String error) {
                System.out.println("Error:" + error);
            }
        };

        m_sr.OnWeightListener = new WeightListener() {

            @Override
            public void Weight(WeightArgs e) {
                try {
                    if (e != null) {
                        m_data = e.Weight;

                        m_isDataRecieved = true;
                        // sendWeight(m_weightData);
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        };
        m_sr.Connect();

        return m_sr != null;
    }

    @Override
    public void Disconnect() {
        if (m_sr != null)
            m_sr.Disconnect();
        m_sr = null;
    }

    @Override
    public void Validate() {

    }

    @Override
    public void Run(boolean isRun) {
        Thread sr = new Thread(() -> {
            m_sr.IsRunning = isRun;
            if (m_sr.IsRunning)
                run();
            else
                Disconnect();

        });
        sr.start();

    }

    @Override
    public String Data() {
        return m_data;
    }

    @Override
    public boolean IsDataRecieved() {
        return m_isDataRecieved;
    }

    @Override
    public void ClearData() {
        m_data = null;
        m_isDataRecieved = false;
    }

}