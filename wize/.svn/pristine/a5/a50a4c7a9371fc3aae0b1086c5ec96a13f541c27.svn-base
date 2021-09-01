package Wize;

import Wize.Builders.ModuleBuilder;
import Wize.Builders.ModuleBuilderFactory;
import Wize.Configurations.ModuleConfiguration;
import Wize.Modules.Module;

public class Director {
    private static String m_activeDirectory = "file:///" + System.getProperty("user.dir").replace("\\", "/");
    private static String m_configFile = m_activeDirectory + "/Configuration.xml";
    private ModuleBuilder _builder = null;
    private ModuleConfiguration _config = null;
    private Module _module = null;
    private static Director m_instance = null;
    private static Object m_lock = new Object();

    private Director() {
        try {
            _config = new ModuleConfiguration();
            _config.LoadModuleConfiguration(m_configFile);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static Director getInstance() {
        synchronized (m_lock) {
            if (m_instance == null)
                m_instance = new Director();
        }
        return m_instance;
    }

    public void Build() {
        _builder = ModuleBuilderFactory.Factory(_config);
        _builder.Build();

    }

    public void LoadModule() {
        _module = _builder.GetModule();
    }

    public void Connect() {
        _module.Connect();
    }

    public void Start() {
        _module.Run(true);
    }

    public void Stop() {
        _module.Run(false);
    }
}