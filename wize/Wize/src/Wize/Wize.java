package Wize;

public class Wize {
	public static void main(String[] args) {

		Director _director = Director.getInstance();
		_director.Build();
		_director.LoadModule();
		_director.Connect();
		_director.Start();

		/*
		 * if (!Engine.loadConfig()) return;
		 * 
		 * Engine eng = Engine.getInstance(WizeModules.valueOf(Configuration.Module));
		 * 
		 * eng.Start();
		 */
	}
}